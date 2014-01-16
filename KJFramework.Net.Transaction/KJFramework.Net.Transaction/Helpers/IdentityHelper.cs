using System.Net;
using System.Threading;
using KJFramework.Net.Transaction.Identities;

namespace KJFramework.Net.Transaction.Helpers
{
    /// <summary>
    ///     事务唯一标示帮助器
    /// </summary>
    public static class IdentityHelper
    {
        #region Members

        public static int _ids;

        #endregion

        #region Methods
        
        /// <summary>
        ///     创建一个事务唯一标示
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
        /// <returns>返回一个新的事务唯一标示</returns>
        public static TransactionIdentity Create(IPEndPoint iep)
        {
            return Create(iep, Interlocked.Increment(ref _ids), false);
        }

        /// <summary>
        ///     创建一个事务唯一标示
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
        /// <returns>返回一个新的事务唯一标示</returns>
        public static TransactionIdentity CreateOneway(IPEndPoint iep)
        {
            return Create(iep, Interlocked.Increment(ref _ids), true);
        }

        /// <summary>
        ///     创建一个事务唯一标示
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
        /// <param name="isOneway">单向请求标示</param>
        /// <returns>返回一个新的事务唯一标示</returns>
        public static TransactionIdentity Create(IPEndPoint iep, bool isOneway)
        {
            return new TransactionIdentity { EndPoint = iep, MessageId = Interlocked.Increment(ref _ids), IsOneway = isOneway, IsRequest = true };
        }

        /// <summary>
        ///     创建一个事务唯一标示
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
        /// <param name="messageId">消息编号</param>
        /// <param name="isOneway">单向请求标示</param>
        /// <returns>返回一个新的事务唯一标示</returns>
        public static TransactionIdentity Create(IPEndPoint iep, int messageId, bool isOneway)
        {
            return new TransactionIdentity {EndPoint = iep, MessageId = messageId, IsOneway = isOneway, IsRequest = true};
        }

        #endregion
    }
}