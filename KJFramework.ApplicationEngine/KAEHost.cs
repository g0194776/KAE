using System.Linq;
using System.Net;
using KJFramework.ApplicationEngine.Connectors;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Exceptions;
using KJFramework.ApplicationEngine.Finders;
using KJFramework.ApplicationEngine.Helpers;
using KJFramework.ApplicationEngine.Messages;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Packages;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Channels.Uri;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    KAE宿主
    /// </summary>
    public class KAEHost : IKAEHost
    {
        #region Constructor

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        ///     <para>* 使用此构造将会从配置文件中读取服务相关信息</para>
        /// </summary>
        /// <param name="rrcsAddr">远程RRCS服务地址</param>
        public KAEHost(IPEndPoint rrcsAddr)
            : this(Process.GetCurrentProcess().MainModule.FileName.Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf('\\') + 1), rrcsAddr)
        {
        }

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        /// </summary>
        /// <param name="workRoot">工作目录</param>
        /// <param name="rrcsAddr">远程RRCS服务地址</param>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        /// <exception cref="DirectoryNotFoundException">工作目录错误</exception>
        /// <exception cref="ArgumentException">无法找到远程RRCS服务地址</exception>
        public KAEHost(string workRoot, IPEndPoint rrcsAddr)
        {
            if (workRoot == null) throw new ArgumentNullException("workRoot");
            if (!Directory.Exists(workRoot)) throw new DirectoryNotFoundException("Current work root don't existed. #dir: " + workRoot);
            _workRoot = workRoot;
            if (rrcsAddr == null) throw new ArgumentException("#We couldn't find any RRCS remoting address.");
            _rrcsAddr = rrcsAddr;
        }

        #endregion

        #region Members.

        private readonly string _workRoot;
        private readonly IPEndPoint _rrcsAddr;
        private TcpUri _defaultKAENetwork;
        private RRCSConnector _rrcsConnector;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (KAEHost));
        private IList<IHostTransportChannel> _hostChannels;
        private readonly object _protocolDicLockObj = new object();
        //caches for network end-points by respective MessageIdentity.
        //1st level of key = MessageIdentity + App Level; second level of key = application's version.
        private IDictionary<string, IDictionary<string, IList<string>>> _caches;
        private Dictionary<long, Dictionary<ProtocolTypes, IList<string>>> _preparedNetworkCache;
        private IDictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>> _protocolDic;

        /// <summary>
        ///    获取内部运行的应用数量
        /// </summary>
        public ushort ApplicationCount { get; private set; }
        /// <summary>
        ///    获取工作目录
        /// </summary>
        public string WorkRoot { get; private set; }
        /// <summary>
        ///    获取KAE宿主当前状态
        /// </summary>0
        public KAEHostStatus Status { get; private set; }

        #endregion

        #region Methods.

        /// <summary>
        ///    KAE宿主初始化函数
        /// </summary>
        /// <exception cref="DuplicatedApplicationException">
        ///     同时存在多个相同的应用包
        ///     <para>* 对于相同的应用包判断条件为：相同的应用名称+相同的应用版本号+相同的应用等级</para>
        /// </exception>
        private IDictionary<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>> Initialize()
        {
            IDictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> appMetadata = ApplicationFinder.Search(_workRoot);
            if (appMetadata == null || appMetadata.Count == 0) return new Dictionary<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>>();
            //re-composites.
            IDictionary<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>> apps = new Dictionary<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>>();
            foreach (KeyValuePair<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> pair in appMetadata)
            {
                IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>> subDic;
                if (!apps.TryGetValue(pair.Key, out subDic))
                    apps.Add(pair.Key, (subDic = new Dictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>()));
                foreach (Tuple<ApplicationEntryInfo, KPPDataStructure> tuple in pair.Value)
                {
                    Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject> entry;
                    string appFullKey = string.Format("{0}_{1}", tuple.Item2.GetSectionField<string>(0x00, "Version"), tuple.Item2.GetSectionField<byte>(0x00, "ApplicationLevel"));
                    if (subDic.TryGetValue(appFullKey, out entry))
                        throw new DuplicatedApplicationException(
                            string.Format(
                                "#Duplicated application had been found! Details blow:\r\nApplication Name: {0}\r\nVersion: {1}\r\nLevel: {2}",
                                tuple.Item2.GetSectionField<string>(0x00, "PackName"),
                                tuple.Item2.GetSectionField<string>(0x00, "Version"),
                                tuple.Item2.GetSectionField<byte>(0x00, "ApplicationLevel")));
                    //assembles a new dynamic object for application.
                    ApplicationDynamicObject dynamicObject = new ApplicationDynamicObject(tuple.Item1, tuple.Item2);
                    entry = new Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>(tuple.Item1, tuple.Item2, dynamicObject);
                    subDic.Add(appFullKey, entry);
                }
            }
            return apps;
        }

        /// <summary>
        ///    开启KAE宿主
        /// </summary>
        /// <exception cref="ConflictedBasicallyResourceException">KAE应用本地资源冲突异常</exception>
        /// <exception cref="DuplicatedApplicationException">KAE应用的版本或者版本冲突异常</exception>
        public void Start()
        {
            IDictionary<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>> apps = Initialize();
            if (apps == null || apps.Count == 0)
            {
                _tracing.Critical("#KAE Host running on process id {0} had no prepared application!");
                return;
            }
            _hostChannels = new List<IHostTransportChannel>();

            #region #Step 1, Build default network.

            _defaultKAENetwork = InitializeDefaultNetworkResource();

            #endregion

            #region #Step 2, initializes current suported mapping from protocol & message identity & application's level.

            /*
             * We had chosen this kind of comminucation cache because that 
             * The RRCS need decides where the right load balancing addresses are.
             * So, we splicted those of remoting information into different parts which grouped by the applications' version.
             * 
             * P:1,S:2,D:3_Stable
             *      --- 1.1.0
             *          --- tcp://192.168.1.1:8000
             *          --- tcp://192.168.1.2:8000
             *          --- tcp://192.168.1.3:8000
             *          --- tcp://192.168.1.4:8000
             *          --- tcp://192.168.1.5:8000
             *          --- http://192.168.1.1:9000
             *      --- 1.2.0
             *          --- tcp://192.168.1.1:8000
             *          --- tcp://192.168.1.2:8000
             *          --- tcp://192.168.1.3:8000
             *          --- tcp://192.168.1.4:8000
             *          --- tcp://192.168.1.5:8000
             *          --- http://192.168.1.1:9000
             */
            _preparedNetworkCache = new Dictionary<long, Dictionary<ProtocolTypes, IList<string>>>();
            List<Tuple<ApplicationLevel, IList<ProtocolTypes>>> networkResources = new List<Tuple<ApplicationLevel, IList<ProtocolTypes>>>();
            IDictionary<string, IDictionary<string, IList<string>>> caches = new Dictionary<string, IDictionary<string, IList<string>>>();
            Dictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>> protocolDic = new Dictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>>();
            foreach (KeyValuePair<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>> pair in apps)
            {
                foreach (KeyValuePair<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>> subPair in pair.Value)
                {
                    Dictionary<ProtocolTypes, Uri> tmpDic = new Dictionary<ProtocolTypes, Uri>();
                    IDictionary<ProtocolTypes, IList<MessageIdentity>> supportedProtocols = subPair.Value.Item3.AcquireSupportedProtocols();
                    //appending wanted network resources.
                    networkResources.Add(new Tuple<ApplicationLevel, IList<ProtocolTypes>>(subPair.Value.Item3.Level, supportedProtocols.Keys.ToList()));
                    foreach (KeyValuePair<ProtocolTypes, IList<MessageIdentity>> innerPair in supportedProtocols)
                    {
                        InitializeNetworkProtocolHandler(protocolDic, innerPair, subPair.Value.Item3);
                        Uri networkUri = InitializeNetworkResource(NetworkTypes.TCP, innerPair.Key, subPair.Value.Item3.Level);
                        tmpDic.Add(innerPair.Key, networkUri);
                        foreach (MessageIdentity messageIdentity in innerPair.Value)
                        {
                            string identity = string.Format("MSG-IDENTITY: {0}, {1}, {2}; APP-LEVEL: {3};", messageIdentity.ProtocolId, messageIdentity.ServiceId, messageIdentity.DetailsId, subPair.Value.Item3.Level);
                            IDictionary<string, IList<string>> versions;
                            if (!caches.TryGetValue(identity, out versions)) caches.Add(identity, (versions = new Dictionary<string, IList<string>>()));
                            versions.Add(subPair.Value.Item3.Version, new List<string>());
                            IList<string> endpoints;
                            if (!versions.TryGetValue(subPair.Value.Item3.Version, out endpoints)) versions.Add(subPair.Value.Item3.Version, (endpoints = new List<string>()));
                            endpoints.Add(networkUri.ToString());
                        }
                    }
                    #region Collects KAE Host information.

                    //collects current KAE host resources.
                    Dictionary<ProtocolTypes, IList<string>> preparedFirstLevel;
                    if (!_preparedNetworkCache.TryGetValue(subPair.Value.Item3.CRC, out preparedFirstLevel))
                        _preparedNetworkCache.Add(subPair.Value.Item3.CRC, (preparedFirstLevel = new Dictionary<ProtocolTypes, IList<string>>()));
                    IList<string> preparedSecLevel;
                    foreach (KeyValuePair<ProtocolTypes, Uri> collectPair in tmpDic)
                    {
                        if (!preparedFirstLevel.TryGetValue(collectPair.Key, out preparedSecLevel))
                            preparedFirstLevel.Add(collectPair.Key, (preparedSecLevel = new List<string>()));
                        if (!preparedSecLevel.Contains(collectPair.Value.ToString())) preparedSecLevel.Add(collectPair.Value.ToString());
                    }

                    #endregion
                }
            }
            _protocolDic = protocolDic;
            _caches = caches;

            #endregion

            _rrcsConnector = new RRCSConnector(_rrcsAddr, this, _defaultKAENetwork);
            _rrcsConnector.Start();
            Status = KAEHostStatus.Prepared;
        }

        private void InitializeNetworkProtocolHandler(Dictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>> dic, KeyValuePair<ProtocolTypes, IList<MessageIdentity>> values, ApplicationDynamicObject dynamicObject)
        {
            IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>> firstLevel;
            if (!dic.TryGetValue(values.Key, out firstLevel)) dic.Add(values.Key, (firstLevel = new Dictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>()));
            foreach (MessageIdentity identity in values.Value)
            {
                IDictionary<ApplicationLevel, ApplicationDynamicObject> secondLevel;
                if (!firstLevel.TryGetValue(identity, out secondLevel)) firstLevel.Add(identity, (secondLevel = new Dictionary<ApplicationLevel, ApplicationDynamicObject>()));
                if (secondLevel.ContainsKey(dynamicObject.Level)) throw new DuplicatedApplicationException(string.Format("#Duplicated application attributes! #Protocol: {0}, #MessageIdentity: {1}, #Level: {2}.", values.Key, identity, dynamicObject.Level));
                secondLevel.Add(dynamicObject.Level, dynamicObject);
            }
        }

        /// <summary>
        ///     初始化相关的KAE网络资源
        /// </summary>
        /// <param name="network">网络通信类型</param>
        /// <param name="protocol">通信协议类型</param>
        /// <param name="level">应用等级</param>
        /// <exception cref="NotSupportedException">不支持的网络类型</exception>
        /// <exception cref="AllocResourceFailedException">申请网络资源失败</exception>
        private Uri InitializeNetworkResource(NetworkTypes network, ProtocolTypes protocol, ApplicationLevel level)
        {
            Uri uri;
            IHostTransportChannel channel = NetworkHelper.BuildHostChannel(network, out uri);
            channel.ChannelCreated += ChannelCreated;
            if (!channel.Regist())
            {
                channel.ChannelCreated -= ChannelCreated;
                throw new AllocResourceFailedException("#Sadly, We couldn't alloc current network resource. #Uri: " + uri);
            }
            channel.Tag = new Tuple<KAENetworkResource, ApplicationLevel>(new KAENetworkResource { NetworkUri = uri, Protocol = protocol }, level);
            _hostChannels.Add(channel);
            return uri;
        }

        /// <summary>
        ///     初始化KAE宿主默认的网络资源
        /// </summary>
        private TcpUri InitializeDefaultNetworkResource()
        {
            TcpUri uri;
            TcpHostTransportChannel defaultChannel = (TcpHostTransportChannel) NetworkHelper.BuildDefaultHostChannel();
            defaultChannel.ChannelCreated += ChannelCreated;
            if (!defaultChannel.Regist())
            {
                defaultChannel.ChannelCreated -= ChannelCreated;
                throw new AllocResourceFailedException("#Sadly, We couldn't alloc default KAE host network resource. ");
            }
            defaultChannel.Tag = new Tuple<KAENetworkResource, ApplicationLevel>(new KAENetworkResource { NetworkUri = (uri = new TcpUri(string.Format("tcp://localhost:{0}", defaultChannel.Port))), Protocol = ProtocolTypes.Metadata }, ApplicationLevel.Unknown);
            _hostChannels.Add(defaultChannel);
            return uri;
        }

        /// <summary>
        ///    开启KAE宿主
        /// </summary>
        public void Stop()
        {
            foreach (IHostTransportChannel channel in _hostChannels)
            {
                channel.ChannelCreated -= ChannelCreated;
                channel.UnRegist();
            }
            _hostChannels.Clear();
            Status = KAEHostStatus.Stopped;
        }

        //handles redirection business.
        private void HandleBusiness(Tuple<KAENetworkResource, ApplicationLevel> tag, object transaction, MessageIdentity reqMsgIdentity, object reqMsg)
        {
            ApplicationDynamicObject dynamicObj;
            IBusinessPackage package;
            lock (_protocolDicLockObj)
            {
                //Targeted network protocol CANNOT be support.
                IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>> dic;
                if (!_protocolDic.TryGetValue(tag.Item1.Protocol, out dic))
                {
                    HandleErrorSituation(tag.Item1.Protocol, transaction, KAEErrorCodes.NotSupportedNetworkType, "#We'd not supported current network type yet!");
                    return;
                }
                //Targeted MessageIdentity CANNOT be support.
                IDictionary<ApplicationLevel, ApplicationDynamicObject> subDic;
                if (!dic.TryGetValue(reqMsgIdentity, out subDic))
                {
                    HandleErrorSituation(tag.Item1.Protocol, transaction, KAEErrorCodes.NotSupportedMessageIdentity, "#We'd not supported current MessageIdentity yet!");
                    return;
                }
                //Targeted application's level CANNOT be support.
                if (!subDic.TryGetValue(tag.Item2, out dynamicObj))
                {
                    HandleErrorSituation(tag.Item1.Protocol, transaction, KAEErrorCodes.NotSupportedApplicationLevel, "#We'd not supported current application's level yet!");
                    return;
                }
                //acquires a business package for getting the return value from targeted application.
                package = dynamicObj.CreateBusinessPackage();
            }
            package.Failed += delegate { HandleErrorSituation(ProtocolTypes.Metadata, transaction, KAEErrorCodes.TunnelCommunicationFailed, "#Occured failed while communicating with the targeted application."); };
            package.Timeout += delegate { HandleErrorSituation(ProtocolTypes.Metadata, transaction, KAEErrorCodes.TunnelCommunicationTimeout, "#Occured timeout while communicating with the targeted application."); };
            package.ResponseArrived += delegate(object o, LightSingleArgEventArgs<MetadataContainer> args)
            {
                //error situation.
                if (args.Target.GetAttributeAsType<byte>(0x0A) != 0x00)
                    HandleErrorSituation(ProtocolTypes.Metadata, transaction, (KAEErrorCodes)args.Target.GetAttributeAsType<byte>(0x0A), args.Target.GetAttributeAsType<string>(0x0B));
                else HandleSucceedSituation(tag.Item1.Protocol, transaction, KAEErrorCodes.OK, args.Target);
            };
            package.SendRequest(CreateTunnelMessage(tag.Item1.Protocol, reqMsg));
        }

        /*
         * Application's tunnel MSG construction.
         * REQ Message:
         *      0x00 - MessageIdentity
         *      0x01 - TransactionIdentity
         *      0x0A - Protocol Type
         *      0x0B - Specified REQ Message Structure
         * RSP Message:
         *      0x00 - MessageIdentity
         *      0x01 - TransactionIdentity
         *      0x0A - Error Id
         *      0x0B - Error Reason
         *      0x0C - Specified Value
         */
        private MetadataContainer CreateTunnelMessage(ProtocolTypes protocol, object content)
        {
            MetadataContainer msg = new MetadataContainer();
            //RESEND_REQUEST_MESSAGE
            msg.SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity { ProtocolId = 0xFE, ServiceId = 0x04 }));
            msg.SetAttribute(0x0A, new ByteValueStored((byte)protocol));
            switch (protocol)
            {
                case ProtocolTypes.Metadata:
                    msg.SetAttribute(0x0B, new ResourceBlockStored((ResourceBlock)content));
                    break;
                case ProtocolTypes.Intellegence:
                    msg.SetAttribute(0x0B, new IntellectObjectValueStored(IntellectObjectEngine.ToBytes((IIntellectObject)content)));
                    break;
                default:
                    throw new NotSupportedException(string.Format("#We've not supported current protocol: {0}!", protocol));
            }
            return msg;
        }

        private void HandleErrorSituation(ProtocolTypes protocol, object transaction, KAEErrorCodes errorCode, string reason)
        {
            switch (protocol)
            {
                case ProtocolTypes.Metadata:
                    IMessageTransaction<MetadataContainer> msgTransaction = ((IMessageTransaction<MetadataContainer>)transaction);
                    MessageIdentity msgIdentity = msgTransaction.Request.GetAttributeAsType<MessageIdentity>(0x00);
                    msgIdentity.DetailsId += 1;
                    MetadataContainer rspMsg = new MetadataContainer();
                    rspMsg.SetAttribute(0x00, new MessageIdentityValueStored(msgIdentity));
                    rspMsg.SetAttribute(0x0A, new ByteValueStored((byte)errorCode));
                    rspMsg.SetAttribute(0x0B, new StringValueStored(reason));
                    msgTransaction.SendResponse(rspMsg);
                    break;
                case ProtocolTypes.Intellegence:
                    IMessageTransaction<BaseMessage> msgTransaction2 = ((IMessageTransaction<BaseMessage>)transaction);
                    KAEResponseMessage rspMsg2 = ((KAERequestMessage)msgTransaction2.Request).CreateResponseMessage();
                    rspMsg2.ErrorId = (byte)errorCode;
                    rspMsg2.Reason = reason;
                    msgTransaction2.SendResponse(rspMsg2);
                    break;
            }
        }

        private void HandleSucceedSituation(ProtocolTypes protocol, object transaction, KAEErrorCodes errorCode, MetadataContainer rspMessage)
        {
            switch (protocol)
            {
                case ProtocolTypes.Metadata:
                    IMessageTransaction<MetadataContainer> msgTransaction = ((IMessageTransaction<MetadataContainer>)transaction);
                    MessageIdentity msgIdentity = msgTransaction.Request.GetAttributeAsType<MessageIdentity>(0x00);
                    msgIdentity.DetailsId += 1;
                    MetadataContainer rspMsg = new MetadataContainer();
                    rspMsg.SetAttribute(0x00, new MessageIdentityValueStored(msgIdentity));
                    rspMsg.SetAttribute(0x0A, new ByteValueStored((byte)errorCode));
                    rspMsg.SetAttribute(0x0C, rspMessage.GetAttribute(0x0C));
                    msgTransaction.SendResponse(rspMsg);
                    break;
                case ProtocolTypes.Intellegence:
                    IMessageTransaction<BaseMessage> msgTransaction2 = ((IMessageTransaction<BaseMessage>)transaction);
                    KAETunnelTransportResponseMessage rspMsg2 = (KAETunnelTransportResponseMessage)((KAERequestMessage)msgTransaction2.Request).CreateResponseMessage();
                    rspMsg2.ErrorId = (byte)errorCode;
                    rspMsg2.Data = rspMessage.GetAttributeAsType<byte[]>(0x0C);
                    msgTransaction2.SendResponse(rspMsg2);
                    break;
            }
        }

        /// <summary>
        ///     Gets local supported network communication protocols.
        /// </summary>
        /// <returns>returns a dictionary that which contains a group of local supported commmunications end-points.</returns>
        internal Dictionary<long, Dictionary<ProtocolTypes, IList<string>>> GetNetworkCache()
        {
            return _preparedNetworkCache;
        }

        /*  [RSP MESSAGE]
         *  ===========================================
         *      0x00 - Message Identity
         *      0x01 - Transaction Identity
         *      0x0A - Error Id
         *      0x0B - Error Reason
         *      0x0C - Remoting cached End-Points resource blocks (ARRAY)
         *      -------- Internal resource block's structure --------
         *          0x00 - application's level.
         *          0x01 - application's version.
         *          0x02 - targeted application's network resource uri (STRING)
         *          0x01 - targeted application supported all network abilities  (ARRAY)
         *          -------- Internal resource block's structure --------
         *              0x00 - Message Identity
         *              0x01 - Supported Protocol
         */
        /// <summary>
        ///     Update local supported caches.
        /// </summary>
        /// <param name="cache">end-points that which it need to be updating.</param>
        internal void UpdateNetworkCache(Dictionary<string, List<string>> cache)
        {
            lock (_protocolDicLockObj)
                foreach (KeyValuePair<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>> pair in _protocolDic)
                    foreach (KeyValuePair<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>> valuePair in pair.Value)
                        foreach (KeyValuePair<ApplicationLevel, ApplicationDynamicObject> keyValuePair in valuePair.Value) keyValuePair.Value.UpdateNetworkCache(cache);
        }

        private void HandleSystemCommand(IMessageTransaction<MetadataContainer> transaction)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Events.

        void ChannelCreated(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {
            IHostTransportChannel hostChannel = (IHostTransportChannel) sender;
            Tuple<KAENetworkResource, ApplicationLevel> tag = (Tuple<KAENetworkResource, ApplicationLevel>) hostChannel.Tag;
            if (tag.Item1.Protocol == ProtocolTypes.Metadata)
            {
                IMessageTransportChannel<MetadataContainer> msgChannel = new MessageTransportChannel<MetadataContainer>((IRawTransportChannel) e.Target, (IProtocolStack<MetadataContainer>) NetworkHelper.GetProtocolStack(tag.Item1.Protocol));
                MetadataConnectionAgent agent = new MetadataConnectionAgent(msgChannel, (MetadataTransactionManager) NetworkHelper.GetTransactionManager(tag.Item1.Protocol));
                agent.Disconnected += AgentDisconnected;
                agent.NewTransaction += MetadataNewTransaction;
                agent.Tag = tag;
            }
            else if (tag.Item1.Protocol == ProtocolTypes.Intellegence)
            {
                IMessageTransportChannel<BaseMessage> msgChannel = new MessageTransportChannel<BaseMessage>((IRawTransportChannel)e.Target, (IProtocolStack<BaseMessage>)NetworkHelper.GetProtocolStack(tag.Item1.Protocol));
                IntellectObjectConnectionAgent agent = new IntellectObjectConnectionAgent(msgChannel, (MessageTransactionManager) NetworkHelper.GetTransactionManager(tag.Item1.Protocol));
                agent.Disconnected += AgentDisconnected;
                agent.NewTransaction += IntellegenceNewTransaction;
                agent.Tag = tag;
            }
        }

        void AgentDisconnected(object sender, System.EventArgs e)
        {
            IConnectionAgent agent = (IConnectionAgent)sender;
            agent.Disconnected -= AgentDisconnected;
        }

        void MetadataNewTransaction(object sender, LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>> e)
        {
            MetadataConnectionAgent agent = (MetadataConnectionAgent)sender;
            Tuple<KAENetworkResource, ApplicationLevel> tag = (Tuple<KAENetworkResource, ApplicationLevel>)agent.Tag;
            IMessageTransaction<MetadataContainer> transaction = e.Target;
            MetadataContainer reqMsg = transaction.Request;
            MessageIdentity reqMsgIdentity = reqMsg.GetAttributeAsType<MessageIdentity>(0x00);
            /*
             * We always makes a checking on the Metadata protocol network communication. 
             * Because all of ours internal system communications are constructed by this kind of MSG protocol.
             */
            if (reqMsgIdentity.ProtocolId >= 0xFD) HandleSystemCommand(transaction);
            //sends it to the appropriate application.
            else HandleBusiness(tag, transaction, reqMsgIdentity, reqMsg);
        }

        void IntellegenceNewTransaction(object sender, LightSingleArgEventArgs<IMessageTransaction<BaseMessage>> e)
        {
            IntellectObjectConnectionAgent agent = (IntellectObjectConnectionAgent)sender;
            Tuple<KAENetworkResource, ApplicationLevel> tag = (Tuple<KAENetworkResource, ApplicationLevel>)agent.Tag;
            IMessageTransaction<BaseMessage> transaction = e.Target;
            KAERequestMessage reqMsg = (KAERequestMessage)transaction.Request;
            MessageIdentity reqMsgIdentity = reqMsg.MessageIdentity;
            HandleBusiness(tag, transaction, reqMsgIdentity, reqMsg);
        }

        #endregion
    }
}