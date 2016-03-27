using System;
using KJFramework.Data.Synchronization.Enums;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     订阅者接口
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        ///     获取订阅者的唯一标示
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     获取订阅者的订阅时间
        /// </summary>
        DateTime StartTime { get; }
        /// <summary>
        ///     获取订阅者当前的状态
        /// </summary>
        SubscriberState State { get; }
        /// <summary>
        ///     订阅者断开连接事件
        /// </summary>
        event EventHandler Disconnected;
    }
}