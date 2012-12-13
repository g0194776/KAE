using KJFramework.Attribute;

namespace KJFramework.Net.Transaction.Configurations
{
    /// <summary>
    ///     相关配置项 
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///     事务超时时间
        /// </summary>
        [CustomerField("TransactionTimeout")]
        public string TransactionTimeout;
        /// <summary>
        ///    事务超时检查时间间隔
        /// </summary>
        [CustomerField("TransactionCheckInterval")]
        public int TransactionCheckInterval;
    }
}