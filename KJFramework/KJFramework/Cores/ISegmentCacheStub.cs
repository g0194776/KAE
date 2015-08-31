using KJFramework.Indexers;

namespace KJFramework.Cores
{
    /// <summary>
    ///     内存段缓存存根元接口，提供了相关的基本操作
    /// </summary>
    internal interface ISegmentCacheStub : ICacheStub<byte[]>
    {
        /// <summary>
        ///     获取一个值，该值表示了当前的缓存是否已经被使用
        /// </summary>
        bool IsUsed { get; }
        /// <summary>
        ///     获取缓存索引器
        /// </summary>
        ICacheIndexer Indexer { get; }
        /// <summary>
        ///     初始化
        /// </summary>
        void Initialize();
    }
}