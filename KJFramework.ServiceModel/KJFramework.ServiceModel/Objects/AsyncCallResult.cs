using KJFramework.ServiceModel.Identity;

namespace KJFramework.ServiceModel.Objects
{
    /// <summary>
    ///     异步调用结果，提供了相关的基本操作
    /// </summary>
    public class AsyncCallResult : IAsyncCallResult
    {
        #region Constructor

        /// <summary>
        ///     异步调用结果，提供了相关的基本操作
        /// </summary>
        /// <param name="isSuccess">调用结果</param>
        public AsyncCallResult(bool isSuccess)
            : this(isSuccess, false, null, null, null)
        {
        }

        /// <summary>
        ///     异步调用结果，提供了相关的基本操作
        /// </summary>
        /// <param name="isSuccess">调用结果</param>
        /// <param name="lastError">最后一个错误信息</param>
        public AsyncCallResult(bool isSuccess, System.Exception lastError)
            : this(isSuccess, false, null, lastError, null)
        {
        }

        /// <summary>
        ///     异步调用结果，提供了相关的基本操作
        /// </summary>
        /// <param name="isSuccess">调用结果</param>
        /// <param name="hasResult">返回值标示</param>
        /// <param name="identity">事务唯一标识</param>
        /// <param name="manager">请求管理器</param>
        public AsyncCallResult(bool isSuccess, bool hasResult, TransactionIdentity identity, IRequestManager manager)
            : this(isSuccess, hasResult, identity, null, manager)
        {
        }

        /// <summary>
        ///     异步调用结果，提供了相关的基本操作
        /// </summary>
        /// <param name="isSuccess">调用结果</param>
        /// <param name="hasResult">返回值标示</param>
        /// <param name="identity">事务唯一标识</param>
        /// <param name="lastError">最后一个错误信息</param>
        /// <param name="manager">请求管理器</param>
        public AsyncCallResult(bool isSuccess, bool hasResult, TransactionIdentity identity, System.Exception lastError, IRequestManager manager)
        {
            _isSuccess = isSuccess;
            _hasResult = hasResult;
            _identity = identity;
            _lastError = lastError;
            _manager = manager;
        }

        #endregion

        #region Implementation of IAsyncCallResult

        private readonly bool _isSuccess;
        private readonly bool _hasResult;
        private readonly System.Exception _lastError;
        private readonly IRequestManager _manager;
        private readonly TransactionIdentity _identity;

        /// <summary>
        ///     获取一个值，该值标示了当前的异步调用是否成功
        /// </summary>
        public bool IsSuccess
        {
            get { return _isSuccess; }
        }

        /// <summary>
        ///     获取一个值，该值标示了当前的异步调用是否包含返回值
        /// </summary>
        public bool HasResult
        {
            get { return _hasResult; }
        }

        /// <summary>
        ///     获取最后一个错误信息
        /// </summary>
        public System.Exception LastError
        {
            get { return _lastError; }
        }

        /// <summary>
        ///     获取返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <returns>返回值</returns>
        public T GetResult<T>()
        {
            return _manager.GetResult<T>(_identity, false);
        }

        #endregion
    }
}