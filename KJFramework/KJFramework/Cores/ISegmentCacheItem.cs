namespace KJFramework.Cores
{
    /// <summary>
    ///     内存段缓存项元接口，提供了相关的基本操作
    /// </summary>
    public interface ISegmentCacheItem : ICacheItem<byte[]>
    {
        /// <summary>
        ///     获取一个值，该值表示了当前的缓存是否已经被使用
        /// </summary>
        bool IsUsed { get; }
        /// <summary>
        ///     获取当前的使用大小
        /// </summary>
        int UsageSize { get; }
        /// <summary>
        ///     初始化
        /// </summary>
        void Initialize();
        /// <summary>
        ///     设置缓存内容
        /// </summary>
        /// <param name="obj">缓存对象</param>
        /// <param name="usedSize">使用大小</param>
        void SetValue(byte[] obj, int usedSize);
    }
}