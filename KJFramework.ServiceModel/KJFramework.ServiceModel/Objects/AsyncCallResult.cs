using KJFramework.ServiceModel.Identity;

namespace KJFramework.ServiceModel.Objects
{
    /// <summary>
    ///     �첽���ý�����ṩ����صĻ�������
    /// </summary>
    public class AsyncCallResult : IAsyncCallResult
    {
        #region Constructor

        /// <summary>
        ///     �첽���ý�����ṩ����صĻ�������
        /// </summary>
        /// <param name="isSuccess">���ý��</param>
        public AsyncCallResult(bool isSuccess)
            : this(isSuccess, false, null, null, null)
        {
        }

        /// <summary>
        ///     �첽���ý�����ṩ����صĻ�������
        /// </summary>
        /// <param name="isSuccess">���ý��</param>
        /// <param name="lastError">���һ��������Ϣ</param>
        public AsyncCallResult(bool isSuccess, System.Exception lastError)
            : this(isSuccess, false, null, lastError, null)
        {
        }

        /// <summary>
        ///     �첽���ý�����ṩ����صĻ�������
        /// </summary>
        /// <param name="isSuccess">���ý��</param>
        /// <param name="hasResult">����ֵ��ʾ</param>
        /// <param name="identity">����Ψһ��ʶ</param>
        /// <param name="manager">���������</param>
        public AsyncCallResult(bool isSuccess, bool hasResult, TransactionIdentity identity, IRequestManager manager)
            : this(isSuccess, hasResult, identity, null, manager)
        {
        }

        /// <summary>
        ///     �첽���ý�����ṩ����صĻ�������
        /// </summary>
        /// <param name="isSuccess">���ý��</param>
        /// <param name="hasResult">����ֵ��ʾ</param>
        /// <param name="identity">����Ψһ��ʶ</param>
        /// <param name="lastError">���һ��������Ϣ</param>
        /// <param name="manager">���������</param>
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
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ���첽�����Ƿ�ɹ�
        /// </summary>
        public bool IsSuccess
        {
            get { return _isSuccess; }
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ���첽�����Ƿ��������ֵ
        /// </summary>
        public bool HasResult
        {
            get { return _hasResult; }
        }

        /// <summary>
        ///     ��ȡ���һ��������Ϣ
        /// </summary>
        public System.Exception LastError
        {
            get { return _lastError; }
        }

        /// <summary>
        ///     ��ȡ����ֵ
        /// </summary>
        /// <typeparam name="T">����ֵ����</typeparam>
        /// <returns>����ֵ</returns>
        public T GetResult<T>()
        {
            return _manager.GetResult<T>(_identity, false);
        }

        #endregion
    }
}