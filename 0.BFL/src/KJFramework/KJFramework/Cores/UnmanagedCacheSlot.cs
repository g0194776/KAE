using System;

namespace KJFramework.Cores
{
    /// <summary>
    ///     非托管缓存槽静态类，提供了相关的基本操作。
    /// </summary>
    public static class UnmanagedCacheSlot
    {
        #region Methods

        /// <summary>
        ///     创建一个新的非托管缓存槽
        /// </summary>
        /// <param name="size">缓存最大容量</param>
        /// <returns>返回非托管内存槽</returns>
        /// <exception cref="System.OutOfMemoryException">内存溢出</exception>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        public static IUnmanagedCacheSlot New(int size)
        {
            return New(size, DateTime.MaxValue);
        }

        /// <summary>
        ///     创建一个新的非托管缓存槽
        /// </summary>
        /// <param name="size">缓存最大容量</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>返回非托管内存槽</returns>
        /// <exception cref="System.OutOfMemoryException">内存溢出</exception>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        public static IUnmanagedCacheSlot New(int size, DateTime expireTime)
        {
            return new UnmanagedCacheStub(size, expireTime);
        }

        /// <summary>
        ///     创建一个新的非托管缓存槽
        /// </summary>
        /// <param name="ptr">内存句柄</param>
        /// <param name="size">缓存最大容量</param>
        /// <param name="useageSize">已使用大小</param>
        /// <returns>返回非托管内存槽</returns>
        /// <exception cref="System.OutOfMemoryException">内存溢出</exception>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        public static IUnmanagedCacheSlot New(IntPtr ptr, int size, int useageSize)
        {
            return new UnmanagedCacheStub(ptr, size, useageSize, DateTime.MaxValue);
        }

        /// <summary>
        ///     创建一个新的非托管缓存槽
        /// </summary>
        /// <param name="ptr">内存句柄</param>
        /// <param name="size">缓存最大容量</param>
        /// <param name="useageSize">已使用大小</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>返回非托管内存槽</returns>
        /// <exception cref="System.OutOfMemoryException">内存溢出</exception>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        public static IUnmanagedCacheSlot New(IntPtr ptr, int size, int useageSize, DateTime expireTime)
        {
            return new UnmanagedCacheStub(ptr, size, useageSize, expireTime);
        }

        #endregion
    }
}