using System;

namespace KJFramework.Net.Channels.ReplyNow
{
    /// <summary>
    ///     请求通道，提供了相关的基本操作
    /// </summary>
    public interface IRequestChannel : ICommunicationChannelAddress, IProtocolChannel
    {
        /// <summary>
        ///     请求一个消息到远程终结点
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message">请求的消息</param>
        /// <returns>返回应答消息</returns>
        T Request<T>(T message);
        /// <summary>
        ///     异步请求一个消息到远程终结点
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message">请求的消息</param>
        /// <param name="callback">回调函数</param>
        /// <param name="state">状态</param>
        /// <returns>返回异步结果</returns>
        IAsyncResult BeginRequest<T>(T message, AsyncCallback callback, Object state);
        /// <summary>
        ///     异步请求一个消息到远程终结点
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="result">异步结果</param>
        /// <returns>返回应答消息</returns>
        T EndRequest<T>(IAsyncResult result);
    }
}