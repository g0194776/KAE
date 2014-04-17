using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.EventArgs;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.Objects
{
    /// <summary>
    ///     KAE应用动态对象
    /// </summary>
    public class ApplicationDynamicObject : MarshalByRefObject
    {
        #region Constructor.

        /// <summary>
        ///     KAE应用动态对象
        /// </summary>
        /// <param name="info">应用入口结构信息</param>
        /// <param name="structure">KPP应用资源包的数据结构</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public ApplicationDynamicObject(ApplicationEntryInfo info, KPPDataStructure structure)
        {
            if (info == null) throw new ArgumentNullException("info");
            if (structure == null) throw new ArgumentNullException("structure");
            _entryInfo = info;
            _structure = structure;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("#NEW COMPONENT...");
            builder.AppendLine("   PATH: " + info.FolderPath);
            builder.AppendLine("   FILE: " + info.FilePath);
            builder.AppendLine("   ENTRYPOINT: " + info.EntryPoint);
            builder.AppendLine("   CRC: " + info.FileCRC);
            WorkProcessingHandler(new LightSingleArgEventArgs<string>(builder.ToString()));
        }

        #endregion

        #region Members.

        private Application _application;
        protected AppDomain _domain;
        private readonly KPPDataStructure _structure;
        private readonly ApplicationEntryInfo _entryInfo;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ApplicationDynamicObject));

        #endregion

        #region Methods.

        /// <summary>
        ///     开启一个动态对象
        /// </summary>
        public void Start()
        {
            if (_domain == null)
            {
                try
                {
                    AppDomainSetup setup = new AppDomainSetup();
                    setup.ShadowCopyFiles = "true";
                    setup.CachePath = "C:\\AssemblyCached";
                    setup.ConfigurationFile = _entryInfo.FilePath + ".config";
                    setup.ApplicationBase = _entryInfo.FolderPath;
                    setup.ApplicationName = _entryInfo.EntryPoint;
                    setup.ShadowCopyDirectories = _entryInfo.FolderPath;
                    setup.PrivateBinPath = AppDomain.CurrentDomain.BaseDirectory;
                    String componentName = _entryInfo.EntryPoint.Substring(_entryInfo.EntryPoint.LastIndexOf('.') + 1);
                    _domain = AppDomain.CreateDomain("{APPDOMAIN:" + componentName + "}", null, setup);
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("Create domain {0} succeed.", _domain.FriendlyName)));
                    _domain.UnhandledException += DomainUnhandledException;
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>("Creating object handle......"));
                    ObjectHandle cls = _domain.CreateInstanceFrom(_entryInfo.FilePath, _entryInfo.EntryPoint);
                    if (cls != null)
                    {
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>("Unwrapping......"));
                        Application app = (Application)cls.Unwrap();
                        _application = app;
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>("Trying to renew application life......"));
                        ReLease(new TimeSpan(365, 0, 0, 0));
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>("Calling OnLoading method......"));
                        _application.Initialize(_structure);
                        _application.OnLoading();
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>("Starting application: " + _entryInfo.EntryPoint));
                        _application.Start();
                        WorkProcessingHandler(new LightSingleArgEventArgs<string>(string.Format("Application {0} has been started!", _entryInfo.EntryPoint)));
                    }
                }
                catch (System.Exception ex)
                {
                    WorkProcessingHandler(new LightSingleArgEventArgs<string>("#Aplication " + _entryInfo.EntryPoint + ", Error trace : " + ex.Message));
                    _tracing.Error(ex, null);
                    throw;
                }
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

        private void ReLease(TimeSpan time)
        {
            if (_application != null)
            {
                MarshalByRefObject marshalByRefObject = (MarshalByRefObject)_application;
                ILease lease = (ILease)marshalByRefObject.InitializeLifetimeService();
                lease.Renew(time);
            }
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