using KJFramework.Messages.Contracts;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction.Processors
{
    /// <summary>
    ///     网络消息事务处理器
    /// </summary>
    public abstract class MessageTransactionProcessor : IMessageTransactionProcessor<MetadataMessageTransaction, MetadataContainer>
    {
        #region Members.

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (MessageTransactionProcessor));

        #endregion

        #region Methods.

        /// <summary>
        ///     网络事务处理函数
        /// </summary>
        /// <param name="transaction">网络事务</param>
        public void Process(MetadataMessageTransaction transaction)
        {
            try { InnerProcess(transaction); }
            catch (System.Exception ex)
            {
                _tracing.Error(ex);
                //sends a default RSP-MESSGAE when there had an unexpected exception;
                MessageIdentity identity = transaction.Request.GetAttributeAsType<MessageIdentity>(0x00);
                identity.DetailsId++;
                MetadataContainer rspMsg = new MetadataContainer();
                rspMsg.SetAttribute(0x00, new MessageIdentityValueStored(identity));
                transaction.SendResponse(rspMsg);
            }
        }

        /// <summary>
        ///     网络事务处理函数
        /// </summary>
        /// <param name="transaction">网络事务</param>
        protected abstract void InnerProcess(MetadataMessageTransaction transaction);

        #endregion
    }
}