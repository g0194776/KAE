namespace KJFramework.Indexers
{
    /// <summary>
    ///     缓存索引器元接口，提供了相关的基本操作。
    /// </summary>
    internal interface ICacheIndexer
    {
        /// <summary>
        ///     获取索引器开始的偏移量
        /// </summary>
        int BeginOffset { get; }
        /// <summary>
        ///     获取当前缓存数据段大小
        /// </summary>
        int SegmentSize { get; }
        /// <summary>
        ///     获取缓存缓冲区
        /// </summary>
        byte[] CacheBuffer { get; }
    }
}