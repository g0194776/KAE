using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Platform.Deploy.Maintenance.Configurations
{
    [CustomerSection("KJFramework")]
    public class MDDSSettingConfigSection : CustomerSection<MDDSSettingConfigSection>
    {
        /// <summary>
        ///   KJFramework.Platform.Deploy.SMC≈‰÷√œÓ
        /// </summary>
        [CustomerField("KJFramework.Platform.Deploy.MDDS")]
        public SettingConfiguration Settings;
    }
}