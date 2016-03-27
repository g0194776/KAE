namespace KJFramework.Net.Processor
{
    /// <summary>
    ///     处理器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IProcessor : IMetadata<int>
    {
        /// <summary>
        ///     获取或设置可用状态
        /// </summary>
        bool IsEnable { get; set;}
        /// <summary>
        ///     获取或设置处理器ID（编号）
        ///         * 此编号可以与KEY相同。
        /// </summary>
        int ID { get; set;}
    }
}
