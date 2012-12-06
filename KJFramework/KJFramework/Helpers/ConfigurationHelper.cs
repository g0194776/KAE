using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace KJFramework.Helpers
{
    /// <summary>
    ///     配置项帮助器
    /// </summary>
    internal class ConfigurationHelper
    {
        /// <summary>
        ///     获取应用程序的配套EXE配置文件
        /// </summary>
        /// <returns>返回配置项</returns>
        public static Configuration GetApplicationConfiguration()
        {
            //如果WEB项目，在使用dll为调用点的时候，HttpContext.Current会为null, 所以启用双重保险
            //return HttpContext.Current != null ? WebConfigurationManager.OpenWebConfiguration("~") : ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            try
            {
                if (HttpContext.Current != null) return WebConfigurationManager.OpenWebConfiguration("~");
                return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            catch (System.ArgumentException e) { return WebConfigurationManager.OpenWebConfiguration("~"); }
        }
    }
}