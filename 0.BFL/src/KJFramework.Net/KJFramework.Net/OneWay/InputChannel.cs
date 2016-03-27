using System;
using KJFramework.Messages.Contracts;
using KJFramework.Net.ProtocolStacks;

namespace KJFramework.Net.OneWay
{
    /// <summary>
    ///     输入通道元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">消息父类类型</typeparam>
    class InputChannel<T> : OnewayChannel<T> 
        where T : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     输入通道元接口，提供了相关的基本操作。
        /// </summary>
        /// <param name="protocolStack">协议栈</param>
        public InputChannel(IProtocolStack protocolStack)
            : base(protocolStack)
        {
        }

        /// <summary>
        ///     输入通道元接口，提供了相关的基本操作。
        /// </summary>
        /// <param name="channel">基于流的通讯信道</param>
        /// <param name="protocolStack">协议栈</param>
        public InputChannel(IRawTransportChannel channel, IProtocolStack protocolStack)
            : base(channel, protocolStack)
        {
        }

        #endregion

        #region Overrides of OnewayChannel<T>

        /// <summary>
        ///     使用此方法来发送响应消息
        /// </summary>
        /// <param name="message">响应消息</param>
        protected override void SendCallbackAsync(T message)
        {
            if (!_connected || message == null) return;
            message.Bind();
            if (!message.IsBind) throw new ArgumentNullException("Cannot bind a message to binary data, type: " + message);
            _channel.Send(message.Body);
        }

        #endregion
    }
}