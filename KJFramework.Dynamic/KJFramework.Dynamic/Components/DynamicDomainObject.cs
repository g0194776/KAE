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
    ///     ��̬����������ṩ����صĻ���������
    /// </summary>
    public class DynamicDomainObject : MarshalByRefObject, IDynamicDomainObject, IStatisticable<IStatistic>
    {
        #region Constructor

        /// <summary>
        ///     ��̬����������ṩ����صĻ���������
        /// </summary>
        /// <param name="info">�����������Ϣ</param>
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
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ����Ƿ�����������
        /// </summary>
        internal bool IsUpdating
        {
            get { return _isUpdating; }
            set { _isUpdating = value; }
        }

        /// <summary>
        ///     ��ȡ����ǰ�����
        /// </summary>
        internal DynamicDomainComponent OrgComponent
        {
            get { return _orgComponent; }
        }

        /// <summary>
        ///     ӵ�еĶ�̬������������
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
        ///     ��ȡ�ڲ���̬���������
        /// </summary>
        public IDynamicDomainComponent Component
        {
            get { return _component; }
        }

        /// <summary>
        ///     ��ȡӦ�ó������齨�����Ϣ
        /// </summary>
        public DomainComponentEntryInfo EntryInfo
        {
            get { return _entryInfo; }
        }

        /// <summary>
        ///     ��ȡ�������ϸ��Ϣ
        /// </summary>
        public PluginInfomation Infomation
        {
            get { return _infomation; }
        }

        /// <summary>
        ///     ��鵱ǰ����Ľ���״��
        /// </summary>
        /// <returns>���ؽ���״��</returns>
        public HealthStatus CheckHealth()
        {
            if (_component != null) return _component.CheckHealth();
            throw new System.Exception("#Current inner component object is *NULL*.");
        }

        /// <summary>
        ///     ��ȡ����ʱ��
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        /// <summary>
        ///     ��ȡ�������ϴθ���ʱ��
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return _lastUpdateTime; }
            set { _lastUpdateTime = value; }
        }

        /// <summary>
        ///     ��ȡ�ڲ�Ӧ�ó�����
        /// </summary>
        /// <returns>����Ӧ�ó�����</returns>
        public AppDomain GetDomain()
        {
            return _domain;
        }

        /// <summary>
        ///     ��ȡΨһ��ʾ
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     ��ʼִ��
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
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>("������̬�������������: " + _entryInfo.EntryPoint + ", Error trace : " + ex.Message));
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
        ///     ִֹͣ��
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
        ///     ���µ�ǰ��̬������
        /// </summary>
        /// <exception cref="DynamicDomainObjectUpdateFailedException">����ʧ��</exception>
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
        ///     ���������������������
        /// </summary>
        /// <param name="time">����ʱ��</param>
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
        ///     ��̬����������˳��¼�
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
        ///     ����״̬�ر��¼�
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
        ///     ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion
    }
}