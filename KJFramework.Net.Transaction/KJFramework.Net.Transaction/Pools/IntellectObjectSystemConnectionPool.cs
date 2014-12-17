using System.Net;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Net.Transaction.Pools
{
    /// <summary>
    ///     系统连接池，仅供系统内部使用
    ///     <para>*key = xxxxxxx(IP):xxxxx(Port)</para>
    ///     <para>*demo = 127.0.0.1:8588</para>
    /// </summary>
    public class IntellectObjectSystemConnectionPool : ConnectionPool<BaseMessage>
    {
        #region Methods

        /// <summary>
        ///     获取具有指定标示的连接代理器，如果具有该条件的代理器不存在，则会创建一个新的代理器
        /// </summary>
        /// <param name="key">连接标示</param>
        /// <param name="roleId">服务角色编号</param>
        /// <param name="protocolStack">连接所承载的协议栈</param>
        /// <param name="transactionManager">事务管理器</param>
        /// <returns>如果返回null, 则表示当前无法连接到目标远程终结点地址</returns>
        public IServerConnectionAgent<BaseMessage> GetChannel(string key, string roleId, IProtocolStack protocolStack, MessageTransactionManager transactionManager)
        {
            string fullKey = string.Format("{0}#{1}", roleId, key);
            return base.GetChannel(key, fullKey, protocolStack, transactionManager);
        }

        /// <summary>
        ///    创建一个新的服务器端连接代理器，并将其注册到当前的连接池中
        /// </summary>
        /// <param name="iep">要创建连接的远程终结点地址</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="transactionManager">网络事务管理器</param>
        /// <returns>返回已经创建好的服务器端连接代理器</returns>
        protected override IServerConnectionAgent<BaseMessage> CreateAgent(IPEndPoint iep, IProtocolStack protocolStack, object transactionManager)
        {
            return IntellectObjectConnectionAgent.Create(iep, protocolStack, (MessageTransactionManager)transactionManager);
        }

        #endregion
    }
}