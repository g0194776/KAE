using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Dynamic.Template.Service.Configurations
{
    [CustomerSection("Setting")]
    internal class SettingConfigSection : CustomerSection<SettingConfigSection>
    {
        [CustomerField("Service")]
        public ServiceItem ServiceItem;
    }
}