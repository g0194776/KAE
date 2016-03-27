using System.ServiceProcess;

namespace KJFramework.Dynamic.Template.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new TemplateService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
