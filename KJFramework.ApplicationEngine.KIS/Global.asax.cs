using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.KIS.Formatters;
using KJFramework.ApplicationEngine.KIS.Handlers;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.Datas;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KJFramework.ApplicationEngine.KIS
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            SystemWorker.Instance.Initialize("KIS", RemoteConfigurationSetting.Default, new SolitaryRemoteConfigurationProxy());
            Global.Database = new Database(SystemWorker.Instance.ConfigurationProxy.GetField("KIS", "DatabaseConnection"), 120);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.Formatters.Insert(0, new JsonNetFormatter());
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CustomerMessageHandler());
        }
    }
}
