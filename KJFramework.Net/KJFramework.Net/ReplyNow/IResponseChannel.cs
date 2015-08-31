using System;

namespace KJFramework.Net.ReplyNow
{
    /// <summary>
    ///     应答通道，提供了相关的基本操作
    /// </summary>
    public interface IResponseChannel : ICommunicationChannelAddress, IProtocolChannel
    {
        /// <summary>
        ///     应答一个消息到远程终结点
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="message">请求的消息</param>
        /// <returns>返回应答消息</returns>
        TMessage Response<TMessage>(TMessage message);
        /// <summary>
        ///     异步应答一个消息到远程终结点
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="message">请求的消息</param>
        /// <param name="callback">回调函数</param>
        /// <param name="state">状态</param>
        /// <returns>返回异步结果</returns>
        IAsyncResult BeginResponse<TMessage>(TMessage message, AsyncCallback callback, Object state);
        /// <summary>
        ///     异步应答一个消息到远程终结点
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="result">异步结果</param>
        /// <returns>返回应答消息</returns>
        TMessage EndResponse<TMessage>(IAsyncResult result);
    }
}