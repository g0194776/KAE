using System;

namespace KJFramework
{
    /// <summary>
    ///     可控的生命周期元接口，提供了相关的基本操作
    /// </summary>
    public interface ILeasable
    {
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