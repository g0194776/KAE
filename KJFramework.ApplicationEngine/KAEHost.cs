using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using KJFramework.ApplicationEngine.Commands;
using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Exceptions;
using KJFramework.ApplicationEngine.Factories;
using KJFramework.ApplicationEngine.Finders;
using KJFramework.ApplicationEngine.Loggers;
using KJFramework.ApplicationEngine.Managers;
using KJFramework.ApplicationEngine.Messages;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Counters;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Configurations;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Channels.Uri;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    KAE宿主
    /// </summary>
    public sealed class KAEHost : IKAEHost
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
        private readonly IRemoteConfigurationProxy _configurationProxy;
        private readonly IKAEResourceProxy _hostProxy = new KAEHostResourceProxy();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(KAEHost));
        private readonly IKAEHostAppManager _hostedAppManager = new KAEHostAppManager();

        #region Performance Counters.

        private readonly LightPerfCounter _rspRemainningCounter = new NumberOfItems64PerfCounter("KAE::COMMUNICATION::RSP::REMAINNING", "It used for counting how many RSP messages are waitting for sends to the remoting network resource.");
        private readonly LightPerfCounter _errorRspCounter = new NumberOfItems64PerfCounter("KAE::COMMUNICATION::RSP::ERROR", "It used for counting how many RSP messages had occured error."); 
        
        #endregion

        private readonly IKAEStateLogger _stateLogger = new KAEStateLogger(_tracing);

        /// <summary>
        ///    获取内部运行的应用数量
        /// </summary>
        public ushort ApplicationCount { get; private set; }
        /// <summary>
        ///    获取工作目录
        /// </summary>
        public string WorkRoot { get; private set; }

        /// <summary>
        ///    获取当前KAE宿主实例的唯一名称
        /// </summary>
        public string UniqueName {
            get
            {
                return Dns.GetHostName();
            }
        }

        /// <summary>
        ///    获取KAE宿主当前状态
        /// </summary>0
        public KAEHostStatus Status { get; private set; }
        /// <summary>
        ///    获取KAE宿主对于KPP实例的配置信息
        /// </summary>
        internal ChannelInternalConfigSettings ChannelInternalConfigSettings
        {
            get
            {
                return _settings;
            }
        }
        /// <summary>
        ///     获取KAE宿主的资源代理器
        /// </summary>
        internal IKAEResourceProxy ResourceProxy
        {
            get
            {
                return _hostProxy;
            }
        }

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
            _tracing.DebugInfo("\t#Loading KPPs...");
            //probing apps from specified file path: $WORK-PATHH\install-apps\
            IDictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> appMetadata = ((IApplicationFinder)KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.APPFinder)).Search(Path.Combine(workRoot, "install-apps"));
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
                    ApplicationDynamicObject dynamicObject = new ApplicationDynamicObject(tuple.Item1, tuple.Item2, _settings, _hostProxy, HandleSucceedSituation, HandleErrorSituation);
                    entry = new Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>(tuple.Item1, tuple.Item2, dynamicObject);
                    subDic.Add(appFullKey, entry);
                    _hostedAppManager.RegisterApp(dynamicObject);
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
            _tracing.DebugInfo("\t#Initializing network resources...");
            if (!KAEHostNetworkResourceManager.IsInitialized)
            {
                KAEHostNetworkResourceManager.IntellegenceNewTransaction += IntellegenceNewTransaction;
                KAEHostNetworkResourceManager.MetadataNewTransaction += MetadataNewTransaction;
                KAEHostNetworkResourceManager.Initialize();
            }
            _defaultKAENetwork = (TcpUri)KAEHostNetworkResourceManager.GetResourceUri(ProtocolTypes.INTERNAL_SPECIAL_RESOURCE);
            _tracing.DebugInfo("#Register itself as an resouce to remote ZooKeeper...");
            IRemotingProtocolRegister protocolRegister = (IRemotingProtocolRegister)KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.ProtocolRegister);
            protocolRegister.Initialize(UniqueName, _defaultKAENetwork);
            protocolRegister.ChildrenChanged += RemoteResourceChanged;
            //Downloads & Initializes remoting KPPs by an installing list file.
            if (_usedInstallingListFile) _workRoot = ((IRemotingApplicationDownloader)KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.APPDownloader)).DownloadFromList(_workRoot, _installingListFile);
            _tracing.DebugInfo("#Initializing KAE hosting...");
            IDictionary<string, IDictionary<string, Tuple<ApplicationEntryInfo, KPPDataStructure, ApplicationDynamicObject>>> apps = Initialize(_workRoot);
            if (apps == null || apps.Count == 0)
            {
                Status = KAEHostStatus.Prepared;
                _tracing.Critical("#KAE Host has started with process id {0} BUT there wasn't any prepared application!");
                return;
            }

            _tracing.DebugInfo("#Initializing KAE hosting command executor...");
            KAECommandsExector.Initialize();
            _tracing.DebugInfo("#Preparing background job for grey policy...");
            GetGreyPolicyAsync();
            Status = KAEHostStatus.Prepared;
            _tracing.DebugInfo("#KAE host has been initialized, STATUS: {0}.", ConsoleColor.DarkGreen, Status);
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
                                if (!string.IsNullOrEmpty(code)) _hostedAppManager.UpdateGreyPolicy(code);
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
        private void HandleBusiness(Tuple<KAENetworkResource, ApplicationLevel> tag, object transaction, MessageIdentity reqMsgIdentity, object reqMsg, Guid kppUniqueId)
        {
            _rspRemainningCounter.Increment();
            ApplicationDynamicObject app = _hostedAppManager.GetApp(kppUniqueId);
            if (app == null)
            {
                HandleErrorSituation(tag.Item1.Protocol, transaction, KAEErrorCodes.SpecifiedKPPNotFound, "#Specified KPP's unique ID had not found!");
                return;
            }
            app.HandleBusiness(tag, transaction, reqMsgIdentity, reqMsg);
        }

        internal void HandleErrorSituation(ProtocolTypes protocol, object transaction, KAEErrorCodes errorCode, string reason)
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

        internal void HandleSucceedSituation(ProtocolTypes protocol, object transaction, KAEErrorCodes errorCode, MetadataContainer rspMessage)
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
            Guid uniqueId = reqMsg.GetAttributeAsType<Guid>(0x03);
            /*
             * We always makes a checking on the Metadata protocol network communication. 
             * Because all of ours internal system communications are constructed by this kind of MSG protocol.
             */
            if (reqMsgIdentity.ProtocolId >= 0xFC) HandleSystemCommand(transaction);
            //sends it to the appropriate application.
            else HandleBusiness(tag, transaction, reqMsgIdentity, reqMsg, uniqueId);
        }

        void IntellegenceNewTransaction(object sender, LightSingleArgEventArgs<IMessageTransaction<BaseMessage>> e)
        {
            IntellectObjectConnectionAgent agent = (IntellectObjectConnectionAgent)sender;
            IMessageTransaction<BaseMessage> transaction = e.Target;
            KAERequestMessage reqMsg = (KAERequestMessage)transaction.Request;
            Tuple<KAENetworkResource, ApplicationLevel> tag = new Tuple<KAENetworkResource, ApplicationLevel>((KAENetworkResource)agent.Tag, reqMsg.RequestedLevel);
            MessageIdentity reqMsgIdentity = reqMsg.MessageIdentity;
            HandleBusiness(tag, transaction, reqMsgIdentity, reqMsg, reqMsg.KPPUniqueId);
        }

        //Received a message from remoting CSN.
        void ConfigurationUpdatedEvent(object sender, LightSingleArgEventArgs<Tuple<string, string>> e)
        {
            _hostedAppManager.UpdateConfiguration(e.Target.Item1, e.Target.Item2);
        }

        //sent from ZooKeeper that it indicates the remote network resource had been changed.
        void RemoteResourceChanged(object sender, LightSingleArgEventArgs<IProtocolResource> e)
        {
            IProtocolResource resource = e.Target;
            List<string> result = (List<string>)resource.GetResult();
            IEnumerable<Guid> apps = resource.GetInterestedApps();
            foreach (Guid uniqueId in apps)
            {
                ApplicationDynamicObject app = _hostedAppManager.GetApp(uniqueId);
                if (app == null) resource.UnRegisterInterestedApp(uniqueId);
                else app.UpdateCache(resource.Protocol, resource.ProtocolTypes, resource.Level, result);
            }
        }

        #endregion
    }
}