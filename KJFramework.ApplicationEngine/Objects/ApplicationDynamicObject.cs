using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Exceptions;
using KJFramework.ApplicationEngine.Extends;
using KJFramework.ApplicationEngine.Managers;
using KJFramework.ApplicationEngine.Packages;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Dynamic;
using KJFramework.Dynamic.Components;
using KJFramework.Enums;
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
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;
using ILease = System.Runtime.Remoting.Lifetime.ILease;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.ApplicationEngine.Objects
{
    /// <summary>
    ///     KAE应用动态对象
    /// </summary>
    internal class ApplicationDynamicObject : MarshalByRefObject, IApplication
    {
        #region Constructor.

        /// <summary>
        ///     KAE应用动态对象
        /// </summary>
        /// <param name="info">应用入口结构信息</param>
        /// <param name="structure">KPP应用资源包的数据结构</param>
        /// <param name="settings">每个APP所使用的网络资源配置集</param>
        /// <param name="proxy">KAE宿主代理器</param>
        /// <param name="handleSucceedSituation">业务处理成功场景下的系统回调函数</param>
        /// <param name="handleErrorSituation">业务处理失败场景下的系统回调函数</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="CannotConnectToTunnelException">无法建立正常的隧道连接</exception>
        public ApplicationDynamicObject(ApplicationEntryInfo info, KPPDataStructure structure, ChannelInternalConfigSettings settings, IKAEResourceProxy proxy, Action<ProtocolTypes, object, KAEErrorCodes, MetadataContainer> handleSucceedSituation, Action<ProtocolTypes, object, KAEErrorCodes, string> handleErrorSituation)
        {
            if (info == null) throw new ArgumentNullException("info");
            if (structure == null) throw new ArgumentNullException("structure");
            if (settings == null) throw new ArgumentNullException("settings");
            _proxy = proxy;
            _entryInfo = info;
            _settings = settings;
            _structure = structure;
            _handleErrorSituation = handleErrorSituation;
            _handleSucceedSituation = handleSucceedSituation;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("#NEW COMPONENT...");
            builder.AppendLine("   PATH: " + info.FolderPath);
            builder.AppendLine("   FILE: " + info.FilePath);
            builder.AppendLine("   ENTRYPOINT: " + info.EntryPoint);
            builder.AppendLine("   CRC: " + info.FileCRC);
            WorkProcessingHandler(new LightSingleArgEventArgs<string>(builder.ToString()));
            PreInitialize();
            InitializeProtocols();
            InitializeTunnel();
        }

        #endregion

        #region Members.

        private int _unRspCount;
        private Application _application;
        protected AppDomain _domain;
        private readonly IKAEResourceProxy _proxy;
        private readonly KPPDataStructure _structure;
        private readonly ApplicationEntryInfo _entryInfo;
        private readonly ChannelInternalConfigSettings _settings;
        private IServerConnectionAgent<MetadataContainer> _agent;
        private readonly Action<ProtocolTypes, object, KAEErrorCodes, string> _handleErrorSituation;
        private static readonly MetadataProtocolStack _protocolStack = new MetadataProtocolStack();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ApplicationDynamicObject));
        private readonly Action<ProtocolTypes, object, KAEErrorCodes, MetadataContainer> _handleSucceedSituation;
        private IDictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, int>>> _protocolDic;
        private static readonly MetadataTransactionManager _transactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());

        /// <summary>
        ///    获取应用版本
        /// </summary>
        public string Version
        {
            get { return _application.Version; }
        }

        /// <summary>
        ///    获取应用描述
        /// </summary>
        public string Description
        {
            get { return _application.Description; }
        }

        /// <summary>
        ///    获取应用包名
        /// </summary>
        public string PackageName
        {
            get { return _application.PackageName; }
        }

        /// <summary>
        ///    获取应用的全局唯一编号
        /// </summary>
        public Guid GlobalUniqueId
        {
            get { return _application.GlobalUniqueId; }
        }

        /// <summary>
        ///    获取应用当前的状态
        /// </summary>
        public ApplicationStatus Status
        {
            get { return _application.Status; }
        }

        /// <summary>
        ///     获取名称
        /// </summary>
        public string Name
        {
            get { return _application.Name; }
        }

        /// <summary>
        ///    获取应用等级
        /// </summary>
        public ApplicationLevel Level
        {
            get { return _application.Level; }
        }

        /// <summary>
        ///    获取应用kpp文件的CRC
        /// </summary>
        public long CRC
        {
            get { return _application.CRC; }
        }

        /// <summary>
        ///     检查当前组件的健康状况
        /// </summary>
        /// <returns>返回健康状况</returns>
        public HealthStatus CheckHealth()
        {
            return _application.CheckHealth();
        }

        /// <summary>
        ///     获取唯一标示
        /// </summary>
        public Guid Id
        {
            get { return _application.Id; }
        }

        /// <summary>
        ///      获取或设置可用标示
        /// </summary>
        public bool Enable
        {
            get { return _application.Enable; }
            set { _application.Enable = value; }
        }

        /// <summary>
        ///     获取或设置插件信息
        /// </summary>
        public PluginInfomation PluginInfo
        {
            get { return _application.PluginInfo; }
        }

        /// <summary>
        ///    获取一个值，该值标示了当前KPP包裹是否包含了一个完整的运行环境所需要的所有依赖文件
        /// </summary>
        public bool IsCompletedEnvironment
        {
            get { return _application.IsCompletedEnvironment; }
        }

        /// <summary>
        ///    获取内部所使用的隧道连接地址
        /// </summary>
        public string TunnelAddress 
        {
            get
            {
                return _application.TunnelAddress;
            }
        }

        [Obsolete("#Sadly, We had not supported this property.", true)]
        public IDynamicDomainService OwnService { get; set; }

        /// <summary>
        ///     获取一个值，该值标示了当前KPP中未返回RSP应答的业务数
        /// </summary>
        public int UnRspCount
        {
            get
            {
                return _unRspCount;
            }
        }

        #endregion

        #region Methods.

        private void PreInitialize()
        {
            try
            {
                if (_domain == null)
                {
                    AppDomainSetup setup = new AppDomainSetup();
                    setup.ShadowCopyFiles = "true";
                    setup.CachePath = "C:\\AssemblyCached";
                    setup.ConfigurationFile = _entryInfo.FilePath + ".config";
                    setup.ApplicationBase = (_structure.GetSectionField<bool>(0x00, "IsCompletedEnvironment") ? _entryInfo.FolderPath : AppDomain.CurrentDomain.BaseDirectory);
                    setup.ApplicationName = _entryInfo.EntryPoint;
                    setup.ShadowCopyDirectories = _entryInfo.FolderPath;
                    setup.PrivateBinPath = AppDomain.CurrentDomain.BaseDirectory;
                    String componentName = _entryInfo.EntryPoint.Substring(_entryInfo.EntryPoint.LastIndexOf('.') + 1);
                    _domain = AppDomain.CreateDomain("{APPDOMAIN:" + componentName + "}", null, setup);
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("Create domain {0} succeed.", _domain.FriendlyName)));
                    _domain.UnhandledException += DomainUnhandledException;
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>("Creating object handle......"));
                    //specially path processed for supporting basic Application class.
                    ObjectHandle cls;
                    if (_entryInfo.EntryPoint != typeof(Application).FullName) cls = _domain.CreateInstanceFrom(_entryInfo.FilePath, _entryInfo.EntryPoint);
                    else cls = _domain.CreateInstanceFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kjframework.applicationengine.dll"), _entryInfo.EntryPoint);
                    if (cls != null)
                    {
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>("Unwrapping......"));
                        Application app = (Application)cls.Unwrap();
                        _application = app;
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>("Trying to renew application life......"));
                        ReLease(new TimeSpan(365, 0, 0, 0));
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>("Calling OnLoading method......"));
                        _application.Initialize(_structure, _settings, _proxy);
                        _application.OnLoading();
                        _application.Start();
                    }
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex);
                throw;
            }
        }

        private void InitializeProtocols()
        {
            IRemotingProtocolRegister protocolRegister = (IRemotingProtocolRegister)KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.ProtocolRegister);
            Dictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, int>>> protocolDic = new Dictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, int>>>();
            IDictionary<ProtocolTypes, IList<MessageIdentity>> supportedProtocols = AcquireSupportedProtocols();
            foreach (KeyValuePair<ProtocolTypes, IList<MessageIdentity>> innerPair in supportedProtocols)
            {
                InitializeNetworkProtocolHandler(protocolDic, innerPair);
                Uri networkUri = KAEHostNetworkResourceManager.GetResourceUri(innerPair.Key);
                if (networkUri == null) throw new AllocResourceFailedException("#There wasn't any network resource can be supported. #Protocol: " + innerPair.Key);
                //registers network protocol to remoting service.
                foreach (MessageIdentity identity in innerPair.Value)
                    protocolRegister.Register(identity, innerPair.Key, Level, networkUri, GlobalUniqueId);
            }
            Interlocked.Exchange(ref _protocolDic, protocolDic);
        }

        private void InitializeNetworkProtocolHandler(Dictionary<ProtocolTypes, IDictionary<MessageIdentity, IDictionary<ApplicationLevel, int>>> dic, KeyValuePair<ProtocolTypes, IList<MessageIdentity>> values)
        {
            IDictionary<MessageIdentity, IDictionary<ApplicationLevel, int>> firstLevel;
            if (!dic.TryGetValue(values.Key, out firstLevel)) dic.Add(values.Key, (firstLevel = new Dictionary<MessageIdentity, IDictionary<ApplicationLevel, int>>()));
            foreach (MessageIdentity identity in values.Value)
            {
                IDictionary<ApplicationLevel, int> secondLevel;
                if (!firstLevel.TryGetValue(identity, out secondLevel)) firstLevel.Add(identity, (secondLevel = new Dictionary<ApplicationLevel, int>()));
                if (secondLevel.ContainsKey(Level)) throw new DuplicatedApplicationException(string.Format("#Duplicated application attributes! #Protocol: {0}, #MessageIdentity: {1}, #Level: {2}.", values.Key, identity, Level));
                secondLevel.Add(Level, 0);
            }
        }

        /// <summary>
        ///    在指定的应用隧道上创建一个业务包裹
        /// </summary>
        public IBusinessPackage CreateBusinessPackage()
        {
            MessageTransaction<MetadataContainer> transaction = _agent.CreateTransaction();
            BusinessPackage package = new BusinessPackage((MetadataMessageTransaction) transaction);
            package.ProtocolType = ProtocolTypes.Metadata;
            package.State = BusinessPackageStates.ReceivedOutsideRequest;
            Console.WriteLine(package.Transaction.Identity);
            return package;
        }

        /// <summary>
        ///     建立与APP之间的内部通信隧道
        /// </summary>
        /// <exception cref="CannotConnectToTunnelException">无法建立正常的隧道连接</exception>
        private void InitializeTunnel()
        {
            PipeUri uri = new PipeUri(TunnelAddress);
            ITransportChannel channel = new PipeTransportChannel(uri);
            channel.Connect();
            if (!channel.IsConnected) throw new CannotConnectToTunnelException("#Couldn't connect to specified remote tunnel address: " + uri);
            _agent = new MetadataConnectionAgent(new MessageTransportChannel<MetadataContainer>((IRawTransportChannel)channel, _protocolStack), _transactionManager);
        }

        /// <summary>
        ///     开启一个动态对象
        /// </summary>
        public void Start()
        {
            WorkProcessingHandler(new LightSingleArgEventArgs<string>("Starting application: " + _entryInfo.EntryPoint));
            _application.Start();
            WorkProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("Application {0} has been started!", _entryInfo.EntryPoint)));
        }

        /// <summary>
        ///     停止执行
        /// </summary>
        public void Stop()
        {
            if (_domain == null) return;
            try
            {
                if (_application != null)
                {
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>("Stopping dynamic component: " + _entryInfo.EntryPoint));
                    _application.Stop();
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("Dynamic component {0} has been stopped.", _entryInfo.EntryPoint)));
                }
                _application = null;
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
            finally
            {
                try
                {
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>("UNLOADING appdomain: " + _domain.FriendlyName));
                    AppDomain.Unload(_domain);
                }
                catch (System.Exception ex) { if (!(ex is ThreadAbortException)) _tracing.Error(ex, null); }
            }
            _domain = null;
        }

        public void OnLoading()
        {
            _application.OnLoading();
        }

        /// <summary>
        ///    续租生命周期
        /// </summary>
        /// <param name="time">生命周期续租的时间</param>
        private void ReLease(TimeSpan time)
        {
            if (_application != null)
            {
                MarshalByRefObject marshalByRefObject = _application;
                ILease lease = (ILease)marshalByRefObject.InitializeLifetimeService();
                lease.Renew(time);
            }
        }

        /// <summary>
        ///    获取应用内部所有已经支持的网络通讯协议
        /// </summary>
        /// <returns>返回支持的网络通信协议列表</returns>
        public IDictionary<ProtocolTypes, IList<MessageIdentity>> AcquireSupportedProtocols()
        {
            return _application.AcquireSupportedProtocols();
        }

        /// <summary>
        ///    更新网络缓存信息
        /// </summary>
        /// <param name="level">应用等级</param>
        /// <param name="cache">远程目标终结点信息列表</param>
        /// <param name="protocol">通信协议</param>
        /// <param name="protocolTypes">协议类型</param>
        public void UpdateCache(Protocols protocol, ProtocolTypes protocolTypes, ApplicationLevel level, List<string> cache)
        {
            _application.UpdateCache(protocol, protocolTypes, level, cache);
        }

        /// <summary>
        ///    更新灰度升级策略的源代码
        /// </summary>
        /// <param name="code">灰度升级策略的源代码</param>
        public void UpdateGreyPolicy(string code)
        {
            _application.UpdateGreyPolicy(code);
        }

        /// <summary>
        ///    反向更新从CSN推送过来的KEY和VALUE配置信息
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">VALUE</param>
        public void UpdateConfiguration(string key, string value)
        {
            _application.UpdateConfiguration(key, value);
        }

        internal void HandleBusiness(Tuple<KAENetworkResource, ApplicationLevel> tag, object transaction, MessageIdentity reqMsgIdentity, object reqMsg)
        {
            //Targeted network protocol CANNOT be support.
            IDictionary<MessageIdentity, IDictionary<ApplicationLevel, int>> dic;
            if (!_protocolDic.TryGetValue(tag.Item1.Protocol, out dic))
            {
                _handleErrorSituation(tag.Item1.Protocol, transaction, KAEErrorCodes.NotSupportedNetworkType, "#We'd not supported current network type yet!");
                return;
            }
            //Targeted MessageIdentity CANNOT be support.
            IDictionary<ApplicationLevel, int> subDic;
            if (!dic.TryGetValue(reqMsgIdentity, out subDic))
            {
                _handleErrorSituation(tag.Item1.Protocol, transaction, KAEErrorCodes.NotSupportedMessageIdentity, "#We'd not supported current MessageIdentity yet!");
                return;
            }
            int tmpValue;
            //Targeted application's level CANNOT be support.
            if (!subDic.TryGetValue(tag.Item2, out tmpValue))
            {
                _handleErrorSituation(tag.Item1.Protocol, transaction, KAEErrorCodes.NotSupportedApplicationLevel, "#We'd not supported current application's level yet!");
                return;
            }
            Interlocked.Increment(ref _unRspCount);
            //acquires a business package for getting the return value from targeted application.
            BusinessPackage package = (BusinessPackage)CreateBusinessPackage();
            package.Transaction.Failed += delegate
            {
                Interlocked.Decrement(ref _unRspCount);
                _handleErrorSituation(ProtocolTypes.Metadata, transaction, KAEErrorCodes.TunnelCommunicationFailed, "#Occured failed while communicating with the targeted application.");
            };
            package.Transaction.Timeout += delegate
            {
                Interlocked.Decrement(ref _unRspCount);
                _handleErrorSituation(ProtocolTypes.Metadata, transaction, KAEErrorCodes.TunnelCommunicationTimeout, "#Occured timeout while communicating with the targeted application.");
            };
            package.Transaction.ResponseArrived += delegate(object o, LightSingleArgEventArgs<MetadataContainer> args)
            {
                Interlocked.Decrement(ref _unRspCount);
                package.State = BusinessPackageStates.ReceivedDeliveryResponse;
                //error situation.
                if (args.Target.GetAttributeByIdSafety<byte>(0x0A) != 0x00)
                    _handleErrorSituation(ProtocolTypes.Metadata, transaction, (KAEErrorCodes)args.Target.GetAttributeAsType<byte>(0x0A), args.Target.GetAttributeAsType<string>(0x0B));
                else _handleSucceedSituation(tag.Item1.Protocol, transaction, KAEErrorCodes.OK, args.Target);
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

        #endregion

        #region Events

        //domain exxception handled proc.
        protected void DomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WorkProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("Appdomain {0} occur unhandled error!", _domain.FriendlyName)));
            WorkProcessingHandler(new LightSingleArgEventArgs<string>("   ->STOPPING current ERROR appdomain: " + _domain.FriendlyName));
            Stop();
            ExitedHandler(null);
        }

        /// <summary>
        ///     工作状态输出事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<String>> WorkProcessing;
        protected void WorkProcessingHandler(LightSingleArgEventArgs<string> e)
        {
            EventHandler<LightSingleArgEventArgs<string>> handler = WorkProcessing;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     退出事件
        /// </summary>
        public event EventHandler Exited;
        protected void ExitedHandler(System.EventArgs e)
        {
            EventHandler handler = Exited;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}