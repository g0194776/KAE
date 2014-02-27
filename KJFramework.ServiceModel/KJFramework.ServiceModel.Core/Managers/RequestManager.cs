using System.Collections.Concurrent;
using KJFramework.Messages.Helpers;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.ServiceModel.Metadata;

namespace KJFramework.ServiceModel.Core.Managers
{
    /// <summary>
    ///     ������������ṩ����صĻ�������
    /// </summary>
    public class RequestManager : IRequestManager
    {
        #region Members

        protected readonly ConcurrentDictionary<TransactionIdentity, IBinaryArgContext> _results = new ConcurrentDictionary<TransactionIdentity, IBinaryArgContext>(new TCPTransactionIdentityComparer());

        #endregion

        #region Implementation of IRequestManager

        /// <summary>
        ///     ��ȡָ���Ự���󷵻صĽ��
        /// </summary>
        /// <typeparam name="T">���ؽ��������</typeparam>
        /// <param name="identity">����Ψһ��ʶ</param>
        /// <param name="isAsync">�첽�����ʾ</param>
        /// <returns>����ֵ</returns>
        public T GetResult<T>(TransactionIdentity identity, bool isAsync)
        {
            if (isAsync) return default(T);
            IBinaryArgContext obj;
            if (!_results.TryRemove(identity, out obj)) return default(T);
            if (obj.HasException) throw obj.Exception;
            return (T)DataHelper.GetObject(typeof(T), obj.Data);
        }

        /// <summary>
        ///     ����һ���Ự������һ�����ؽ��
        /// </summary>
        /// <param name="identity">����Ψһ��ʶ</param>
        /// <param name="result">���ؽ��</param>
        public void AddResult(TransactionIdentity identity, IBinaryArgContext result)
        {
            _results.TryAdd(identity, result);
        }

        public void CheckException(TransactionIdentity identity)
        {
            IBinaryArgContext temp;
            if (_results.TryRemove(identity, out temp))
            {
                if (temp.HasException)
                {
                    throw temp.Exception;
                }
            }
        }

        #endregion
    }
}