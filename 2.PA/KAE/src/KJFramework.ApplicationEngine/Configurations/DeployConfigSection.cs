using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.ApplicationEngine.Configurations
{
    [CustomerSection("DeployCenter")]
    public class DeployConfigSection : CustomerSection<DeployConfigSection>
    {
        /// <summary>
        ///   Addresses������
        /// </summary>
        [CustomerField("Addresses")]
        public DeploySettingConfiguration Settings;
    }
}