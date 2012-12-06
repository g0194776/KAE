using System;

namespace KJFramework.Net.Channels.Spy
{
    /// <summary>
    ///     消息拦截器，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">消息父类类型</typeparam>
    public interface IMessageSpy<T>
    {
        /// <summary>
        ///     获取支持拦截的消息类型
        /// </summary>
        Type SupportType { get; }
        /// <summary>
        ///     拦截一个消息
        /// </summary>
        /// <param name="message">被拦截的消息</param>
        /// <returns>
        ///     返回需要响应的消息
        ///     <para>* 如果无需响应，则返回null.</para>
        /// </returns>
        T Spy(T message);
    }
}