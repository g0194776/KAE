using System;

namespace KJFramework
{
    /// <summary>
    ///     只读的缓存生命周期租约元接口，提供了相关的基本操作
    /// </summary>
    public interface IReadonlyCacheLease
    {
        /// <summary>
        ///     获取一个值，该值表示了当前的缓存是否支持超时检查
        /// </summary>
        bool CanTimeout { get; }
        /// <summary>
        ///     获取一个值，该值表示了当前的缓存是否已经处于死亡的状态
        /// </summary>
        bool IsDead { get; }
        /// <summary>
        ///     获取生命周期创建的时间
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     获取超时时间
        /// </summary>
        DateTime ExpireTime { get; }
    }
}