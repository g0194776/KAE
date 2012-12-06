using System;
using KJFramework.Configurations;
using KJFramework.Logger.LogObject;
namespace KJFramework.Logger
{
    /// <summary>
    ///     KJFramework�ڲ��ṩ��ȫ���ı���־��¼��
    /// </summary>
    public static class Logs
    {
        static Logs()
        {
            String path = SystemConfigurationSection.Current.ConfigurationSettingsItem ==
                          null
                              ? Environment.CurrentDirectory
                              : (SystemConfigurationSection.Current.
                                     ConfigurationSettingsItem.LogPath + "Component_" +
                                 AppDomain.CurrentDomain.FriendlyName.Replace(":", "_") + "\\");
            Logger = new BasicTextLogger<ITextLog>(path);
        }

        /// <summary>
        ///     ȫ�ֵ���־��¼��
        /// </summary>
        public static ITextLogger<ITextLog> Logger;
        /// <summary>
        ///     ��־��¼ǩ��������
        /// </summary>
        public static String Name = "KJFramework Developer";
    }
}