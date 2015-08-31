using System;

namespace KJFramework.Cores
{
    /// <summary>
    ///     非托管缓存槽元接口，提供了相关的基本操作
    /// </summary>
    public interface IUnmanagedCacheSlot
    {
        /// <summary>
        ///     获取内存句柄
        /// </summary>
        IntPtr Handle { get; }
        /// <summary>
        ///     放弃当前非托管内存
        /// </summary>
        void Discard();
        /// <summary>
        ///     获取生命租约
        /// </summary>
        /// <returns>返回生命租约</returns>
        ICacheLease GetLease();
        /// <summary>
        ///     获取缓存数据
        /// </summary>
        /// <returns>返回缓存内容</returns>
        byte[] GetValue();
        /// <summary>
        ///     设置缓存数据
        /// </summary>
        /// <param name="data">要设置的数据</param>
        void SetValue(byte[] data);
    }
}