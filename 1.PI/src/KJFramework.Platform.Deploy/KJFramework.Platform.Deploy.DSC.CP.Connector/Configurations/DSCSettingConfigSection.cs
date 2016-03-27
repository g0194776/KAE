using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Configurations
{
    [CustomerSection("KJFramework")]
    public class DSCSettingConfigSection : CustomerSection<DSCSettingConfigSection>
    {
        /// <summary>
        ///   KJFramework.Platform.Deploy.DSC≈‰÷√œÓ
        /// </summary>
        [CustomerField("KJFramework.Platform.Deploy.DSC")]
        public SettingConfiguration Settings;
    }
}