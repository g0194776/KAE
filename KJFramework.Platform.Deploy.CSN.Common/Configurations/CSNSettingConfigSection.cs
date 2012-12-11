using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Platform.Deploy.CSN.Common.Configurations
{
    [CustomerSection("KJFramework")]
    public class CSNSettingConfigSection : CustomerSection<CSNSettingConfigSection>
    {
        /// <summary>
        ///   KJFramework.Platform.Deploy.CSN≈‰÷√œÓ
        /// </summary>
        [CustomerField("KJFramework.Platform.Deploy.CSN")]
        public SettingConfiguration Settings;
    }
}