using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Connectors;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Exceptions;
using KJFramework.ApplicationEngine.Extends;
using KJFramework.ApplicationEngine.Finders;
using KJFramework.ApplicationEngine.Managers;
using KJFramework.ApplicationEngine.Messages;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Packages;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Counters;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Configurations;
using KJFramework.Net.Channels.Extends;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Channels.Uri;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
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
        /// <exception cref="ArgumentException">无法找到远程RRCS服务地址</exception>
        public KAEHost(string workRoot)
        {
            if (workRoot == null) throw new ArgumentNullException("workRoot");
            if (!Directory.Exists(workRoot)) throw new DirectoryNotFoundException("Current work root don't existed. #dir: " + workRoot);
            _workRoot = workRoot;
        }

        #endregion

        #region Members.

        private string _greyPolicyAddress;
        private Thread _greyPolicyThread;
        private readonly string _workRoot;
        private IPEndPoint _rrcsAddr;
        private TimeSpan _greyPolicyInterval;
        private TcpUri _defaultKAENetwork;
        private RRCSConnector _rrcsConnector;
        private ChannelInternalConfigSettings _settings;
        private readonly IKAEHostProxy _hostProxy = new KAEHostProxy();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (KAEHost));
        private readonly object _protocolDicLockObj = new object();
        #region Performance Counters.

        private readonly LightPerfCounter _rspRemainningCounter = new NumberOfItems64PerfCounter("KAE::COMMUNICATION::RSP::REMAINNING", "It used for counting how many RSP messages are waitting for sends to the remoting network resource.");
        private readonly LightPerfCounter _errorRspCounter = new NumberOfItems64PerfCounter("KAE::COMMUNICATION::RSP::ERROR", "It used for counting how many RSP messages had occured error."); 
        
        #endregion
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
        /// <param name="workRoot">KAE宿主初始化时搜寻KPP文件的目录地址</param>
        /// <exception cref="DuplicatedApplicationException">
        ///     同时存在多个相同的应用包
        ///     <para>* 对于相同的应用包判断条件为：相同的应用名称+相同的应用版本号+相同的应用等级</para>
        /// </exception>
        private IDictionary<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>> Initialize(string workRoot)
        {
            _greyPolicyAddress = SystemWorker.Instance.ConfigurationProxy.GetField("KAEWorker","GreyPolicyAddress", address => _greyPolicyAddress = address);
            _greyPolicyInterval = TimeSpan.Parse(SystemWorker.Instance.ConfigurationProxy.GetField("KAEWorker", "GreyPolicyInternal", interval => _greyPolicyInterval = TimeSpan.Parse(interval)));
            SystemWorker.Instance.ConfigurationProxy.ConfigurationUpdated += ConfigurationUpdatedEvent;
            //does a copy of current AppDomain's global network layer settings for each of installing KPP.
            _settings = new ChannelInternalConfigSettings
            {
                BuffStubPoolSize = ChannelConst.BuffStubPoolSize,
                MaxMessageDataLength = ChannelConst.MaxMessageDataLength,
                NamedPipeBuffStubPoolSize = ChannelConst.NamedPipeBuffStubPoolSize,
                NoBuffStubPoolSize = ChannelConst.NoBuffStubPoolSize,
                RecvBufferSize = ChannelConst.RecvBufferSize,
                SegmentSize = ChannelConst.SegmentSize
            };
            if (!KAEHostNetworkResourceManager.IsInitialized)
            {
                KAEHostNetworkResourceManager.IntellegenceNewTransaction += IntellegenceNewTransaction;
                KAEHostNetworkResourceManager.MetadataNewTransaction += MetadataNewTransaction;
                KAEHostNetworkResourceManager.Initialize();
            }
            IDictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> appMetadata = ApplicationFinder.Search(workRoot);
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
                    ApplicationDynamicObject dynamicObject = new ApplicationDynamicObject(tuple.Item1, tuple.Item2, _settings, _hostProxy);
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
            SystemWorker.Instance.Initialize("KAEWorker", RemoteConfigurationSetting.Default);
            string rrcsAddr = SystemWorker.Instance.ConfigurationProxy.GetField("KAEWorker", "RRCS-Address");
            if (rrcsAddr == null) throw new ArgumentException("#We couldn't find any RRCS address from remoting CSN.");
            _rrcsAddr = rrcsAddr.ConvertToIPEndPoint();
            IDictionary<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>> apps = Initialize(_workRoot);
            if (apps == null || apps.Count == 0)
            {
                _tracing.Critical("#KAE Host running on process id {0} had no prepared application!");
                return;
            }

            #region #Step 1, Build default network.

            _defaultKAENetwork = (TcpUri) KAEHostNetworkResourceManager.GetResourceUri(ProtocolTypes.INTERNAL_SPECIAL_RESOURCE);

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
                        Uri networkUri = KAEHostNetworkResourceManager.GetResourceUri(innerPair.Key);
                        if (networkUri == null) throw new AllocResourceFailedException("#There wasn't any network resource can be supported. #Protocol: " + innerPair.Key);
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
            GetGreyPolicyAsync();
            Status = KAEHostStatus.Prepared;
            Thread.CurrentThread.Join();
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
        ///    异步的从指定远程地址上获取灰度升级策略脚本
        /// </summary>
        private void GetGreyPolicyAsync()
        {
            _greyPolicyThread = new Thread(delegate()
            {
                while (true)
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_greyPolicyAddress);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                string code = reader.ReadToEnd();
                                if (!string.IsNullOrEmpty(code))
                                {
                                    lock (_protocolDicLockObj)
                                        foreach (KeyValuePair<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>> pair in _protocolDic)
                                            foreach (KeyValuePair<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>> valuePair in pair.Value)
                                                foreach (KeyValuePair<ApplicationLevel, ApplicationDynamicObject> keyValuePair in valuePair.Value) keyValuePair.Value.UpdateGreyPolicy(code);
                                }
                            }
                        }
                    }
                    catch { }
                    Thread.Sleep(_greyPolicyInterval);
                }
            })
            {
                IsBackground = true,
                Name = "KAE-HOST_BACKEND_THREAD_GET_GREY_POLICY"
            };
            _greyPolicyThread.Start();
        }

        /// <summary>
        ///    开启KAE宿主
        /// </summary>
        public void Stop()
        {
            Status = KAEHostStatus.Stopped;
        }

        /*
         *  处理外部请求的总入口
         *  
         *  [RSP MESSAGE]
         *  ===========================================
         *      0x00 - Message Identity
         *      0x01 - Transaction Identity
         *      0x02 - Requested Targeting APP Level (REQUIRED)
         *      ...
         *      Other Business Fields
         */
        private void HandleBusiness(Tuple<KAENetworkResource, ApplicationLevel> tag, object transaction, MessageIdentity reqMsgIdentity, object reqMsg)
        {
            _rspRemainningCounter.Increment();
            ApplicationDynamicObject dynamicObj;
            BusinessPackage package;
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
                package = (BusinessPackage) dynamicObj.CreateBusinessPackage();
            }
            package.Transaction.Failed += delegate { HandleErrorSituation(ProtocolTypes.Metadata, transaction, KAEErrorCodes.TunnelCommunicationFailed, "#Occured failed while communicating with the targeted application."); };
            package.Transaction.Timeout += delegate { HandleErrorSituation(ProtocolTypes.Metadata, transaction, KAEErrorCodes.TunnelCommunicationTimeout, "#Occured timeout while communicating with the targeted application."); };
            package.Transaction.ResponseArrived += delegate(object o, LightSingleArgEventArgs<MetadataContainer> args)
            {
                package.State = BusinessPackageStates.ReceivedDeliveryResponse;
                //error situation.
                if (args.Target.GetAttributeByIdSafety<byte>(0x0A) != 0x00)
                    HandleErrorSituation(ProtocolTypes.Metadata, transaction, (KAEErrorCodes)args.Target.GetAttributeAsType<byte>(0x0A), args.Target.GetAttributeAsType<string>(0x0B));
                else HandleSucceedSituation(tag.Item1.Protocol, transaction, KAEErrorCodes.OK, args.Target);
            };
            package.Transaction.SendRequest(CreateTunnelMessage(tag.Item1.Protocol, reqMsg));
            package.State = BusinessPackageStates.Delivered;
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
            _rspRemainningCounter.Decrement();
            _errorRspCounter.Increment();
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
            _rspRemainningCounter.Decrement();
            switch (protocol)
            {
                case ProtocolTypes.Metadata:
                    IMessageTransaction<MetadataContainer> msgTransaction = ((IMessageTransaction<MetadataContainer>)transaction);
                    MessageIdentity msgIdentity = msgTransaction.Request.GetAttributeAsType<MessageIdentity>(0x00);
                    msgIdentity.DetailsId += 1;
                    MetadataContainer rspMsg = new MetadataContainer();
                    rspMsg.SetAttribute(0x00, new MessageIdentityValueStored(msgIdentity));
                    rspMsg.SetAttribute(0x0A, new ByteValueStored((byte)errorCode));
                    if (rspMessage.IsAttibuteExsits(0x0C)) rspMsg.SetAttribute(0x0C, rspMessage.GetAttribute(0x0C));
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

        void MetadataNewTransaction(object sender, LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>> e)
        {
            MetadataConnectionAgent agent = (MetadataConnectionAgent)sender;
            IMessageTransaction<MetadataContainer> transaction = e.Target;
            MetadataContainer reqMsg = transaction.Request;
            Tuple<KAENetworkResource, ApplicationLevel> tag = new Tuple<KAENetworkResource, ApplicationLevel>((KAENetworkResource)agent.Tag, (ApplicationLevel)reqMsg.GetAttributeAsType<byte>(0x02));
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
            IMessageTransaction<BaseMessage> transaction = e.Target;
            KAERequestMessage reqMsg = (KAERequestMessage)transaction.Request;
            Tuple<KAENetworkResource, ApplicationLevel> tag = new Tuple<KAENetworkResource, ApplicationLevel>((KAENetworkResource)agent.Tag, reqMsg.RequestedLevel);
            MessageIdentity reqMsgIdentity = reqMsg.MessageIdentity;
            HandleBusiness(tag, transaction, reqMsgIdentity, reqMsg);
        }

        //Received a message from remoting CSN.
        void ConfigurationUpdatedEvent(object sender, LightSingleArgEventArgs<Tuple<string, string>> e)
        {
            lock (_protocolDicLockObj)
                foreach (KeyValuePair<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>> pair in _protocolDic)
                    foreach (KeyValuePair<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>> valuePair in pair.Value)
                        foreach (KeyValuePair<ApplicationLevel, ApplicationDynamicObject> keyValuePair in valuePair.Value) keyValuePair.Value.UpdateConfiguration(e.Target.Item1, e.Target.Item2);
        }

        #endregion
    }
}