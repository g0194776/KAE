using System;
using KJFramework.Messages.Contracts;
using KJFramework.Net.ProtocolStacks;

namespace KJFramework.Net.OneWay
{
    /// <summary>
    ///     输出通道元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">消息父类类型</typeparam>
    class OutputChannel<T> : OnewayChannel<T>, IOutputChannel<T>
        where T : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     输出通道元接口，提供了相关的基本操作。
        /// </summary>
        /// <param name="protocolStack">协议栈</param>
        public OutputChannel(IProtocolStack protocolStack)
            : base(protocolStack)
        {
        }

        /// <summary>
        ///     输出通道元接口，提供了相关的基本操作。
        /// </summary>
        /// <param name="channel">基于流的通讯信道</param>
        /// <param name="protocolStack">协议栈</param>
        public OutputChannel(IRawTransportChannel channel, IProtocolStack protocolStack)
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
            //use raw transport channel to send current response msg.
            Send(message);
        }

        #endregion

        #region Implementation of IOutputChannel<T>

        /// <summary>
        ///     请求一个消息到远程终结点
        /// </summary>
        /// <param name="message">请求的消息</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <exception cref="Exception">发送失败</exception>
        public int Send(T message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (!_connected) throw new System.Exception("You cannot call \"Send\" operation on a disconnected channel!");
            message.Bind();
            if (!message.IsBind) throw new ArgumentNullException("Cannot bind a message to binary data, type: " + message);
            return _channel.Send(message.Body);
        }

        /// <summary>
        ///     异步请求一个消息到远程终结点
        /// </summary>
        /// <param name="message">请求的消息</param>
        /// <returns>返回异步结果</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <exception cref="Exception">发送失败</exception>
        public void SendAsync(T message)
        {
            Send(message);
        }

        #endregion
    }
}