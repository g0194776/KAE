using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.ApplicationEngine.Configurations
{
    [CustomerSection("DeployCenter")]
    public class DeployConfigSection : CustomerSection<DeployConfigSection>
    {
        /// <summary>
        ///   Addresses≈‰÷√œÓ
        /// </summary>
        [CustomerField("Addresses")]
        public DeploySettingConfiguration Settings;
    }
}