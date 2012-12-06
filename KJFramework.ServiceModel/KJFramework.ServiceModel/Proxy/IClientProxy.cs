using System;
using KJFramework.EventArgs;
using KJFramework.ServiceModel.Enums;
using KJFramework.ServiceModel.Objects;

namespace KJFramework.ServiceModel.Proxy
{
    public delegate void AsyncMethodCallback(IAsyncCallResult result);
    /// <summary>
    ///     客户端代理元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">服务端契约接口</typeparam>
    public interface IClientProxy<T>
    {
        /// <summary>
        ///     获取代理器状态
        /// </summary>
        ProxyStatus Status { get; }
        /// <summary>
        ///     获取契约信道
        /// </summary>
        T Channel { get; }
        /// <summary>
        ///     关闭当前的代理器
        /// </summary>
        void Close();
        /// <summary>
        ///     客户端代理器发生错误事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<System.Exception>> OnError;
    }
}