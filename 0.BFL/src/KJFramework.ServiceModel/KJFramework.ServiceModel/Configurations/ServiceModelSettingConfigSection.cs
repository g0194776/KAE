using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.ServiceModel.Configurations
{
    [CustomerSection("KJFramework")]
    public class ServiceModelSettingConfigSection : CustomerSection<ServiceModelSettingConfigSection>
    {
        /// <summary>
        ///   KJFramework.ServiceModel≈‰÷√œÓ
        /// </summary>
        [CustomerField("KJFramework.ServiceModel")]
        public SettingConfiguration NetworkLayer;
    }
}