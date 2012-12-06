using System;
using KJFramework.Configurations;
using KJFramework.Logger.LogObject;
namespace KJFramework.Logger
{
    /// <summary>
    ///     KJFramework内部提供的全局文本日志记录器
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
        ///     全局的日志记录器
        /// </summary>
        public static ITextLogger<ITextLog> Logger;
        /// <summary>
        ///     日志记录签署人名称
        /// </summary>
        public static String Name = "KJFramework Developer";
    }
}