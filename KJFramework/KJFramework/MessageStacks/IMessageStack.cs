using System;
using KJFramework.EventArgs;
using KJFramework.Plugin;
using KJFramework.Statistics;

namespace KJFramework.MessageStacks
{
    /// <summary>
    ///     消息协议栈元接口，提供了相关的基本操作。
    /// </summary>
    public interface IMessageStack<TMessage> : IStatisticable<IStatistic>,  IPlugin, IDisposable
    {
        /// <summary>
        ///     提取一个具有指定协议编号的消息
        /// </summary>
        /// <param name="protocolId">协议编号</param>
        /// <returns>返回指定消息</returns>
        TMessage Pickup(int protocolId);
        /// <summary>
        ///      提取一个具有指定协议编号以及服务编号的消息
        /// </summary>
        /// <param name="protocolId">协议编号</param>
        /// <param name="serviceId">服务编号</param>
        /// <returns>返回指定消息</returns>
        TMessage Pickup(int protocolId, int serviceId);
        /// <summary>
        ///      提取一个具有指定协议编号，服务编号以及详细服务编号的消息
        /// </summary>
        /// <param name="protocolId">协议编号</param>
        /// <param name="serviceId">服务编号</param>
        /// <param name="detailServiceId">详细服务编号</param>
        /// <returns>返回指定消息</returns>
        TMessage Pickup(int protocolId, int serviceId, int detailServiceId);
        /// <summary>
        ///     获取当前协议栈中的消息数量
        /// </summary>
        int Count { get; }
        /// <summary>
        ///     获取协议栈名称
        /// </summary>
        String Name { get; }
        /// <summary>
        ///     获取协议栈版本
        /// </summary>
        String Version { get; }
        /// <summary>
        ///     提取消息成功事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<TMessage>> PickupSuccessfully;
        /// <summary>
        ///     提取消息失败事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<int>> PickupFailed;
    }
}