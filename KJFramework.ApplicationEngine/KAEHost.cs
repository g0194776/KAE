using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Exceptions;
using KJFramework.ApplicationEngine.Extends;
using KJFramework.ApplicationEngine.Factories;
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
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Channels.Uri;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;
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
        /// </summary>
        /// <param name="installingListFile">
        ///     KPP装配清单文件的地址
        ///     <para>* 如果该地址为空则表示使用本地已拥有的KPP进行KAE宿主的初始化操作</para>
        /// </param>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        /// <exception cref="DirectoryNotFoundException">工作目录错误</exception>
        /// <exception cref="ArgumentException">无法找到远程RRCS服务地址</exception>
        public KAEHost(string installingListFile)
            : this(Process.GetCurrentProcess().MainModule.FileName.Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf('\\') + 1), installingListFile, new DefaultInternalResourceFactory(), new SolitaryRemoteConfigurationProxy())
        {
        }

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        /// </summary>
        /// <param name="workRoot">工作目录</param>
        /// <param name="installingListFile">
        ///     KPP装配清单文件的地址
        ///     <para>* 如果该地址为空则表示使用本地已拥有的KPP进行KAE宿主的初始化操作</para>
        /// </param>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        /// <exception cref="DirectoryNotFoundException">工作目录错误</exception>
        /// <exception cref="ArgumentException">无法找到远程RRCS服务地址</exception>
        public KAEHost(string workRoot, string installingListFile = null)
            : this(workRoot, installingListFile, new DefaultInternalResourceFactory(), new SolitaryRemoteConfigurationProxy())
        {
        }

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        ///     <para>* 使用此构造将会从配置文件中读取服务相关信息</para>
        /// </summary>
        /// <param name="installingListFile">
        ///     KPP装配清单文件的地址
        ///     <para>* 如果该地址为空则表示使用本地已拥有的KPP进行KAE宿主的初始化操作</para>
        /// </param>
        /// <param name="configurationProxy">远程配置站访问代理器</param>
        public KAEHost(string installingListFile = null, IRemoteConfigurationProxy configurationProxy = null)
            : this(Process.GetCurrentProcess().MainModule.FileName.Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf('\\') + 1), installingListFile, new DefaultInternalResourceFactory(), configurationProxy)
        {
        }

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        ///     <para>* 使用此构造将会从配置文件中读取服务相关信息</para>
        /// </summary>
        /// <param name="installingListFile">
        ///     KPP装配清单文件的地址
        ///     <para>* 如果该地址为空则表示使用本地已拥有的KPP进行KAE宿主的初始化操作</para>
        /// </param>
        /// <param name="resourceFactory">内部资源工厂</param>
        /// <param name="configurationProxy">远程配置站访问代理器</param>
        internal KAEHost(string installingListFile = null, IInternalResourceFactory resourceFactory = null, IRemoteConfigurationProxy configurationProxy = null)
            : this(Process.GetCurrentProcess().MainModule.FileName.Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf('\\') + 1), installingListFile, resourceFactory, configurationProxy)
        {
        }

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        /// </summary>
        /// <param name="workRoot">工作目录</param>
        /// <param name="installingListFile">
        ///     KPP装配清单文件的地址
        ///     <para>* 如果该地址为空则表示使用本地已拥有的KPP进行KAE宿主的初始化操作</para>
        /// </param>
        /// <param name="configurationProxy">远程配置站访问代理器</param>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        /// <exception cref="DirectoryNotFoundException">工作目录错误</exception>
        /// <exception cref="ArgumentException">无法找到远程RRCS服务地址</exception>
        public KAEHost(string workRoot, string installingListFile = null, IRemoteConfigurationProxy configurationProxy = null)
            : this(workRoot, installingListFile, new DefaultInternalResourceFactory(), configurationProxy)
        {
        }

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        /// </summary>
        /// <param name="workRoot">工作目录</param>
        /// <param name="installingListFile">
        ///     KPP装配清单文件的地址
        ///     <para>* 如果该地址为空则表示使用本地已拥有的KPP进行KAE宿主的初始化操作</para>
        /// </param>
        /// <param name="resourceFactory">内部资源工厂</param>
        /// <param name="configurationProxy">远程配置站访问代理器</param>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        /// <exception cref="DirectoryNotFoundException">工作目录错误</exception>
        /// <exception cref="ArgumentException">无法找到远程RRCS服务地址</exception>
        internal KAEHost(string workRoot, string installingListFile = null, IInternalResourceFactory resourceFactory = null, IRemoteConfigurationProxy configurationProxy = null)
        {
            if (workRoot == null) throw new ArgumentNullException("workRoot");
            if (resourceFactory == null) throw new ArgumentNullException("resourceFactory");
            if (!Directory.Exists(workRoot)) throw new DirectoryNotFoundException("Current work root don't existed. #dir: " + workRoot);
            _workRoot = workRoot;
            _installingListFile = installingListFile;
            _configurationProxy = (configurationProxy ?? new SolitaryRemoteConfigurationProxy());
            KAESystemInternalResource.Factory = resourceFactory;
            if (string.IsNullOrEmpty(_installingListFile))
            {
                _tracing.DebugInfo("#Probing whether have a file named \"installing.kl\"...", ConsoleColor.DarkGray);
                _installingListFile = (File.Exists(Path.Combine(workRoot, "installing.kl")) ? Path.Combine(workRoot, "installing.kl") : null);
            }
            _usedInstallingListFile = !string.IsNullOrEmpty(_installingListFile);
        }

        #endregion

        #region Members.

        private string _workRoot;
        private string _greyPolicyAddress;
        private Thread _greyPolicyThread;
        private TimeSpan _greyPolicyInterval;
        private TcpUri _defaultKAENetwork;
        private readonly bool _usedInstallingListFile;
        private readonly string _installingListFile;
        private ChannelInternalConfigSettings _settings;
        private readonly object _appDicLockObj = new object();
        private readonly object _protocolDicLockObj = new object();
        private readonly IRemoteConfigurationProxy _configurationProxy;
        private readonly IKAEHostProxy _hostProxy = new KAEHostProxy();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(KAEHost));

        #region Performance Counters.

        private readonly LightPerfCounter _rspRemainningCounter = new NumberOfItems64PerfCounter("KAE::COMMUNICATION::RSP::REMAINNING", "It used for counting how many RSP messages are waitting for sends to the remoting network resource.");
        private readonly LightPerfCounter _errorRspCounter = new NumberOfItems64PerfCounter("KAE::COMMUNICATION::RSP::ERROR", "It used for counting how many RSP messages had occured error."); 
        
        #endregion

        private readonly IDictionary<Guid, ApplicationDynamicObject> _activeApps = new Dictionary<Guid, ApplicationDynamicObject>();
        //caches for network end-points by respective MessageIdentity.
        //1st level of key = MessageIdentity + App Level; second level of key = application's version.
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
            _tracing.DebugInfo("\t#Initializing grey policy...");
            _greyPolicyAddress = SystemWorker.ConfigurationProxy.GetField("KAEWorker", "GreyPolicyAddress", (KAESettings.IsTDDTesting ? (Action<string>)null : (address => _greyPolicyAddress = address)));
            _greyPolicyInterval = TimeSpan.Parse(SystemWorker.ConfigurationProxy.GetField("KAEWorker", "GreyPolicyInternal", (KAESettings.IsTDDTesting ? (Action<string>)null : (interval => _greyPolicyInterval = TimeSpan.Parse(interval)))));
            _tracing.DebugInfo("\t#Hooking remoting configuration proxy's event...");
            SystemWorker.ConfigurationProxy.ConfigurationUpdated += ConfigurationUpdatedEvent;
            _tracing.DebugInfo("\t#Initializing default KPP network setting template...");
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
            _tracing.DebugInfo("\t#Initializing network resources...");
            if (!KAEHostNetworkResourceManager.IsInitialized)
            {
                KAEHostNetworkResourceManager.IntellegenceNewTransaction += IntellegenceNewTransaction;
                KAEHostNetworkResourceManager.MetadataNewTransaction += MetadataNewTransaction;
                KAEHostNetworkResourceManager.Initialize();
            }
            _tracing.DebugInfo("\t#Loading KPPs...");
            IDictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> appMetadata = ((IApplicationFinder)KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.APPFinder)).Search(workRoot);
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
                    {
                        _tracing.Warn(string.Format(
                                "#Duplicated application had been found! Details blow:\r\n\tApplication Name: {0}\r\n\tVersion: {1}\r\n\tLevel: {2}",
                                tuple.Item2.GetSectionField<string>(0x00, "PackName"),
                                tuple.Item2.GetSectionField<string>(0x00, "Version"),
                                tuple.Item2.GetSectionField<byte>(0x00, "ApplicationLevel")));
                        continue;
                    }
                    //assembles a new dynamic object for application.
                    ApplicationDynamicObject dynamicObject = new ApplicationDynamicObject(tuple.Item1, tuple.Item2, _settings, _hostProxy);
                    entry = new Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>(tuple.Item1, tuple.Item2, dynamicObject);
                    subDic.Add(appFullKey, entry);
                    _activeApps[dynamicObject.GlobalUniqueId] = dynamicObject;
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
            _tracing.DebugInfo("#Initializing from remoting CSN...");
            SystemWorker.Initialize("KAEWorker", RemoteConfigurationSetting.Default, _configurationProxy);
            _tracing.DebugInfo("#Initializing KAE internal system resource factory...");
            KAESystemInternalResource.Factory.Initialize();
            //Downloads & Initializes remoting KPPs by an installing list file.
            if (_usedInstallingListFile) _workRoot = ((IRemotingApplicationDownloader) KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.APPDownloader)).Download(_workRoot, _installingListFile);
            _tracing.DebugInfo("#Initializing KAE hosting...");
            IDictionary<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>> apps = Initialize(_workRoot);
            if (apps == null || apps.Count == 0)
            {
                Status = KAEHostStatus.Prepared;
                _tracing.Critical("#KAE Host has started with process id {0} BUT there wasn't any prepared application!");
                return;
            }

            #region #Step 1, Build default network.

            _defaultKAENetwork = (TcpUri) KAEHostNetworkResourceManager.GetResourceUri(ProtocolTypes.INTERNAL_SPECIAL_RESOURCE);

            #endregion

            #region #Step 2, initializes current suported mapping from protocol & message identity & application's level.

            _tracing.DebugInfo("#Preparing Internal Data For Remoting RRCS...");

            IRemotingProtocolRegister protocolRegister = (IRemotingProtocolRegister) KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.ProtocolRegister);
            protocolRegister.ChildrenChanged += RemoteResourceChanged;
            List<Tuple<ApplicationLevel, IList<ProtocolTypes>>> networkResources = new List<Tuple<ApplicationLevel, IList<ProtocolTypes>>>();
            Dictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>> protocolDic = new Dictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, ApplicationDynamicObject>>>();
            foreach (KeyValuePair<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>> pair in apps)
            {
                foreach (KeyValuePair<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>> subPair in pair.Value)
                {
                    IDictionary<ProtocolTypes, IList<MessageIdentity>> supportedProtocols = subPair.Value.Item3.AcquireSupportedProtocols();
                    //appending wanted network resources.
                    networkResources.Add(new Tuple<ApplicationLevel, IList<ProtocolTypes>>(subPair.Value.Item3.Level, supportedProtocols.Keys.ToList()));
                    foreach (KeyValuePair<ProtocolTypes, IList<MessageIdentity>> innerPair in supportedProtocols)
                    {
                        InitializeNetworkProtocolHandler(protocolDic, innerPair, subPair.Value.Item3);
                        Uri networkUri = KAEHostNetworkResourceManager.GetResourceUri(innerPair.Key);
                        if (networkUri == null) throw new AllocResourceFailedException("#There wasn't any network resource can be supported. #Protocol: " + innerPair.Key);
                        //registers network protocol to remoting service.
                        foreach (MessageIdentity identity in innerPair.Value)
                            protocolRegister.Register(identity, innerPair.Key, subPair.Value.Item3.Level, networkUri);
                    }
                }
            }
            _protocolDic = protocolDic;

            #endregion

            _tracing.DebugInfo("#Preparing background job for grey policy...");
            GetGreyPolicyAsync();
            Status = KAEHostStatus.Prepared;
            _tracing.DebugInfo("#KAE host has been initialized, STATUS: {0}.", ConsoleColor.DarkGreen, Status);
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
                                    lock (_appDicLockObj)
                                        foreach (KeyValuePair<Guid, ApplicationDynamicObject> pair in _activeApps) pair.Value.UpdateGreyPolicy(code);
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
        ///    更新网络缓存信息
        /// </summary>
        /// <param name="level">应用等级</param>
        /// <param name="cache">远程目标终结点信息列表</param>
        /// <param name="protocol">通信协议</param>
        /// <param name="protocolTypes">协议类型</param>
        internal void UpdateCache(Protocols protocol, ProtocolTypes protocolTypes, ApplicationLevel level, List<string> cache)
        {
            lock (_appDicLockObj)
                foreach (KeyValuePair<Guid, ApplicationDynamicObject> pair in _activeApps) pair.Value.UpdateCache(protocol, protocolTypes, level, cache);
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
            Tuple<KAENetworkResource, ApplicationLevel> tag = new Tuple<KAENetworkResource, ApplicationLevel>((KAENetworkResource)agent.Tag, (reqMsg.IsAttibuteExsits(0x05) ? (ApplicationLevel)reqMsg.GetAttributeAsType<byte>(0x05) : ApplicationLevel.Stable));
            MessageIdentity reqMsgIdentity = reqMsg.GetAttributeAsType<MessageIdentity>(0x00);
            /*
             * We always makes a checking on the Metadata protocol network communication. 
             * Because all of ours internal system communications are constructed by this kind of MSG protocol.
             */
            if (reqMsgIdentity.ProtocolId >= 0xFC) HandleSystemCommand(transaction);
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

        //sent from ZooKeeper that it indicates the remote network resource had been changed.
        void RemoteResourceChanged(object sender, LightSingleArgEventArgs<IProtocolResource> e)
        {
            IList<string> result = e.Target.GetResult();
            UpdateCache(e.Target.Protocol, e.Target.ProtocolTypes, e.Target.Level, (List<string>) result);
        }

        #endregion
    }
}