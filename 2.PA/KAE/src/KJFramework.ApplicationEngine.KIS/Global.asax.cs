using System;
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
            SystemWorker.Initialize("KIS", RemoteConfigurationSetting.Default, new EtcdRemoteConfigurationProxy(new Uri("")));
            Global.Database = new Database(SystemWorker.ConfigurationProxy.GetField("KIS", "DatabaseConnection", false), 120);
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
