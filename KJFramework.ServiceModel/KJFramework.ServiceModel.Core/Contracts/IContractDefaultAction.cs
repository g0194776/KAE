using System;
using System.Net;
using KJFramework.Net.Transaction.Identities;
using KJFramework.ServiceModel.Core.EventArgs;

namespace KJFramework.ServiceModel.Core.Contracts
{
    /// <summary>
    ///     契约协议默认动作元接口，提供了相关的基本操作
    /// </summary>
    public interface IContractDefaultAction
    {
        /// <summary>
        ///     获取或设置本地终结点地址
        /// </summary>
        IPEndPoint LocalEndPoint { get; set; }
        /// <summary>
        ///     获取或设置请求管理器
        /// </summary>
        IRequestManager Manager { get; set; }
        /// <summary>
        ///     创建一个新的事务标识
        /// </summary>
        TransactionIdentity Create(bool isOneway);
        /// <summary>
        ///     调用契约接口事件
        /// </summary>
        event EventHandler<ClientLowProxyRequestEventArgs> Calling;
        /// <summary>
        ///     调用契约接口完成后事件
        /// </summary>
        event EventHandler<AfterCallEventArgs> AfterCall; 
    }
}