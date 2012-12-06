using KJFramework.Net.Transaction.Agent;

namespace KJFramework.Net.Transaction.Contexts
{
    /// <summary>
    ///     事务相关上下文基础结构
    /// </summary>
    public class BusinessTransactionContext
    {
        #region Members

        /// <summary>
        ///     获取或设置连接方的连接代理器
        /// </summary>
        public IServerConnectionAgent ClientAgent { get; set; }
        /// <summary>
        ///     获取或设置客户端事务
        /// </summary>
        public BusinessMessageTransaction ClientTransactionEx { get; set; }
        /// <summary>
        ///     获取或设置与本事务所关联的执行事务
        /// </summary>
        public BusinessMessageTransaction PreviousTransaction { get; set; }

        #endregion
    }
}