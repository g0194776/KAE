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
    ///     ���������࣬�ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    internal class RequestTask<T> : Task, IRequestTask<T>
    {
        #region ���캯��

        /// <summary>
        ///     ���������࣬�ṩ����صĻ���������
        /// </summary>
        internal RequestTask() : this(null, null)
        {
            
        }

        /// <summary>
        ///     ���������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="node">����ڵ�</param>
        /// <param name="channel">����ͨ��</param>
        internal RequestTask(NetworkNode<T> node, IMessageTransportChannel<T> channel)
        {
            _taskId = Guid.NewGuid();
            _node = node;
            _channel = channel;
        }

        #endregion

        #region Abstract of Task

        /// <summary>
        ///     ȡ������
        /// </summary>
        public override void Cancel()
        {
            _isCanceled = true;
        }

        /// <summary>
        ///     ִ������
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
        ///     ��ȡ�����ö�Ӧ�Ĵ���ͨ��
        /// </summary>
        public IMessageTransportChannel<T> Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        /// <summary>
        ///     ��ȡ����Ψһ��ʾ
        /// </summary>
        public Guid TaskId
        {
            get { return _taskId; }
        }

        /// <summary>
        ///     ��ȡ�����ö�Ӧ������ڵ�
        /// </summary>
        public NetworkNode<T> Node
        {
            get { return _node; }
            set { _node = value; }
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