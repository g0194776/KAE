using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Data.Synchronization.Policies
{
    /// <summary>
    ///     �����߲���
    /// </summary>
    public class PublisherPolicy : IntellectObject, IPublisherPolicy
    {
        #region Implementation of IPublisherPolicy

        protected bool _canRetry;
        protected bool _isOneway;
        protected byte _retryCount;
        protected int _timeoutSec;

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�ķ������Ƿ���Զ���ʧ�ܵķ�����Ϣ�������Բ���
        /// </summary>
        [IntellectProperty(0)]
        public bool CanRetry
        {
            get { return _canRetry; }
            set { _canRetry = value; }
        }

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ������������Ϣ�ǲ��ǵ����
        /// </summary>
        [IntellectProperty(1)]
        public bool IsOneway
        {
            get { return _isOneway; }
            set { _isOneway = value; }
        }

        /// <summary>
        ///     ��ȡ���������Դ���
        ///     <para>* ���ֶν��� CanRetry = true��ʱ����Ч</para>
        /// </summary>
        [IntellectProperty(2)]
        public byte RetryCount
        {
            get { return _retryCount; }
            set { _retryCount = value; }
        }

        /// <summary>
        ///     ��ȡ������ͬ������ʱ�ĳ�ʱʱ��
        ///     <para>* ��λ: ��</para>
        /// </summary>
        [IntellectProperty(3)]
        public int TimeoutSec
        {
            get { return _timeoutSec; }
            set { _timeoutSec = value; }
        }

        #endregion

        #region Members

        /// <summary>
        ///     Ĭ�ϵķ����߲���
        /// </summary>
        public static readonly PublisherPolicy Default = new PublisherPolicy { CanRetry = false, IsOneway = false, TimeoutSec = (int)Global.TranTimeout.TotalSeconds };

        #endregion
    }
}