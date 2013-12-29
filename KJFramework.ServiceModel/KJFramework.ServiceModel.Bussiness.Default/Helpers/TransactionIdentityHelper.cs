using System.Net;
using System.Threading;
using KJFramework.Net.Channels;
using KJFramework.ServiceModel.Bussiness.Default.Messages;
using KJFramework.ServiceModel.Identity;

namespace KJFramework.ServiceModel.Bussiness.Default.Helpers
{
    internal static class TransactionIdentityHelper
    {
        #region Members

        private static int _msgId;

        #endregion

        #region Methods

        /// <summary>
        ///     创建一个新事务标识
        /// </summary>
        /// <param name="channel">通信信道</param>
        /// <param name="isOneway">单向通信标识</param>
        /// <param name="isRequest">请求标识</param>
        /// <returns>返回创建后的新事务标识</returns>
        public static TransactionIdentity Create(IMessageTransportChannel<Message> channel, bool isOneway, bool isRequest)
        {
            return new TransactionIdentity
                       {
                           Iep = (IPEndPoint) channel.LocalEndPoint,
                           IsOneway = isOneway,
                           IsRequest = isRequest,
                           MessageId = Interlocked.Increment(ref _msgId)
                       };
        }

        #endregion
    }
}