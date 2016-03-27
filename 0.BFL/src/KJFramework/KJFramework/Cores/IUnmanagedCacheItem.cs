using System;

namespace KJFramework.Cores
{
    /// <summary>
    ///     非托管缓存项元接口，提供了相关的基本操作。
    /// </summary>
    public interface IUnmanagedCacheItem : ICacheItem<byte[]>
    {
        /// <summary>
        ///     获取内存句柄
        /// </summary>
        IntPtr Handle { get; }
        /// <summary>
        ///     获取当前的使用大小
        /// </summary>
        int UsageSize { get; }
        /// <summary>
        ///     获取当前的最大容量
        /// </summary>
        int MaxSize { get; }
        /// <summary>
        ///     释放当前所持有的非托管缓存
        /// </summary>
        void Free();
    }
}