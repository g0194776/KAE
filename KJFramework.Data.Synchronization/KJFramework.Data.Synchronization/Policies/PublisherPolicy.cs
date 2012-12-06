using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Data.Synchronization.Policies
{
    /// <summary>
    ///     发布者策略
    /// </summary>
    public class PublisherPolicy : IntellectObject, IPublisherPolicy
    {
        #region Implementation of IPublisherPolicy

        protected bool _canRetry;
        protected bool _isOneway;
        protected byte _retryCount;
        protected int _timeoutSec;

        /// <summary>
        ///     获取或设置一个值，该值标示了当前的发布者是否可以对于失败的发布消息进行重试操作
        /// </summary>
        [IntellectProperty(0)]
        public bool CanRetry
        {
            get { return _canRetry; }
            set { _canRetry = value; }
        }

        /// <summary>
        ///     获取或设置一个值，该值表示了当前发布的所有消息是不是单向的
        /// </summary>
        [IntellectProperty(1)]
        public bool IsOneway
        {
            get { return _isOneway; }
            set { _isOneway = value; }
        }

        /// <summary>
        ///     获取或设置重试次数
        ///     <para>* 此字段仅当 CanRetry = true的时候有效</para>
        /// </summary>
        [IntellectProperty(2)]
        public byte RetryCount
        {
            get { return _retryCount; }
            set { _retryCount = value; }
        }

        /// <summary>
        ///     获取或设置同步数据时的超时时间
        ///     <para>* 单位: 秒</para>
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
        ///     默认的发布者策略
        /// </summary>
        public static readonly PublisherPolicy Default = new PublisherPolicy { CanRetry = false, IsOneway = false, TimeoutSec = (int)Global.TranTimeout.TotalSeconds };

        #endregion
    }
}