using KJFramework.Objects;

namespace KJFramework.Containers
{
    /// <summary>
    ///     连续内存段缓存容器元接口，提供了相关的基本操作
    /// </summary>
    public interface IMemoryChunkCacheContainer
    {
        /// <summary>
        ///     获取内存片段大小
        /// </summary>
        int SegmentSize { get; }
        /// <summary>
        ///     获取连续占用的内存段大小
        /// </summary>
        int MemoryChunkSize { get; }
        /// <summary>
        ///     租借一个内存片段
        /// </summary>
        /// <returns>如果返回null, 则证明已经没有剩余的内存片段可以被租借</returns>
        IMemorySegment Rent();
        /// <summary>
        ///     归还一个内存片段
        /// </summary>
        /// <param name="segment">内存片段</param>
        void Giveback(IMemorySegment segment);
    }
}