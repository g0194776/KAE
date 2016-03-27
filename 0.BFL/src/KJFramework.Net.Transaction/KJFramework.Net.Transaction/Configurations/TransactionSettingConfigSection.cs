using KJFramework.Attribute;
using KJFramework.Configurations;

namespace KJFramework.Net.Transaction.Configurations
{
    /// <summary>
    ///     事务模型相关配置节
    /// </summary>
    [CustomerSection("KJFramework")]
    public class TransactionSettingConfigSection : CustomerSection<TransactionSettingConfigSection>
    {
        /// <summary>
        ///   KJFramework.Net.Transaction配置项
        /// </summary>
        [CustomerField("KJFramework.Net.Transaction")]
        public SettingConfiguration Settings;
    }
}