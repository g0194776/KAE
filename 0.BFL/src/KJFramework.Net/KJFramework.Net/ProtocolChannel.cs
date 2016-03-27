using System;
using KJFramework.EventArgs;

namespace KJFramework.Net
{
    /// <summary>
    ///     协议通道抽象基类，提供了相关的基本操作。
    /// </summary>
    internal abstract class ProtocolChannel : ServiceChannel, IProtocolChannel
    {
        #region IProtocolChannel 成员

        /// <summary>
        ///     创建协议消息
        /// </summary>
        /// <typeparam name="TMessage">协议消息类型</typeparam>
        /// <returns>返回协议消息</returns>
        public abstract TMessage CreateProtocolMessage<TMessage>();

        /// <summary>
        ///     请求事件
        /// </summary>
        public event EventHandler<LightMultiArgEventArgs<Object>> Requested;
        protected void RequestedHandler(LightMultiArgEventArgs<object> e)
        {
            EventHandler<LightMultiArgEventArgs<object>> requested = Requested;
            if (requested != null) requested(this, e);
        }

        /// <summary>
        ///     回馈事件
        /// </summary>
        public event EventHandler<LightMultiArgEventArgs<Object>> Responsed;
        protected void InvokeResponsed(LightMultiArgEventArgs<object> e)
        {
            EventHandler<LightMultiArgEventArgs<object>> responsed = Responsed;
            if (responsed != null) responsed(this, e);
        }

        #endregion
    }
}