using KJFramework.ApplicationEngine.Clusters;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///    基于强类型消息的事务代理器
    /// </summary>
    internal class BusinessMessageTransactionProxy : MessageTransactionProxy<BaseMessage>
    {
        #region Constructor.

        /// <summary>
        ///    网络消息事务代理器抽象父类
        /// </summary>
        /// <param name="container">网络协议栈容器</param>
        /// <param name="cluster">网络负载器</param>
        public BusinessMessageTransactionProxy(IProtocolStackContainer container, INetworkCluster<BaseMessage> cluster, ITransactionManager<BaseMessage> transactionManager)
            : base(container, cluster, transactionManager)
        {
        }

        #endregion

        #region Methods.

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="lease">网络事务的生命周期</param>
        /// <param name="channel">网络事务内部包含的通信信道</param>
        /// <returns>返回一个新的网络事务</returns>
        protected override MessageTransaction<BaseMessage> NewTransaction(Lease lease, IMessageTransportChannel<BaseMessage> channel)
        {
            return new BusinessMessageTransaction(lease, channel);
        }

        #endregion
    }
}