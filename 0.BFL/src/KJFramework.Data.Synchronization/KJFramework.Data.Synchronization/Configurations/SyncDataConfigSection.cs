using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Data.Synchronization.Configurations
{
    [CustomerSection("KJFramework")]
    public class SyncDataConfigSection : CustomerSection<SyncDataConfigSection>
    {
        /// <summary>
        ///   Addresses≈‰÷√œÓ
        /// </summary>
        [CustomerField("KJFramework.Data.Synchronization")]
        public SyncDataSettingConfiguration Settings;
    }
}