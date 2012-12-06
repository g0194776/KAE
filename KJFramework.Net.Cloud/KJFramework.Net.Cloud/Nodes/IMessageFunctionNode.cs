using System;
using KJFramework.Net.Cloud.Exceptions;
using KJFramework.Net.Cloud.Processors;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     基于消息驱动机制的功能节点
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public interface IMessageFunctionNode<T> : IFunctionNode<T>
    {
        /// <summary>
        ///     获取处理器个数
        /// </summary>
        int ProcessorCount { get; }/// <summary>
        ///     探测一个消息是否能被当前处理器所处理
        /// </summary>
        /// <param name="id">传输通道标示</param>
        /// <param name="message">探测的消息</param>
        /// <returns>返回一个可以支持的处理器</returns>
        IFunctionProcessor<T> CanProcess(Guid id, T message);
        /// <summary>
        ///     获取具有指定标示的功能处理器
        /// </summary>
        /// <param name="id">功能处理器标示</param>
        /// <returns>返回功能处理器</returns>
        IFunctionProcessor<T> GetProcessor(Guid id);
        /// <summary>
        ///     处理一个消息
        /// </summary>
        /// <param name="id">传输通道标示</param>
        /// <param name="message">要处理的消息</param>
        /// <returns>
        ///     返回反馈消息
        ///     <para>* 如果返回null, 则认为没有反馈消息。</para>
        /// </returns>
        /// <exception cref="NotSupportedProcessException">不支持的操作</exception>
        /// <exception cref="FunctionProcessorNotEnableException">功能处理器未启动</exception>
        T Process(Guid id, T message);
    }
}