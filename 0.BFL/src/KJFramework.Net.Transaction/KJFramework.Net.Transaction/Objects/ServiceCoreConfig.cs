namespace KJFramework.Net.Transaction.Objects
{
    /// <summary>
    ///     服务核心配置信息
    /// </summary>
    public class ServiceCoreConfig
    {
        /// <summary>
        ///     获取服务远程地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        ///     获取服务的负载区域开始位置
        /// </summary>
        public int BeginRange { get; set; }
        /// <summary>
        ///     获取服务的负载区域结束位置
        /// </summary>
        public int EndRange { get; set; }
    }
}