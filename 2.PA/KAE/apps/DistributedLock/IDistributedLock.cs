using System;

namespace DistributedLock
{
    /// <summary>
    ///    分布式锁对象接口
    /// </summary>
    internal interface IDistributedLock
    {
        #region Members.

        /// <summary>
        ///    获取当前分布式锁实例的唯一编号
        /// </summary>
        Guid LockingId { get; }
        /// <summary>
        ///    获取当前锁的拥有者
        /// </summary>
        object Owner { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///    释放当前锁
        /// </summary>
        void Release();

        #endregion
    }
}