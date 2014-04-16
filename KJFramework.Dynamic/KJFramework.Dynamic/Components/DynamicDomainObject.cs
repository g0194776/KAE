using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Exceptions;
using KJFramework.Dynamic.Statistics;
using KJFramework.Dynamic.Structs;
using KJFramework.EventArgs;
using KJFramework.Statistics;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     动态程序域对象，提供了相关的基本操作。
    /// </summary>
    public class DynamicDomainObject : MarshalByRefObject, IDynamicDomainObject, IStatisticable<IStatistic>
    {
        #region Constructor

        /// <summary>
        ///     动态程序域对象，提供了相关的基本操作。
        /// </summary>
        /// <param name="info">程序域组件信息</param>
        public DynamicDomainObject(DomainComponentEntryInfo info)
        {
            _entryInfo = info;
            _id = Guid.NewGuid();
            _createTime = DateTime.Now;
            DynamicDomainObjectStatistic statistic = new DynamicDomainObjectStatistic();
            statistic.Initialize(this);
            _statistics.Add(StatisticTypes.Other, statistic);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("#NEW COMPONENT...");
            builder.AppendLine("   PATH: " + _entryInfo.FolderPath);
            builder.AppendLine("   FILE: " + _entryInfo.FilePath);
            builder.AppendLine("   ENTRYPOINT: " + _entryInfo.EntryPoint);
            WorkProcessingHandler(new LightSingleArgEventArgs<string>(builder.ToString()));
        }

        #endregion

        #region Members

        private readonly Guid _id;
        private readonly DateTime _createTime;
        protected bool _isUpdating;
        protected AppDomain _domain;
        protected DomainComponentEntryInfo _entryInfo;
        protected IDynamicDomainComponent _component;
        protected DateTime _lastUpdateTime;
        protected DynamicDomainComponent _orgComponent;
        protected PluginInfomation _infomation;
        protected Dictionary<StatisticTypes, IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DynamicDomainObject));

        /// <summary>
        ///     获取或设置一个值，该值标示了当前组件是否正在升级中
        /// </summary>
        internal bool IsUpdating
        {
            get { return _isUpdating; }
            set { _isUpdating = value; }
        }

        /// <summary>
        ///     获取升级前的组件
        /// </summary>
        internal DynamicDomainComponent OrgComponent
        {
            get { return _orgComponent; }
        }

        /// <summary>
        ///     拥有的动态程序域服务对象
        /// </summary>
        internal DynamicDomainService OwnService { get; set; }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (_statistics != null)
            {
                foreach (IStatistic statistic in _statistics.Values)
                {
                    statistic.Close();
                }
                _statistics.Clear();
                _statistics = null;
            }
        }

        #endregion

        #region Implementation of IDynamicDomainObject

        /// <summary>
        ///     获取内部动态程序域组件
        /// </summary>
        public IDynamicDomainComponent Component
        {
            get { return _component; }
        }

        /// <summary>
        ///     获取应用程序域组建入口信息
        /// </summary>
        public DomainComponentEntryInfo EntryInfo
        {
            get { return _entryInfo; }
        }

        /// <summary>
        ///     获取插件的详细信息
        /// </summary>
        public PluginInfomation Infomation
        {
            get { return _infomation; }
        }

        /// <summary>
        ///     检查当前组件的健康状况
        /// </summary>
        /// <returns>返回健康状况</returns>
        public HealthStatus CheckHealth()
        {
            if (_component != null) return _component.CheckHealth();
            throw new System.Exception("#Current inner component object is *NULL*.");
        }

        /// <summary>
        ///     获取创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        /// <summary>
        ///     获取或设置上次更新时间
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return _lastUpdateTime; }
            set { _lastUpdateTime = value; }
        }

        /// <summary>
        ///     获取内部应用程序域
        /// </summary>
        /// <returns>返回应用程序域</returns>
        public AppDomain GetDomain()
        {
            return _domain;
        }

        /// <summary>
        ///     获取唯一标示
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     开始执行
        /// </summary>
        public virtual void Start()
        {
            if (_domain == null)
            {
                try
                {
                    AppDomainSetup setup = new AppDomainSetup();
                    setup.ShadowCopyFiles = "true";
                    setup.CachePath = "C:\\AssemblyCached";
                    setup.ConfigurationFile = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config";
                    setup.ApplicationBase = _entryInfo.FolderPath;
                    setup.ApplicationName = _entryInfo.EntryPoint;
                    setup.ShadowCopyDirectories = _entryInfo.FolderPath;
                    setup.PrivateBinPath = AppDomain.CurrentDomain.BaseDirectory;
                    String componentName = _entryInfo.EntryPoint.Substring(_entryInfo.EntryPoint.LastIndexOf('.') + 1);

#if !MONO
                    _domain = AppDomain.CreateDomain("{APPDOMAIN:" + componentName + "}", null, setup);
#else
                    _domain = AppDomain.CreateDomain("{APPDOMAIN:" + componentName + "}");
#endif

                    WorkProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("Create domain {0} succeed.", _domain.FriendlyName)));
                    _domain.UnhandledException += DomainUnhandledException;
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>("Creating object handle......"));
                    ObjectHandle cls = _domain.CreateInstanceFrom(_entryInfo.FilePath, _entryInfo.EntryPoint);
                    if (cls != null)
                    {
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>("Unwrapping inner core component......"));
                        IDynamicDomainComponent component = (IDynamicDomainComponent)cls.Unwrap();
                        component.OwnService = OwnService;
                        _component = component;
                        _infomation = _component.PluginInfo;
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>("Trying to renew component life......"));
                        ReLease(new TimeSpan(365, 0, 0, 0));
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>("Calling OnLoading method......"));
                        _component.OnLoading();
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>("Opening dynamic component: " + _entryInfo.EntryPoint));
                        _component.Start();
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("Component {0} has been started!", _entryInfo.EntryPoint)));
                    }
                }
                catch (System.Exception ex)
                {
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>("开启动态程序域组件错误: " + _entryInfo.EntryPoint + ", Error trace : " + ex.Message));
                    _tracing.Error(ex, null);
                    throw;
                }
                return;
            }
            try { _component.Start(); }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
        }

        /// <summary>
        ///     停止执行
        /// </summary>
        public void Stop()
        {
            if (_domain == null) return;
            try
            {
                if (_component != null)
                {
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>("Stopping dynamic component: " + _entryInfo.EntryPoint));
                    _component.Stop();
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("Dynamic component {0} has been stopped.", _entryInfo.EntryPoint)));
                }
                _component = null;
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

        /// <summary>
        ///     更新当前动态程序域
        /// </summary>
        /// <exception cref="DynamicDomainObjectUpdateFailedException">更新失败</exception>
        public void Update()
        {
            try
            {
                _isUpdating = true;
                WorkProcessingHandler(new LightSingleArgEventArgs<string>("*BEGIN* updating dynamic component: " + _entryInfo.EntryPoint));
                WorkProcessingHandler(new LightSingleArgEventArgs<string>("   ->STOPPING current working component: " + _entryInfo.EntryPoint));
                Stop();
                WorkProcessingHandler(new LightSingleArgEventArgs<string>("   ->REOPENING current component: " + _entryInfo.EntryPoint));
                Start();
                WorkProcessingHandler(new LightSingleArgEventArgs<string>("*END* updating dynamic component: " + _entryInfo.EntryPoint));
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
            finally
            {
                _isUpdating = false;
                _lastUpdateTime = DateTime.Now;
            }
        }

        /// <summary>
        ///     重新续订组件的生命周期
        /// </summary>
        /// <param name="time">过期时间</param>
        public void ReLease(TimeSpan time)
        {
            if (_component != null)
            {
                MarshalByRefObject marshalByRefObject = (MarshalByRefObject) _component;
                ILease lease = (ILease) marshalByRefObject.InitializeLifetimeService();
                lease.Renew(time);
            }
        }

        /// <summary>
        ///     动态程序域对象退出事件
        /// </summary>
        public event EventHandler Exited;
        protected void ExitedHandler(System.EventArgs e)
        {
            EventHandler handler = Exited;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Events

        //domain exxception handled proc.
        protected void DomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WorkProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("Appdomain {0} occur unhandled error!",_domain.FriendlyName)));
            WorkProcessingHandler(new LightSingleArgEventArgs<string>("   ->STOPPING current ERROR appdomain: " + _domain.FriendlyName));
            Stop();   
            ExitedHandler(null);
        }

        /// <summary>
        ///     工作状态回报事件
        /// </summary>
        internal event EventHandler<LightSingleArgEventArgs<String>> WorkProcessing;
        protected void WorkProcessingHandler(LightSingleArgEventArgs<string> e)
        {
            EventHandler<LightSingleArgEventArgs<string>> handler = WorkProcessing;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        /// <summary>
        ///     获取或设置统计器
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion
    }
}