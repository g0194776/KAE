namespace KJFramework.Net.Transaction.Processors
{
    /// <summary>
    ///     消息处理器元接口，提供了相关的基本操作
    /// </summary>
    public interface IMessageTransactionProcessor<in TMessageTransaction, TMessage>
        where TMessageTransaction : MessageTransaction<TMessage>
    {
        /// <summary>
        ///     处理一个事务
        /// </summary>
        /// <param name="transaction">消息事务</param>
        void Process(TMessageTransaction transaction);
    }
}