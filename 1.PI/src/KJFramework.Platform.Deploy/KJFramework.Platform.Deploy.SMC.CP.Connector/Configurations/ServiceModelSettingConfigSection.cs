using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Configurations
{
    [CustomerSection("KJFramework")]
    public class SMCSettingConfigSection : CustomerSection<SMCSettingConfigSection>
    {
        /// <summary>
        ///   KJFramework.Platform.Deploy.SMC≈‰÷√œÓ
        /// </summary>
        [CustomerField("KJFramework.Platform.Deploy.SMC")]
        public SettingConfiguration Settings;
    }
}