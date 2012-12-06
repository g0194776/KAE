using KJFramework.ServiceModel.Bussiness.Default.Transactions;

namespace KJFramework.ServiceModel.Bussiness.Default
{
    internal class Global
    {
        /// <summary>
        ///     全局唯一事务管理器
        /// </summary>
        public static readonly RPCTransactionManager TransactionManager = new RPCTransactionManager();
    }
}