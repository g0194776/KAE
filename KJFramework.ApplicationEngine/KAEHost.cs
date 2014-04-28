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
        public KAEHost()
            : this(Process.GetCurrentProcess().MainModule.FileName.Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf('\\') + 1))
        {
        }

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        /// </summary>
        /// <param name="workRoot">工作目录</param>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        /// <exception cref="DirectoryNotFoundException">工作目录错误</exception>
        public KAEHost(String workRoot)
        {
            if (workRoot == null) throw new ArgumentNullException("workRoot");
            if (!Directory.Exists(workRoot)) throw new DirectoryNotFoundException("Current work root don't existed. #dir: " + workRoot);
            _workRoot = workRoot;
        }

        #endregion

        #region Members.

        private readonly string _workRoot;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (KAEHost));
        private IList<IHostTransportChannel> _hostChannels;
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
        /// </summary>
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
        public void Start()
        {
            IDictionary<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>> apps = Initialize();
            if (apps == null || apps.Count == 0)
            {
                _tracing.Critical("#KAE Host running on process id {0} had no prepared application!");
                return;
            }
            _hostChannels = new List<IHostTransportChannel>();

            #region Step 1, Build default network.

            InitializeDefaultNetworkResource();

            #endregion

            #region #Step 2, initializes current suported mapping from protocol & message identity & application's level.

            List<Tuple<ApplicationLevel, IList<KAENetworkResource>>> networkResources = new List<Tuple<ApplicationLevel, IList<KAENetworkResource>>>();
            Dictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>> protocolDic = new Dictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>>();
            foreach (KeyValuePair<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>> pair in apps)
            {
                foreach (KeyValuePair<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>> subPair in pair.Value)
                {
                    IDictionary<ProtocolTypes, IList<MessageIdentity>> supportedProtocols = subPair.Value.Item3.AcquireSupportedProtocols();
                    IList<KAENetworkResource> communicationSupport = subPair.Value.Item3.AcquireCommunicationSupport();
                    //appending wanted network resources.
                    if (communicationSupport != null && communicationSupport.Count != 0) networkResources.Add(new Tuple<ApplicationLevel, IList<KAENetworkResource>>(subPair.Value.Item3.Level, communicationSupport));
                    foreach (KeyValuePair<ProtocolTypes, IList<MessageIdentity>> innerPair in supportedProtocols) InitializeNetworkProtocolHandler(protocolDic, innerPair, subPair.Value.Item3);
                }
            }
            _protocolDic = protocolDic;

            #endregion

            #region #Step 3, initializes network resource by that mapping relations.

            foreach (Tuple<ApplicationLevel, IList<KAENetworkResource>> tuple in networkResources)
                foreach (KAENetworkResource resource in tuple.Item2) InitializeNetworkResource(resource, tuple.Item1);
            
            #endregion

            Status = KAEHostStatus.Started;
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
        /// <param name="resource">KAE网络资源</param>
        /// <param name="level">应用等级</param>
        /// <exception cref="NotSupportedException">不支持的网络类型</exception>
        /// <exception cref="AllocResourceFailedException">申请网络资源失败</exception>
        private void InitializeNetworkResource(KAENetworkResource resource, ApplicationLevel level)
        {
            IHostTransportChannel channel = NetworkHelper.BuildHostChannel(resource);
            channel.ChannelCreated += ChannelCreated;
            if (!channel.Regist())
            {
                channel.ChannelCreated -= ChannelCreated;
                throw new AllocResourceFailedException("#Sadly, We couldn't alloc current network resource. #Resource: " + resource.NetworkUri);
            }
            channel.Tag = new Tuple<KAENetworkResource, ApplicationLevel>(resource, level);
            _hostChannels.Add(channel);
        }

        /// <summary>
        ///     初始化KAE宿主默认的网络资源
        /// </summary>
        private void InitializeDefaultNetworkResource()
        {
            IHostTransportChannel defaultChannel = NetworkHelper.BuildDefaultHostChannel();
            defaultChannel.ChannelCreated += ChannelCreated;
            if (!defaultChannel.Regist())
            {
                defaultChannel.ChannelCreated -= ChannelCreated;
                throw new AllocResourceFailedException("#Sadly, We couldn't alloc default KAE host network resource. ");
            }
            defaultChannel.Tag = new Tuple<KAENetworkResource, ApplicationLevel>(new KAENetworkResource{NetworkUri = new TcpUri("tcp://localhost:*"), Protocol = ProtocolTypes.Metadata}, ApplicationLevel.Unknown);
            _hostChannels.Add(defaultChannel);
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
            IConnectionAgent agent = (IConnectionAgent) sender;
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
            KAERequestMessage reqMsg = (KAERequestMessage) transaction.Request;
            MessageIdentity reqMsgIdentity = reqMsg.MessageIdentity;
            HandleBusiness(tag, transaction, reqMsgIdentity, reqMsg);
        }

        //handles redirection business.
        private void HandleBusiness(Tuple<KAENetworkResource, ApplicationLevel> tag, object transaction, MessageIdentity reqMsgIdentity, object reqMsg)
        {
            IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>> dic;
            IDictionary<ApplicationLevel, ApplicationDynamicObject> subDic;
            ApplicationDynamicObject dynamicObj;
            //Targeted network protocol CANNOT be support.
            if (!_protocolDic.TryGetValue(tag.Item1.Protocol, out dic))
            {
                HandleErrorSituation(tag.Item1.Protocol, transaction, KAEErrorCodes.NotSupportedNetworkType, "#We'd not supported current network type yet!");
                return;
            }
            //Targeted MessageIdentity CANNOT be support.
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
            IBusinessPackage package = dynamicObj.CreateBusinessPackage();
            package.Failed += delegate { HandleErrorSituation(ProtocolTypes.Metadata, transaction, KAEErrorCodes.TunnelCommunicationFailed, "#Occured failed while communicating with the targeted application."); };
            package.Timeout += delegate { HandleErrorSituation(ProtocolTypes.Metadata, transaction, KAEErrorCodes.TunnelCommunicationTimeout, "#Occured timeout while communicating with the targeted application."); };
            package.ResponseArrived += delegate(object o, LightSingleArgEventArgs<MetadataContainer> args)
            {
                IMessageTransaction<MetadataContainer> targetedTransaction = (IMessageTransaction<MetadataContainer>) transaction;
                //error situation.
                if (args.Target.GetAttributeAsType<byte>(0x0A) != 0x00)
                    HandleErrorSituation(ProtocolTypes.Metadata, targetedTransaction, (KAEErrorCodes)args.Target.GetAttributeAsType<byte>(0x0A), args.Target.GetAttributeAsType<string>(0x0B));
                else
                {
                    MessageIdentity msgIdentity = targetedTransaction.Request.GetAttributeAsType<MessageIdentity>(0x00);
                    msgIdentity.DetailsId += 1;
                    MetadataContainer rspMsg = new MetadataContainer();
                    rspMsg.SetAttribute(0x00, new MessageIdentityValueStored(msgIdentity));
                    rspMsg.SetAttribute(0x0A, new ByteValueStored((byte) KAEErrorCodes.OK));
                    targetedTransaction.SendResponse(rspMsg);
                }
            };
            package.SendRequest(CreateTunnelMessage(tag.Item1.Protocol, reqMsg));
        }

        /*
         * Application's tunnel MSG construction.
         * REQ Message:
         *      0x00 - MessageIdentity
         *      0x01 - TransactionIdentity
         *      0x0A - Specified Value
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
            msg.SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity{ProtocolId = 0xFE, ServiceId = 0x04}));
            switch (protocol)
            {
                case ProtocolTypes.Metadata:
                    msg.SetAttribute(0x0A, new ResourceBlockStored((ResourceBlock) content));
                    break;
                case ProtocolTypes.Intellegence:
                    msg.SetAttribute(0x0A, new IntellectObjectValueStored(IntellectObjectEngine.ToBytes((IIntellectObject)content)));
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
                    IMessageTransaction<MetadataContainer> msgTransaction = ((IMessageTransaction<MetadataContainer>) transaction);
                    MessageIdentity msgIdentity = msgTransaction.Request.GetAttributeAsType<MessageIdentity>(0x00);
                    msgIdentity.DetailsId += 1;
                    MetadataContainer rspMsg = new MetadataContainer();
                    rspMsg.SetAttribute(0x00, new MessageIdentityValueStored(msgIdentity));
                    rspMsg.SetAttribute(0x0A, new ByteValueStored((byte) errorCode));
                    rspMsg.SetAttribute(0x0B, new StringValueStored(reason));
                    msgTransaction.SendResponse(rspMsg);
                    break;
                case ProtocolTypes.Intellegence:
                    IMessageTransaction<BaseMessage> msgTransaction2 = ((IMessageTransaction<BaseMessage>)transaction);
                    KAEResponseMessage rspMsg2 = ((KAERequestMessage) msgTransaction2.Request).CreateResponseMessage();
                    rspMsg2.ErrorId = (byte) errorCode;
                    rspMsg2.Reason = reason;
                    msgTransaction2.SendResponse(rspMsg2);
                    break;
            }
        }

        private void HandleSystemCommand(IMessageTransaction<MetadataContainer> transaction)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}