using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Platform.Deploy.DSC.CP.Client.Configurations
{
    [CustomerSection("KJFramework")]
    public class ClientSettingConfigSection : CustomerSection<ClientSettingConfigSection>
    {
        /// <summary>
        ///   KJFramework.Platform.Deploy.DSC≈‰÷√œÓ
        /// </summary>
        [CustomerField("KJFramework.Platform.Deploy.DSC.Client")]
        public ClientSettingConfiguration Settings;
    }
}