using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Exceptions;
using KJFramework.ApplicationEngine.Packages;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Dynamic;
using KJFramework.Dynamic.Components;
using KJFramework.Enums;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Configurations;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Channels.Uri;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Tracing;
using ILease = System.Runtime.Remoting.Lifetime.ILease;

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
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="CannotConnectToTunnelException">无法建立正常的隧道连接</exception>
        public ApplicationDynamicObject(ApplicationEntryInfo info, KPPDataStructure structure, ChannelInternalConfigSettings settings, IKAEHostProxy proxy)
        {
            if (info == null) throw new ArgumentNullException("info");
            if (structure == null) throw new ArgumentNullException("structure");
            if (settings == null) throw new ArgumentNullException("settings");
            _entryInfo = info;
            _settings = settings;
            _proxy = proxy;
            _structure = structure;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("#NEW COMPONENT...");
            builder.AppendLine("   PATH: " + info.FolderPath);
            builder.AppendLine("   FILE: " + info.FilePath);
            builder.AppendLine("   ENTRYPOINT: " + info.EntryPoint);
            builder.AppendLine("   CRC: " + info.FileCRC);
            WorkProcessingHandler(new LightSingleArgEventArgs<string>(builder.ToString()));
            PreInitialize();
            InitializeTunnel();
        }

        #endregion

        #region Members.

        private Application _application;
        protected AppDomain _domain;
        private readonly IKAEHostProxy _proxy;
        private readonly KPPDataStructure _structure;
        private readonly ApplicationEntryInfo _entryInfo;
        private readonly ChannelInternalConfigSettings _settings;
        private IServerConnectionAgent<MetadataContainer> _agent;
        private static readonly MetadataProtocolStack _protocolStack = new MetadataProtocolStack();
        private static readonly MetadataTransactionManager _transactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ApplicationDynamicObject));

        public string Version
        {
            get { return _application.Version; }
        }
        public string Description
        {
            get { return _application.Description; }
        }
        public string PackageName
        {
            get { return _application.PackageName; }
        }
        public Guid GlobalUniqueId
        {
            get { return _application.GlobalUniqueId; }
        }
        public ApplicationStatus Status
        {
            get { return _application.Status; }
        }
        public string Name
        {
            get { return _application.Name; }
        }
        public ApplicationLevel Level
        {
            get { return _application.Level; }
        }
        public long CRC
        {
            get { return _application.CRC; }
        }
        public HealthStatus CheckHealth()
        {
            return _application.CheckHealth();
        }
        public Guid Id
        {
            get { return _application.Id; }
        }
        public bool Enable
        {
            get { return _application.Enable; }
            set { _application.Enable = value; }
        }
        public PluginInfomation PluginInfo
        {
            get { return _application.PluginInfo; }
        }
        public bool IsCompletedEnvironment
        {
            get { return _application.IsCompletedEnvironment; }
        }

        /// <summary>
        ///    获取内部所使用的隧道连接地址
        /// </summary>
        public string TunnelAddress {
            get
            {
                return _application.TunnelAddress;
            }
        }

        [Obsolete("#Sadly, We had not supported this property.", true)]
        public IDynamicDomainService OwnService { get; set; }

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
        /// <param name="cache">网络信息</param>
        public void UpdateNetworkCache(Dictionary<string, List<string>> cache)
        {
            _application.UpdateNetworkCache(cache);
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