using KJFramework.Data.Synchronization.Transactions;
using KJFramework.Messages.Contracts;
using KJFramework.Net;

namespace KJFramework.Data.Synchronization.EventArgs
{
    /// <summary>
    ///     新事物创建事件
    /// </summary>
    internal class NewTransactionEventArgs : System.EventArgs
    {
        #region Constructor

        /// <summary>
        ///     新事物创建事件
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="channel">内部通信信道</param>
        public NewTransactionEventArgs(SyncDataTransaction transaction, IMessageTransportChannel<MetadataContainer> channel)
        {
            Transaction = transaction;
            Channel = channel;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置相关事务
        /// </summary>
        public SyncDataTransaction Transaction { get; private set; }
        /// <summary>
        ///     获取或设置内部通信信道
        /// </summary>
        public IMessageTransportChannel<MetadataContainer> Channel { get; private set; }

        #endregion
    }
}