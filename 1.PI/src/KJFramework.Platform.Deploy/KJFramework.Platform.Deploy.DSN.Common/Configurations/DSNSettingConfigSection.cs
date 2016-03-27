using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Platform.Deploy.DSN.Common.Configurations
{
    [CustomerSection("KJFramework")]
    public class DSNSettingConfigSection : CustomerSection<DSNSettingConfigSection>
    {
        /// <summary>
        ///   KJFramework.Platform.Deploy.DSC≈‰÷√œÓ
        /// </summary>
        [CustomerField("KJFramework.Platform.Deploy.DSN")]
        public SettingConfiguration Settings;
    }
}