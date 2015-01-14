using System;

namespace KJFramework.ApplicationEngine.Apps.DistributedLock
{
    /// <summary>
    ///    锁管理器
    /// </summary>
    internal static class LockingManager
    {
        #region Methods.

        /// <summary>
        ///    获取或创建一个具有指定资源名称的分布式锁
        /// </summary>
        /// <param name="name">被锁定的资源名称</param>
        /// <param name="lockingType">分布式锁类型</param>
        /// <param name="owner">设置当前分布式锁的拥有者</param>
        /// <param name="callback">获取到分布式锁后会激活此回调函数</param>
        /// <returns>返回一个对应的分布式锁实例</returns>
        public static IDistributedLock GetOrNewLockAsync(string name, byte lockingType, object owner, Action<IDistributedLock> callback)
        {
            return null;
        }

        #endregion
    }
}