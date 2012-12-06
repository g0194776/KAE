using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Net.Channels.Configurations
{
    /// <summary>
    ///     通信信道模型相关配置节
    /// </summary>
    [CustomerSection("KJFramework")]
    public class ChannelModelSettingConfigSection : CustomerSection<ChannelModelSettingConfigSection>
    {
        /// <summary>
        ///   KJFramework.Net.Channels配置项
        /// </summary>
        [CustomerField("KJFramework.Net.Channels")]
        public SettingConfiguration Settings;
    }
}