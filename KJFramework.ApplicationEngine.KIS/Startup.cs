using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(KJFramework.ApplicationEngine.KIS.Startup))]

namespace KJFramework.ApplicationEngine.KIS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
