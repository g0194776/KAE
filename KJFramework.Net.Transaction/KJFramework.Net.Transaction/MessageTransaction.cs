using System;
using KJFramework.EventArgs;
using KJFramework.Logger;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     ��Ϣ�������ڳ���������Ϣ�����ר������
    /// </summary>
    /// <typeparam name="T">��Ϣ��������</typeparam>
    public abstract class MessageTransaction<T> : Transaction, IMessageTransaction<T>
        where T : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     ��Ϣ�������ڳ���������Ϣ�����ר������
        ///     <para>* ֻ�з�����״̬��ʹ�ô˹���</para>
        /// </summary>
        public MessageTransaction()
        { }

        /// <summary>
        ///     ��Ϣ�������ڳ���������Ϣ�����ר������
        ///     <para>* ʹ�ô˹��죬�������õ�ǰ��������Զ����ʱ</para>
        /// </summary>
        /// <param name="channel">��ϢͨѶ�ŵ�</param>
        public MessageTransaction(IMessageTransportChannel<T> channel)
        {
            _channel = channel;
        }

        /// <summary>
        ///     ��Ϣ�������ڳ���������Ϣ�����ר������
        /// </summary>
        /// <param name="lease">����������Լ</param>
        /// <param name="channel">��ϢͨѶ�ŵ�</param>
        public MessageTransaction(ILease lease, IMessageTransportChannel<T> channel)
            : base(lease)
        {
            _channel = channel;
        }

        #endregion

        #region Implementation of IMessageTransaction<T>

        protected T _request;
        protected T _response;
        protected bool _needResponse;
        protected readonly IMessageTransportChannel<T> _channel;

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�������Ƿ���Ҫ��Ӧ��Ϣ
        /// </summary>
        public bool NeedResponse
        {
            get { return _needResponse; }
            set { _needResponse = value; }
        }

        /// <summary>
        ///     ��ȡ������������Ϣ
        /// </summary>
        public virtual  T Request
        {
            get { return _request; }
            set { _request = value; }
        }

        /// <summary>
        ///     ��ȡ��������Ӧ��Ϣ
        /// </summary>
        public virtual T Response
        {
            get { return _response; }
            set { _response = value; }
        }

        /// <summary>
        ///     ������Ӧ��Ϣ�������������
        /// </summary>
        /// <param name="response">��Ӧ��Ϣ</param>
        public virtual void SetResponse(T response)
        {
            _response = response;
            try { ResponseArrivedHandler(new LightSingleArgEventArgs<T>(_response)); }
            catch (System.Exception ex) { Logs.Logger.Log(ex); }
        }

        /// <summary>
        ///     ����һ��������Ϣ
        /// </summary>
        /// <param name="message">������Ϣ</param>
        public abstract void SendRequest(T message);
        /// <summary>
        ///     ����һ����Ӧ��Ϣ
        /// </summary>
        /// <param name="message">��Ӧ��Ϣ</param>
        public abstract void SendResponse(T message);

        /// <summary>
        ///     ��ȡ��صĴ����ŵ�
        /// </summary>
        public IMessageTransportChannel<T> GetChannel()
        {
            return _channel;
        }

        /// <summary>
        ///     ���ﳬʱ�¼�
        /// </summary>
        public event EventHandler Timeout;
        protected void TimeoutHandler(System.EventArgs e)
        {
            EventHandler handler = Timeout;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     ����ʧ���¼�
        /// </summary>
        public event EventHandler Failed;
        protected void FailedHandler(System.EventArgs e)
        {
            EventHandler handler = Failed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     ��Ӧ��Ϣ�ִ��¼�
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