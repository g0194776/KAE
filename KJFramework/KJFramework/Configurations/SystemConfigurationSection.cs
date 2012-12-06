using KJFramework.Attribute;
using KJFramework.Configurations.Items;

namespace KJFramework.Configurations
{
    [CustomerSection("Logger")]
    public class SystemConfigurationSection : CustomerSection<SystemConfigurationSection>
    {
        [CustomerField("ConfigurationSettings")]
        public ConfigurationSettingsItem ConfigurationSettingsItem;
    }
}
