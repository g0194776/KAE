using System;
using KJFramework.Enums;
using KJFramework.EventArgs;

namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     ��Ϣ�ռ������࣬�ṩ����صĻ���������
    /// </summary>
    public class InfomationCollector : IInfomationCollector
    {
        #region ���캯��

        /// <summary>
        ///     ��Ϣ�ռ��������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="collectType">�ռ�����</param>
        /// <param name="reviewere">�����</param>
        protected InfomationCollector(Type collectType, IInfomationReviewer reviewere)
        {
            _collectType = collectType;
            _reviewer = reviewere;
            _createTime = DateTime.Now;
            _id = Guid.NewGuid();
        }

        #endregion

        #region ��������

        ~InfomationCollector()
        {
            Dispose();
        }

        #endregion

        #region ��Ա

        protected System.Timers.Timer _timer;

        #endregion

        #region Implementation of IControlable

        protected IInfomationReviewer _reviewer;
        protected Guid _id;
        protected bool _isActive;
        protected Type _collectType;
        protected DateTime _createTime;
        protected object _tag;
        protected InfomationCollectorTypes _infomationCollectorType;

        /// <summary>
        ///     ��ʼִ��
        /// </summary>
        public virtual void Start()
        {
            BeginWorkHandler(null);
        }
        /// <summary>
        ///     ִֹͣ��
        /// </summary>
        public virtual void Stop()
        {
            EndWorkHandler(null);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of ICollector

        /// <summary>
        ///     ��ȡΨһ��ʾ
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�ռ����Ƿ����
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
        }

        public event EventHandler BeginWork;
        protected void BeginWorkHandler(System.EventArgs e)
        {
            EventHandler work = BeginWork;
            if (work != null) work(this, e);
        }

        public event EventHandler EndWork;
        protected void EndWorkHandler(System.EventArgs e)
        {
            EventHandler work = EndWork;
            if (work != null) work(this, e);
        }

        #endregion

        #region Implementation of IInfomationCollector

        protected double _collectInterval = 5000;

        /// <summary>
        ///     �ռ�ʱ����
        ///            * ʱ�䵥λ�� ���롣
        /// </summary>
        public double CollectInterval
        {
            get { return _collectInterval; }
            set { _collectInterval = value; }
        }

        /// <summary>
        ///     ��ȡ�����ñ��ռ���Ϣ�Ķ�������
        /// </summary>
        public Type CollectType
        {
            get { return _collectType; }
        }

        /// <summary>
        ///     ��ȡ����ʱ��
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        /// <summary>
        ///     ��ȡ�����ø�������
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     ��ȡ��Ϣ�ռ�������ö��
        /// </summary>
        public InfomationCollectorTypes InfomationCollectorType
        {
            get { return _infomationCollectorType; }
        }

        /// <summary>
        ///     ��ȡ������
        /// </summary>
        /// <returns>����������</returns>
        public IInfomationReviewer GetReviewer()
        {
            return _reviewer;
        }

        /// <summary>
        ///     �����ռ�ʱ��
        /// </summary>
        /// <param name="interval">�ռ�ʱ��</param>
        public void ResetCollectInterval(double interval)
        {
            if (interval > 0)
            {
                _collectInterval = interval;
                IntervalTimeChangedHandler(null);
            }
        }

        /// <summary>
        ///     ����Ϣ�¼�
        /// </summary>
        public event EventHandler<NewInfomationEventArgs> NewInfomation;
        /// <summary>
        ///     �ռ�ʱ�䱻�����¼�
        /// </summary>
        public event EventHandler IntervalTimeChanged;
        protected void IntervalTimeChangedHandler(System.EventArgs e)
        {
            EventHandler changed = IntervalTimeChanged;
            if (changed != null) changed(this, e);
        }
        protected void NewInfomationHandler(NewInfomationEventArgs e)
        {
            EventHandler<NewInfomationEventArgs> infomation = NewInfomation;
            if (infomation != null) infomation(this, e);
        }

        #endregion

        #region ���෽��

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return String.Format("Infomation Collector - {0}.", _collectType.FullName);
        }

        #endregion
    }
}