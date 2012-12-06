using System;
using System.Threading;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Tasks;

namespace KJFramework.Net.Cloud.Tasks
{
    /// <summary>
    ///     请求任务父类，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    internal class RequestTask<T> : Task, IRequestTask<T>
    {
        #region 析构函数

        ~RequestTask()
        {
            Dispose();
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     请求任务父类，提供了相关的基本操作。
        /// </summary>
        internal RequestTask() : this(Guid.Empty, Guid.Empty)
        {
            
        }

        /// <summary>
        ///     请求任务父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="nodeId">网络节点标示</param>
        /// <param name="channelId">传输通道标示</param>
        internal RequestTask(Guid nodeId, Guid channelId)
        {
            _taskId = Guid.NewGuid();
            _nodeId = nodeId;
            _channelId = channelId;
        }

        #endregion

        #region Abstract of Task

        /// <summary>
        ///     取消任务
        /// </summary>
        public override void Cancel()
        {
            _isCanceled = true;
            if (_resetEvent != null)
            {
                _resetEvent.Set();
            }
        }

        /// <summary>
        ///     执行任务
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (_processor == null)
                {
                    throw new System.Exception("未知的功能处理器。");
                }
                _resetEvent = new AutoResetEvent(false);
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    _resultMessage = _processor.Process(_channelId, _message);
                    _resetEvent.Set();
                });
                //Timeout, default 3s.
                if (!_resetEvent.WaitOne(_timeoutValue))
                {
                    _isTimeout = true;
                    ExecuteTimeoutHandler(new LightSingleArgEventArgs<T>(_message));
                    ExecuteFailHandler(null);
                    return;
                }
                if (!_isCanceled)
                {
                    _isFinished = true;
                    ExecuteSuccessfulHandler(null);
                }
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

        protected AutoResetEvent _resetEvent;
        protected T _message;
        protected T _resultMessage;
        protected bool _hasRented;
        protected Guid _channelId;
        protected Guid _nodeId;
        protected Guid _taskId;
        protected IFunctionProcessor<T> _processor;
        protected int _timeoutValue = 3000;
        protected bool _isTimeout;

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
        ///     获取或设置对应的传输通道编号
        /// </summary>
        public Guid ChannelId
        {
            get { return _channelId; }
            set { _channelId = value; }
        }

        /// <summary>
        ///     获取任务唯一标示
        /// </summary>
        public Guid TaskId
        {
            get { return _taskId; }
        }

        /// <summary>
        ///     获取或设置对应的网络节点编号
        /// </summary>
        public Guid NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value; }
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
            _resetEvent = null;
            _processor = null;
            _message = default(T);
            _resultMessage = default(T);
            _isFinished = false;
            _isCanceled = false;
            _channelId = Guid.Empty;
            _nodeId = Guid.Empty;
            _taskId = Guid.NewGuid();
            _priority = TaskPriority.Low;
            _hasRented = false;
            _expiredTime = null;
            _createTime = DateTime.Now;
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