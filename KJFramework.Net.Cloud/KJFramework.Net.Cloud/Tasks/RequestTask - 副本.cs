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
    ///     ���������࣬�ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    internal class RequestTask<T> : Task, IRequestTask<T>
    {
        #region ��������

        ~RequestTask()
        {
            Dispose();
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     ���������࣬�ṩ����صĻ���������
        /// </summary>
        internal RequestTask() : this(Guid.Empty, Guid.Empty)
        {
            
        }

        /// <summary>
        ///     ���������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="nodeId">����ڵ��ʾ</param>
        /// <param name="channelId">����ͨ����ʾ</param>
        internal RequestTask(Guid nodeId, Guid channelId)
        {
            _taskId = Guid.NewGuid();
            _nodeId = nodeId;
            _channelId = channelId;
        }

        #endregion

        #region Abstract of Task

        /// <summary>
        ///     ȡ������
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
        ///     ִ������
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (_processor == null)
                {
                    throw new System.Exception("δ֪�Ĺ��ܴ�������");
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
        ///     ��ȡ��������Ϣ
        /// </summary>
        public T Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        ///     ��ȡִ�н��
        /// </summary>
        public T ResultMessage
        {
            get { return _resultMessage; }
        }

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ��Ѿ�������
        /// </summary>
        public bool HasRented
        {
            get { return _hasRented; }
            set { _hasRented = value; }
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�������Ƿ��Ѿ���ʱ
        /// </summary>
        public bool IsTimeout
        {
            get { return _isTimeout; }
        }

        /// <summary>
        ///     ��ȡ�����ö�Ӧ�Ĵ���ͨ�����
        /// </summary>
        public Guid ChannelId
        {
            get { return _channelId; }
            set { _channelId = value; }
        }

        /// <summary>
        ///     ��ȡ����Ψһ��ʾ
        /// </summary>
        public Guid TaskId
        {
            get { return _taskId; }
        }

        /// <summary>
        ///     ��ȡ�����ö�Ӧ������ڵ���
        /// </summary>
        public Guid NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value; }
        }

        /// <summary>
        ///     ��ȡ�����ù��ܴ�����
        /// </summary>
        public IFunctionProcessor<T> Processor
        {
            get { return _processor; }
            set { _processor = value; }
        }

        /// <summary>
        ///     ���õ�ǰ���������״̬
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
        ///     ����ʱ�¼�
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