using System;
using System.Threading;
using KJFramework.Enums;

namespace KJFramework.Tasks
{
    /// <summary>
    ///      ��������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class Task : ITask
    {
        #region ��������

        ~Task()
        {
            Dispose();
        }

        #endregion

        #region ITask��Ա

        protected int _id;
        protected bool _isFinished;
        protected bool _isCanceled;
        protected DateTime _createTime = DateTime.Now;
        protected DateTime? _expiredTime;
        protected TaskPriority _priority;
        protected String _description;


        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        ///     ��ȡ����������Ψһ��ʾ
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ��Ѿ����
        /// </summary>
        public bool IsFinished
        {
            get { return _isFinished; }
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ��Ѿ�ȡ��
        /// </summary>
        public bool IsCanceled
        {
            get { return _isCanceled; }
        }

        /// <summary>
        ///     ��ȡ���񴴽�ʱ��
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        /// <summary>
        ///     ��ȡ�������������ʱ��
        ///             * �������Ϊnull, ���ʾ��Զ������
        /// </summary>
        public DateTime? ExpiredTime
        {
            get { return _expiredTime; }
            set { _expiredTime = value; }
        }

        /// <summary>
        ///     ��ȡ�������������ȼ�
        /// </summary>
        public TaskPriority Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        /// <summary>
        ///     ȡ������
        /// </summary>
        public abstract void Cancel();
        /// <summary>
        ///     ִ������
        /// </summary>
        public abstract void Execute();

        /// <summary>
        ///     �첽ִ������
        /// </summary>
        public virtual void ExecuteAsyn()
        {
            ThreadPool.QueueUserWorkItem(Execute);
        }

        public event EventHandler ExecuteSuccessful;
        protected void ExecuteSuccessfulHandler(System.EventArgs e)
        {
            EventHandler handler = ExecuteSuccessful;
            if (handler != null) handler(this, e);
        }

        public event EventHandler ExecuteFail;
        protected void ExecuteFailHandler(System.EventArgs e)
        {
            EventHandler handler = ExecuteFail;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region ����

        /// <summary>
        ///     �ڲ��첽ִ�з���
        /// </summary>
        /// <param name="arg">�̲߳���</param>
        protected virtual void Execute(Object arg)
        {
            Execute();
        }

        #endregion

        #region IDispose��Ա

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}