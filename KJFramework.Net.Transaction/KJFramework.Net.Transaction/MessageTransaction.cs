using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Tracing;
using System;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     消息事务，用于承载网络消息处理的专用事务
    /// </summary>
    /// <typeparam name="T">消息父类类型</typeparam>
    public abstract class MessageTransaction<T> : Transaction, IMessageTransaction<T>
    {
        #region Constructor

        /// <summary>
        ///     消息事务，用于承载网络消息处理的专用事务
        ///     <para>* 只有非正常状态才使用此构造</para>
        /// </summary>
        protected MessageTransaction()
        {
            CreateTime = DateTime.Now;
            RequestTime = DateTime.MaxValue;
            ResponseTime = DateTime.MaxValue;
        }

        /// <summary>
        ///     消息事务，用于承载网络消息处理的专用事务
        ///     <para>* 使用此构造，将会设置当前的事务永远不超时</para>
        /// </summary>
        /// <param name="channel">消息通讯信道</param>
        protected MessageTransaction(IMessageTransportChannel<T> channel)
        {
            _channel = channel;
            CreateTime = DateTime.Now;
            RequestTime = DateTime.MaxValue;
            ResponseTime = DateTime.MaxValue;
        }

        /// <summary>
        ///     消息事务，用于承载网络消息处理的专用事务
        /// </summary>
        /// <param name="lease">生命周期租约</param>
        /// <param name="channel">消息通讯信道</param>
        protected MessageTransaction(ILease lease, IMessageTransportChannel<T> channel)
            : base(lease)
        {
            _channel = channel;
            CreateTime = DateTime.Now;
            RequestTime = DateTime.MaxValue;
            ResponseTime = DateTime.MaxValue;
        }

        #endregion

        #region Members

        protected TimeSpan _transactionTimeout = Global.TransactionTimeout;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(MessageTransaction<T>));
        /// <summary>
        ///     获取或设置当前事务的唯一标示
        /// </summary>
        public TransactionIdentity Identity { get; internal set; }
        /// <summary>
        ///     获取事务的创建时间
        /// </summary>
        public DateTime CreateTime { get; protected set; }
        /// <summary>
        ///     获取成功操作后的请求时间
        /// </summary>
        public DateTime RequestTime { get; protected set; }
        /// <summary>
        ///     获取成功操作后的应答时间
        /// </summary>
        public DateTime ResponseTime { get; protected set; }
        /// <summary>
        ///     获取或设置事务管理器
        /// </summary>
        public ITransactionManager<T> TransactionManager { get; set; }

        #endregion

        #region Implementation of IMessageTransaction<T>

        protected T _request;
        protected T _response;
        protected bool _needResponse;
        protected readonly IMessageTransportChannel<T> _channel;

        /// <summary>
        ///     获取或设置一个值，该值标示了当前的事务是否需要响应消息
        /// </summary>
        public bool NeedResponse
        {
            get { return _needResponse; }
            set { _needResponse = value; }
        }

        /// <summary>
        ///     获取或设置请求消息
        /// </summary>
        public virtual  T Request
        {
            get { return _request; }
            set { _request = value; }
        }

        /// <summary>
        ///     获取或设置响应消息
        /// </summary>
        public virtual T Response
        {
            get { return _response; }
            set { _response = value; }
        }

        /// <summary>
        ///     设置响应消息，并激活处理流程
        /// </summary>
        /// <param name="response">响应消息</param>
        public virtual void SetResponse(T response)
        {
            _response = response;
            try { ResponseArrivedHandler(new LightSingleArgEventArgs<T>(_response)); }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }

        /// <summary>
        ///     发送一个请求消息
        /// </summary>
        /// <param name="message">请求消息</param>
        public abstract void SendRequest(T message);
        /// <summary>
        ///     发送一个响应消息
        /// </summary>
        /// <param name="message">响应消息</param>
        public abstract void SendResponse(T message);

        /// <summary>
        ///     获取相关的传输信道
        /// </summary>
        public IMessageTransportChannel<T> GetChannel()
        {
            return _channel;
        }

        internal void SetTimeout()
        {
            try { TimeoutHandler(null); }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }

        /// <summary>
        ///     事物超时事件
        /// </summary>
        public event EventHandler Timeout;
        protected void TimeoutHandler(System.EventArgs e)
        {
            EventHandler handler = Timeout;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     事物失败事件
        /// </summary>
        public event EventHandler Failed;
        protected void FailedHandler(System.EventArgs e)
        {
            EventHandler handler = Failed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     响应消息抵达事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<T>> ResponseArrived;
        protected void ResponseArrivedHandler(LightSingleArgEventArgs<T> e)
        {
            EventHandler<LightSingleArgEventArgs<T>> handler = ResponseArrived;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}