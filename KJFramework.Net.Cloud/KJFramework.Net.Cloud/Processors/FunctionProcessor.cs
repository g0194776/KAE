using System;
using KJFramework.EventArgs;

namespace KJFramework.Net.Cloud.Processors
{
    /// <summary>
    ///     功能处理器抽象父类，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public abstract class FunctionProcessor<T> : IFunctionProcessor<T>
    {
        #region Constructor

        /// <summary>
        ///     功能处理器抽象父类，提供了相关的基本操作。
        /// </summary>
        public FunctionProcessor()
        {
            _id = Guid.NewGuid();
        }

        #endregion

        #region Implementation of IDisposable

        protected object _tag;
        protected Guid _id;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IFunctionProcessor<T>

        /// <summary>
        ///   获取或设置附属属性
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     获取唯一标示
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     处理一个请求消息
        /// </summary>
        /// <param name="id">传输通道标示</param>
        /// <param name="message">请求消息</param>
        /// <returns>
        ///     返回回馈消息
        ///     <para>* 如果返回为null, 则证明没有反馈消息。</para>
        /// </returns>
        public abstract T Process(Guid id, T message);

        /// <summary>
        ///     处理请求消息成功事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<T>> ProcessSuccessfully;
        protected void ProcessSuccessfullyHandler(LightSingleArgEventArgs<T> e)
        {
            EventHandler<LightSingleArgEventArgs<T>> handler = ProcessSuccessfully;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     处理请求消息失败事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<T>> ProcessFailed;
        protected void ProcessFailedHandler(LightSingleArgEventArgs<T> e)
        {
            EventHandler<LightSingleArgEventArgs<T>> handler = ProcessFailed;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}