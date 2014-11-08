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
        /// <summary>
        ///    最小连接数(在相同的远程终结点地址情况下)
        /// </summary>
        [CustomerField("MinimumConnectionCount")]
        public int MinimumConnectionCount;
        /// <summary>
        ///    最大连接数(在相同的远程终结点地址情况下)
        /// </summary>
        [CustomerField("MaximumConnectionCount")]
        public int MaximumConnectionCount;
        /// <summary>
        ///    并行网络连接的分发策略
        /// </summary>
        [CustomerField("ConnectionLoadBalanceStrategy")]
        public string ConnectionLoadBalanceStrategy;
    }
}