using KJFramework.ApplicationEngine.Eums;
using KJFramework.Data.Synchronization;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.Data.Synchronization.Factories;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Channels.Uri;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.ValueStored;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.Connectors
{
    /// <summary>
    ///     RRCS服务连接器
    /// </summary>
    internal class RRCSConnector
    {
        #region Constructors.

        /// <summary>
        ///     RRCS服务连接器
        /// </summary>
        /// <param name="rrcsAddr">远程RRCS服务地址</param>
        /// <param name="host">KAE宿主</param>
        /// <param name="defaultKAENetwork">KAE宿主的默认网络通信地址</param>
        public RRCSConnector(IPEndPoint rrcsAddr, KAEHost host, TcpUri defaultKAENetwork)
        {
            _rrcsAddr = rrcsAddr;
            _host = host;
            _defaultKAENetwork = defaultKAENetwork;
        }

        #endregion

        #region Members.

        private readonly KAEHost _host;
        private readonly TcpUri _defaultKAENetwork;
        private readonly IPEndPoint _rrcsAddr;
        private MetadataConnectionAgent _agent;
        private Thread _thread;
        private bool _enable;
        private bool _connectedToRRCS;
        private IRemoteDataSubscriber<string, string[]> _subscriber;
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private static readonly TimeSpan _sleepInterval = TimeSpan.Parse("00:00:05");
        private static readonly MetadataProtocolStack _protocolStack = new MetadataProtocolStack();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(RRCSConnector));
        private static readonly MetadataTransactionManager _transactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());

        #endregion

        #region Methods.

        public void Start()
        {
            _enable = true;
            _thread = new Thread(delegate()
            {
                while (_enable)
                {
                    if (!_connectedToRRCS)
                    {
                        RegisterToRRCS();
                        _resetEvent.WaitOne();
                    }
                    Thread.Sleep(_sleepInterval);
                }
            }) {Name = "RRCS_CONNECTOR_BACKGROUND_THREAD", IsBackground = true};
            _thread.Start();
        }

        public void Stop()
        {
            _enable = false;
            if (_thread != null)
            {
                try { _thread.Abort(); } catch { }
                _thread = null;
            }
            _connectedToRRCS = false;
        }

        /// <summary>
        ///    Ensures that there had kept a connection to remoting RRCS.
        /// </summary>
        /// <returns></returns>
        private bool EnsureConnection()
        {
            if (_agent != null && _agent.GetChannel().IsConnected) return true;
            ITransportChannel channel = new TcpTransportChannel(_rrcsAddr);
            channel.Connect();
            if (!channel.IsConnected) return false;
            IMessageTransportChannel<MetadataContainer> msgChannel = new MessageTransportChannel<MetadataContainer>((IRawTransportChannel) channel, _protocolStack);
            _agent = new MetadataConnectionAgent(msgChannel, _transactionManager);
            _agent.Disconnected += AgentDisconnected;
            return true;
        }

        /*
         *  Register to RRCS message structures:
         *  [REQ MESSAGE]
         *  ===========================================
         *      0x00 - Message Identity
         *      0x01 - Transaction Identity
         *      0x02 - KAE-Host's Default Communication Port
         *      0x03 - Register Source Type
         *      0x0A - Application's Information (ARRAY)
         *      -------- Internal resource block's structure --------
         *          0x00 - application's CRC.
         *          0x01 - application's network resource blocks  (ARRAY)
         *          -------- Internal resource block's structure --------
         *              0x00 - supported newtork protocol.
         *              0x01 - network end-points.  (STRING ARRAY).
         * 
         *  [RSP MESSAGE]
         *  ===========================================
         *      0x00 - Message Identity
         *      0x01 - Transaction Identity
         *      0x0A - Error Id
         *      0x0B - Error Reason
         *      0x0C - Remoting cached End-Points resource blocks (ARRAY)
         *      -------- Internal resource block's structure --------
         *          0x00 - application's network identity.
         *          0x01 - application's end-points (STRING ARRAY).
         *      0x0D - Current Registed Process' Token (GUID).
         *      0x0E - Remoting RRCS Publisher Accessed Uri.
         */
        private void RegisterToRRCS()
        {
            if (!EnsureConnection())
            {
                _resetEvent.Set();
                return;
            }
            MetadataContainer reqMsg = CreateRequestMessage();
            if (reqMsg == null)
            {
                _resetEvent.Set();
                return;
            }
            reqMsg.SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity { ProtocolId = 0xFC, ServiceId = 0x00 }));
            MessageTransaction<MetadataContainer> transaction = _agent.CreateTransaction();
            transaction.Failed += delegate
            {
                _resetEvent.Set(); 
                _tracing.Critical("#A communication of register to RRCS had been failed.");
            };
            transaction.Failed += delegate
            {
                _resetEvent.Set();
                _tracing.Critical("#A communication of register to RRCS had been timeout.");
            };
            transaction.ResponseArrived += delegate(object sender, LightSingleArgEventArgs<MetadataContainer> args)
            {
                _resetEvent.Set();
                MetadataContainer rspMsg = args.Target;
                if (rspMsg.IsAttibuteExsits(0x0A) && rspMsg.GetAttributeAsType<byte>(0x0A) != 0x00)
                {
                    _tracing.Critical("#Register to RRCS failed by reason: {0}", rspMsg.GetAttributeAsType<string>(0x0B));
                    return;
                }
                _connectedToRRCS = true;
                DescribeRRCS(rspMsg.GetAttributeAsType<string>(0x0E));
                AnalyizeRRCSResult(rspMsg);
            };
            transaction.SendRequest(reqMsg);
        }

        private MetadataContainer CreateRequestMessage()
        {
            Dictionary<long, Dictionary<ProtocolTypes, IList<string>>> caches = _host.GetNetworkCache();
            if (caches.Count == 0) return null;
            MetadataContainer reqMsg = new MetadataContainer();
            reqMsg.SetAttribute(0x02, new Int32ValueStored(_defaultKAENetwork.Port));
            reqMsg.SetAttribute(0x03, new ByteValueStored((byte) KAEHostRegisterSourceTypes.Service));
            ResourceBlock[] blocks = new ResourceBlock[caches.Count];
            reqMsg.SetAttribute(0x0A, new ResourceBlockArrayStored(blocks));
            int offset = 0;
            foreach (KeyValuePair<long, Dictionary<ProtocolTypes, IList<string>>> pair in caches)
            {
                ResourceBlock block = (blocks[offset] = new ResourceBlock());
                ResourceBlock[] innerBlocks = new ResourceBlock[pair.Value.Count];
                block.SetAttribute(0x00, new Int64ValueStored(pair.Key));
                block.SetAttribute(0x01, new ResourceBlockArrayStored(innerBlocks));
                int innerOffset = 0;
                foreach (KeyValuePair<ProtocolTypes, IList<string>> innerPair in pair.Value)
                {
                    ResourceBlock innerBlock = (innerBlocks[innerOffset] = new ResourceBlock());
                    innerBlock.SetAttribute(0x00, new ByteValueStored((byte) innerPair.Key));
                    innerBlock.SetAttribute(0x01, new StringArrayValueStored(innerPair.Value.ToArray()));
                    innerOffset++;
                }
                offset++;
            }
            return reqMsg;
        }

        /*  [RSP MESSAGE]
         *  ===========================================
         *      0x00 - Message Identity
         *      0x01 - Transaction Identity
         *      0x0A - Error Id
         *      0x0B - Error Reason
         *      0x0C - Remoting cached End-Points resource blocks (ARRAY)
         *      -------- Internal resource block's structure --------
         *          0x00 - application's identity.
         *          0x01 - application's end-points (STRING ARRAY).
         */
        private void AnalyizeRRCSResult(MetadataContainer rspMsg)
        {
            ResourceBlock[] blocks;
            if (!rspMsg.IsAttibuteExsits(0x0C) || (blocks = rspMsg.GetAttributeAsType<ResourceBlock[]>(0x0C)) == null) return;
            if (blocks.Length == 0) return;
            Dictionary<string, List<string>> caches = new Dictionary<string, List<string>>();
            foreach (ResourceBlock block in blocks)
            {
                List<string> firstLevel;
                if(!caches.TryGetValue(block.GetAttributeAsType<string>(0x00), out firstLevel))
                    caches.Add(block.GetAttributeAsType<string>(0x00), (firstLevel = new List<string>()));
                firstLevel.AddRange(block.GetAttributeAsType<string[]>(0x01));
            }
            _host.UpdateNetworkCache(caches);
        }

        /// <summary>
        ///    订阅远程RRCS服务的发布者
        /// </summary>
        /// <param name="rrcsPublisherUri">远程RRCS服务的发布者地址</param>
        private void DescribeRRCS(string rrcsPublisherUri)
        {
            rrcsPublisherUri = rrcsPublisherUri.Substring(rrcsPublisherUri.LastIndexOf("://") + 3);
            _subscriber = DataSubscriberFactory.Instance.Create<string, string[]>("*", new NetworkResource(rrcsPublisherUri), true);
            _subscriber.MessageRecv += SubscriberMessageRecv;
            _subscriber.Open();
        }

        #region Events.
        void SubscriberMessageRecv(object sender, LightSingleArgEventArgs<DataRecvEventArgs<string, string[]>> e)
        {
            Dictionary<string, List<string>> caches = new Dictionary<string, List<string>>();
            caches.Add(e.Target.Key, new List<string>(e.Target.Value));
            _host.UpdateNetworkCache(caches);
        }

        void AgentDisconnected(object sender, System.EventArgs e)
        {
            MetadataConnectionAgent agent = (MetadataConnectionAgent)sender;
            agent.Disconnected -= AgentDisconnected;
            _connectedToRRCS = false;
            _agent = null;
        }

        #endregion

        #endregion
    }
}