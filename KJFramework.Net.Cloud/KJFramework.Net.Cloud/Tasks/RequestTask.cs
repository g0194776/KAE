using System;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Net.Channels;
using KJFramework.Tasks;

namespace KJFramework.Net.Cloud.Tasks
{
    /// <summary>
    ///     请求任务父类，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    internal class RequestTask<T> : Task, IRequestTask<T>
    {
        #region 构造函数

        /// <summary>
        ///     请求任务父类，提供了相关的基本操作。
        /// </summary>
        internal RequestTask() : this(null, null)
        {
            
        }

        /// <summary>
        ///     请求任务父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="node">网络节点</param>
        /// <param name="channel">传输通道</param>
        internal RequestTask(NetworkNode<T> node, IMessageTransportChannel<T> channel)
        {
            _taskId = Guid.NewGuid();
            _node = node;
            _channel = channel;
        }

        #endregion

        #region Abstract of Task

        /// <summary>
        ///     取消任务
        /// </summary>
        public override void Cancel()
        {
            _isCanceled = true;
        }

        /// <summary>
        ///     执行任务
        /// </summary>
        public override void Execute()
        {
            try
            {
                _resultMessage = _processor.Process(_channel.Key, _message);
                _isFinished = true;
                ExecuteSuccessfulHandler(null);
            }
            catch (System.Exception ex)
            {
                _isFinished = false;
                Logs.Logger.Log(ex);
                ExecuteFailHandler(null);
            }
        }

        #endregion

        #region Members

        protected T _message;
        protected T _resultMessage;
        protected bool _hasRented;
        protected IMessageTransportChannel<T> _channel;
        protected NetworkNode<T> _node;
        protected IFunctionProcessor<T> _processor;
        protected int _timeoutValue = 3000;
        protected bool _isTimeout;
        private readonly Guid _taskId;

        #endregion

        #region Implementation of IRequestTask<T>

        /// <summary>
        ///     获取或设置消息
        /// </summary>
        public T Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        ///     获取执行结果
        /// </summary>
        public T ResultMessage
        {
            get { return _resultMessage; }
        }

        /// <summary>
        ///     获取或设置一个值，该值标示了当前任务是否已经被出租
        /// </summary>
        public bool HasRented
        {
            get { return _hasRented; }
            set { _hasRented = value; }
        }

        /// <summary>
        ///     获取一个值，该值标示了当前的任务是否已经超时
        /// </summary>
        public bool IsTimeout
        {
            get { return _isTimeout; }
        }

        /// <summary>
        ///     获取或设置对应的传输通道
        /// </summary>
        public IMessageTransportChannel<T> Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        /// <summary>
        ///     获取任务唯一标示
        /// </summary>
        public Guid TaskId
        {
            get { return _taskId; }
        }

        /// <summary>
        ///     获取或设置对应的网络节点
        /// </summary>
        public NetworkNode<T> Node
        {
            get { return _node; }
            set { _node = value; }
        }

        /// <summary>
        ///     获取或设置功能处理器
        /// </summary>
        public IFunctionProcessor<T> Processor
        {
            get { return _processor; }
            set { _processor = value; }
        }

        /// <summary>
        ///     重置当前任务的所有状态
        /// </summary>
        public void Reset()
        {
            _processor = null;
            _message = default(T);
            _resultMessage = default(T);
            _isFinished = false;
            _isCanceled = false;
            _priority = TaskPriority.Low;
            _hasRented = false;
            _expiredTime = null;
            _description = null;
        }

        /// <summary>
        ///     处理超时事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<T>> ExecuteTimeout;
        protected void ExecuteTimeoutHandler(LightSingleArgEventArgs<T> e)
        {
            EventHandler<LightSingleArgEventArgs<T>> handler = ExecuteTimeout;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}