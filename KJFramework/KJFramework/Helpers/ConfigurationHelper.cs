using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace KJFramework.Helpers
{
    /// <summary>
    ///     �����������
    /// </summary>
    internal class ConfigurationHelper
    {
        /// <summary>
        ///     ��ȡӦ�ó��������EXE�����ļ�
        /// </summary>
        /// <returns>����������</returns>
        public static Configuration GetApplicationConfiguration()
        {
            //���WEB��Ŀ����ʹ��dllΪ���õ��ʱ��HttpContext.Current��Ϊnull, ��������˫�ر���
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