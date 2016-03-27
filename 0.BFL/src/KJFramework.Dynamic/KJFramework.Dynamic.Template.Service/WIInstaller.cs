using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Xml;
using KJFramework.Tracing;

namespace KJFramework.Dynamic.Template.Service
{
    [RunInstaller(true)]
    public class WIInstaller : Installer
    {
        // Fields
        private ServiceProcessInstaller _pro;
        private ServiceInstaller _svc = new ServiceInstaller();

        // Methods
        public WIInstaller()
        {
            Configuration openExeConfiguration = ConfigurationManager.OpenExeConfiguration(typeof (WIInstaller).Assembly.Location);
            _svc.StartType = ServiceStartMode.Automatic;
            _svc.DisplayName = openExeConfiguration.AppSettings.Settings["ServiceName"].Value;
            _svc.ServiceName = openExeConfiguration.AppSettings.Settings["ServiceName"].Value;
            _svc.Description = openExeConfiguration.AppSettings.Settings["Description"].Value;
            Installers.Add(_svc);
            _pro = new ServiceProcessInstaller();
            _pro.Account = ServiceAccount.LocalSystem;
            Installers.Add(_pro);
        }

        public void init()
        {
            
        }
    }
}