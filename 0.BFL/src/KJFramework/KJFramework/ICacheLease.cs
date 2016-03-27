using System;

namespace KJFramework
{
    /// <summary>
    ///     缓存生命周期租约元接口，提供了相关的基本操作
    /// </summary>
    public interface ICacheLease
    {
        /// <summary>
        ///     获取或设置一个值，该值表示了当前的缓存是否支持超时检查
        ///     <para>* 如果CanTimeout = false, 则ExpireTime = max(DateTime)</para>
        /// </summary>
        bool CanTimeout { get; set; }
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
        /// <summary>
        ///     将当前缓存的生命周期置为死亡状态
        /// </summary>
        void Discard();
        /// <summary>
        ///     将当前租期续约一段时间
        /// </summary>
        /// <param name="timeSpan">续约时间</param>
        /// <returns>返回续约后的到期时间</returns>
        /// <exception cref="System.Exception">更新失败</exception>
        DateTime Renew(TimeSpan timeSpan);
    }
}