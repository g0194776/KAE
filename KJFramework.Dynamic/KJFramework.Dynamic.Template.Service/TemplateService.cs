using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using KJFramework.Dynamic.Components;

namespace KJFramework.Dynamic.Template.Service
{
    /// <summary>
    ///     模板服务，用于创建一个被部署的Dynamic Domain Service
    /// </summary>
    partial class TemplateService : ServiceBase
    {
        #region Constructor

        /// <summary>
        ///     模板服务，用于创建一个被部署的Dynamic Domain Service
        /// </summary>
        public TemplateService()
        {
            if (!EventLog.SourceExists(".NET Runtime")) EventLog.CreateEventSource(".NET Runtime", "Application");
            InitializeComponent();
        }

        #endregion

        #region Members

        private String _name;
        private IDynamicDomainService _service;

        #endregion

        #region Methods

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command. </param>
        protected override void OnStart(string[] args)
        {
            try
            {
                //Thread.Sleep(20000);
                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                string path = exePath.Substring(0, exePath.LastIndexOf('\\') + 1);
                DynamicDomainService service = new DynamicDomainService(path, null);
                try { service.Start(); }
                catch (System.Exception ex)
                {
                    // 写事件日志
                    LogEvent(service.Infomation.ServiceName, ex);
                    try { service.Stop(); }
                    catch (System.Exception e) { LogEvent(service.Infomation.ServiceName, e); }
                    throw;
                }
            }
            catch (System.Exception ex)
            {
                // 写事件日志
                LogEvent(Process.GetCurrentProcess().ProcessName, ex);
                throw;
            }
        }

        private void LogEvent(string name, System.Exception ex)
        {
            EventLog logEntry;
            logEntry = new EventLog();
            logEntry.Source = name ?? ".NET Runtime";
            logEntry.WriteEntry(ex.Message + "\r\n" + ex.StackTrace, EventLogEntryType.Error);
            logEntry.Close();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            if (!String.IsNullOrEmpty(_name))
            {
                // 写事件日志
                EventLog logEntry = new EventLog();
                logEntry.Source = _name;
                logEntry.WriteEntry(_name + "服务已经被关闭。", EventLogEntryType.Warning);
                logEntry.Close();
            }
            if (_service == null) return;
            try { _service.Stop(); }
            catch (System.Exception ex) { LogEvent(_name, ex); }
            _service = null;
        }

        #endregion
    }
}
