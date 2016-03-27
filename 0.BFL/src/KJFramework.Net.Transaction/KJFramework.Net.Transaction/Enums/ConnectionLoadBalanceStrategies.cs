namespace KJFramework.Net.Transaction.Enums
{
    /// <summary>
    ///    并行网络连接的分发策略
    /// </summary>
    public enum ConnectionLoadBalanceStrategies
    {
        /// <summary>
        ///    支持顺序算法的连接容器
        /// </summary>
        Sequential,
        /// <summary>
        ///    支持随机算法的连接容器
        /// </summary>
        Random
    }
}