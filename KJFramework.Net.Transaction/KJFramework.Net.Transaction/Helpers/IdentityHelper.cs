using System;
using System.Net;
using System.Threading;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.Identities;

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
        /// <param name="channelType">通信信道类型</param>
        /// <returns>返回一个新的事务唯一标示</returns>
        /// <exception cref="NotSupportedException">不支持的通信信道类型</exception>
        public static TransactionIdentity Create(EndPoint iep, TransportChannelTypes channelType)
        {
            return Create(iep, (uint)Interlocked.Increment(ref _ids), false, channelType);
        }

        /// <summary>
        ///     创建一个事务唯一标示
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
        /// <param name="channelType">通信信道类型</param>
        /// <returns>返回一个新的事务唯一标示</returns>
        /// <exception cref="NotSupportedException">不支持的通信信道类型</exception>
        public static TransactionIdentity CreateOneway(EndPoint iep, TransportChannelTypes channelType)
        {
            return Create(iep, (uint)Interlocked.Increment(ref _ids), true, channelType);
        }

        /// <summary>
        ///     创建一个事务唯一标示
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
        /// <param name="isOneway">单向请求标示</param>
        /// <param name="channelType">通信信道类型</param>
        /// <returns>返回一个新的事务唯一标示</returns>
        /// <exception cref="NotSupportedException">不支持的通信信道类型</exception>
        public static TransactionIdentity Create(EndPoint iep, bool isOneway, TransportChannelTypes channelType)
        {
            return new TCPTransactionIdentity { EndPoint = iep, MessageId = (uint)Interlocked.Increment(ref _ids), IsOneway = isOneway, IsRequest = true };
        }

        /// <summary>
        ///     创建一个事务唯一标示
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
        /// <param name="messageId">消息编号</param>
        /// <param name="isOneway">单向请求标示</param>
        /// <param name="channelType">通信信道类型</param>
        /// <returns>返回一个新的事务唯一标示</returns>
        /// <exception cref="NotSupportedException">不支持的通信信道类型</exception>
        public static TransactionIdentity Create(EndPoint iep, uint messageId, bool isOneway, TransportChannelTypes channelType)
        {
            if (channelType == TransportChannelTypes.TCP) return new TCPTransactionIdentity { EndPoint = iep, MessageId = messageId, IsOneway = isOneway, IsRequest = true };
            if (channelType == TransportChannelTypes.NamedPipe) return new NamedPipeTransactionIdentity { EndPoint = iep, MessageId = messageId, IsOneway = isOneway, IsRequest = true };
            throw new NotSupportedException("#Current type of Channel cannot be supoorted. #" + channelType);
        }

        #endregion
    }
}