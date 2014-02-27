using System.Collections.Concurrent;
using KJFramework.Messages.Helpers;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.ServiceModel.Metadata;

namespace KJFramework.ServiceModel.Core.Managers
{
    /// <summary>
    ///     请求管理器，提供了相关的基本操作
    /// </summary>
    public class RequestManager : IRequestManager
    {
        #region Members

        protected readonly ConcurrentDictionary<TransactionIdentity, IBinaryArgContext> _results = new ConcurrentDictionary<TransactionIdentity, IBinaryArgContext>(new TCPTransactionIdentityComparer());

        #endregion

        #region Implementation of IRequestManager

        /// <summary>
        ///     获取指定会话请求返回的结果
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="identity">事务唯一标识</param>
        /// <param name="isAsync">异步请求标示</param>
        /// <returns>返回值</returns>
        public T GetResult<T>(TransactionIdentity identity, bool isAsync)
        {
            if (isAsync) return default(T);
            IBinaryArgContext obj;
            if (!_results.TryRemove(identity, out obj)) return default(T);
            if (obj.HasException) throw obj.Exception;
            return (T)DataHelper.GetObject(typeof(T), obj.Data);
        }

        /// <summary>
        ///     根据一个会话编号添加一个返回结果
        /// </summary>
        /// <param name="identity">事务唯一标识</param>
        /// <param name="result">返回结果</param>
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