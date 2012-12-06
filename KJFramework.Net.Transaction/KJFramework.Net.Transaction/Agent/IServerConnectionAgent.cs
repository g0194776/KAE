using System;
using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Net.Transaction.Agent
{
    /// <summary>
    ///     服务器端代理器元接口
    /// </summary>
    public interface IServerConnectionAgent : IConnectionAgent
    {
        /// <summary>
        ///     获取消息事务管理器
        /// </summary>
        MessageTransactionManager TransactionManager { get; }
        /// <summary>
        ///     获取内部的通信信道
        /// </summary>
        /// <returns></returns>
        IMessageTransportChannel<BaseMessage> GetChannel();
        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <returns>返回新创建的事务</returns>
        BusinessMessageTransaction CreateTransaction();
        /// <summary>
        ///     创建一个新的单向事务
        /// </summary>
        /// <returns>返回新创建的单向事务</returns>
        BusinessMessageTransaction CreateOnewayTransaction();
        /// <summary>
        ///     新的事物创建被创建时激活此事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<IMessageTransaction<BaseMessage>>> NewTransaction;
    }
}