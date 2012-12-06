using System;
using KJFramework.Attribute;

namespace KJFramework.Configurations.Items
{
    /// <summary>
    ///     配置相关设置项
    /// </summary>
    public class ConfigurationSettingsItem
    {
        /// <summary>
        ///     日志记录位置
        /// </summary>
        [CustomerField("LogPath")]
        public String LogPath;
    }
}
