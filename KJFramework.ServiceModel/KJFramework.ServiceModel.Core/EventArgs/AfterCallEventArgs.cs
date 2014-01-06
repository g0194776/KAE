using KJFramework.Net.Transaction.Identities;

namespace KJFramework.ServiceModel.Core.EventArgs
{
    /// <summary>
    ///     调用契约后的事件参数
    /// </summary>
    public class AfterCallEventArgs : System.EventArgs
    {
        #region Constructor

        /// <summary>
        ///     调用契约后的事件参数
        /// </summary>
        /// <param name="identity">事务唯一标识</param>
        /// <param name="isAsync">异步请求标示</param>
        /// <param name="isOneway">单向请求标示</param>
        public AfterCallEventArgs(TransactionIdentity identity, bool isAsync, bool isOneway)
        {
            _identity = identity;
            _isAsync = isAsync;
            _isOneway = isOneway;
        }

        #endregion

        #region Members

        private readonly TransactionIdentity _identity;
        private readonly bool _isAsync;
        private readonly bool _isOneway;

        /// <summary>
        ///     获取事务唯一标识
        /// </summary>
        public TransactionIdentity Identity
        {
            get { return _identity; }
        }

        /// <summary>
        ///     获取异步请求标示
        /// </summary>
        public bool IsAsync
        {
            get { return _isAsync; }
        }

        /// <summary>
        ///     获取一个值，该值标示当前的请求是否是单向的
        /// </summary>
        public bool IsOneway
        {
            get { return _isOneway; }
        }

        #endregion
    }
}