using System;
using KJFramework.EventArgs;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     协议通道元接口，提供了相关的基本操作。
    /// </summary>
    public interface IProtocolChannel : IServiceChannel
    {
        /// <summary>
        ///     创建协议消息
        /// </summary>
        /// <typeparam name="TMessage">协议消息类型</typeparam>
        /// <returns>返回协议消息</returns>
        TMessage CreateProtocolMessage<TMessage>();
        /// <summary>
        ///     请求事件
        /// </summary>
        event EventHandler<LightMultiArgEventArgs<Object>> Requested;
        /// <summary>
        ///     回馈事件
        /// </summary>
        event EventHandler<LightMultiArgEventArgs<Object>> Responsed;
    }
}