using System;
using KJFramework.EventArgs;

namespace KJFramework.Net.Cloud.Processors
{
    /// <summary>
    ///     功能处理器元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public interface IFunctionProcessor<T> : IDisposable
    {
        /// <summary>
        ///   获取或设置附属属性
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     获取唯一标示
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     处理一个请求消息
        /// </summary>
        /// <param name="id">传输通道标示</param>
        /// <param name="message">请求消息</param>
        /// <returns>
        ///     返回回馈消息
        ///     <para>* 如果返回为null, 则证明没有反馈消息。</para>
        /// </returns>
        T Process(Guid id, T message);
        /// <summary>
        ///     处理请求消息成功事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> ProcessSuccessfully;
        /// <summary>
        ///     处理请求消息失败事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> ProcessFailed;
    }
}