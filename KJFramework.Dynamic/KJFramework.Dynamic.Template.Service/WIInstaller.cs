using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Xml;

namespace KJFramework.Dynamic.Template.Service
{
    [RunInstaller(true)]
    public class WIInstaller : Installer
    {
        // Fields
        private string _description;
        private ServiceProcessInstaller _pro;
        private string _serviceName;
        private ServiceInstaller _svc = new ServiceInstaller();

        // Methods
        public WIInstaller()
        {
            _svc.StartType = ServiceStartMode.Automatic;
            InitConfig();
            _svc.ServiceName = _serviceName;
            _svc.DisplayName = _svc.ServiceName;
            _svc.Description = _description;
            Installers.Add(_svc);
            _pro = new ServiceProcessInstaller();
            _pro.Account = ServiceAccount.LocalSystem;
            Installers.Add(_pro);
        }

        private void InitConfig()
        {
            string filename = typeof(WIInstaller).Assembly.Location + ".config";
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlElement element = document.SelectSingleNode("/configuration/CustomerConfig/Service/Infomation") as XmlElement;
            if (element == null)
            {
                throw new System.Exception("/configuration/CustomerConfig/Service/Infomation not found in " + filename);
            }
            this._serviceName = element.GetAttribute("ServiceName");
            if (string.IsNullOrEmpty(this._serviceName))
            {
                throw new System.Exception("/configuration/CustomerConfig/Service/Infomation/@ServiceName not found in " + filename);
            }
            this._description = element.GetAttribute("Description");
        }
    }
}