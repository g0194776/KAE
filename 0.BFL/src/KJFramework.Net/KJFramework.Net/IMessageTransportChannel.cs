using System;
using System.Collections.Generic;
using KJFramework.EventArgs;
using KJFramework.Net.Identities;
using KJFramework.Net.Managers;
using KJFramework.Net.ProtocolStacks;

namespace KJFramework.Net
{
    /// <summary>
    ///     消息传输信道元接口，提供了相关的基本操作
    /// </summary>
    public interface IMessageTransportChannel<T> : ITransportChannel
    {
        /// <summary>
        ///     获取协议栈
        /// </summary>
        IProtocolStack ProtocolStack { get; }
        /// <summary>
        ///     获取或设置封包片消息管理器
        /// </summary>
        IMultiPacketManager<T> MultiPacketManager { get; set; }
        /// <summary>
        ///     发送一个消息
        /// </summary>
        /// <param name="obj">要发送的消息</param>
        /// <returns>返回发送的字节数</returns>
        int Send(T obj);
        /// <summary>
        ///     接收到消息事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<List<T>>> ReceivedMessage;
        /// <summary>
        ///   生成一个请求的事务唯一标示
        /// </summary>
        /// <param name="messageId">消息编号</param>
        /// <returns>返回创建后的事务唯一标示</returns>
        TransactionIdentity GenerateRequestIdentity(uint messageId);
    }
}