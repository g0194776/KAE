using System;
using System.Net;
using System.Threading;
using KJFramework.Net.Transaction.Identities;
using KJFramework.ServiceModel.Core.EventArgs;

namespace KJFramework.ServiceModel.Core.Contracts
{
    /// <summary>
    ///     契约协议默认动作，提供了相关的基本操作
    /// </summary>
    public class ContractDefaultAction : IContractDefaultAction
    {
        #region Members

        protected IRequestManager _manager;
        private static int _msgId;

        #endregion

        #region Implementation of IContractDefaultAction

        /// <summary>
        ///     获取或设置本地终结点地址
        /// </summary>
        public IPEndPoint LocalEndPoint { get; set; }

        /// <summary>
        ///     获取或设置请求管理器
        /// </summary>
        public IRequestManager Manager
        {
            get { return _manager; }
            set { _manager = value; }
        }

        /// <summary>
        ///     创建一个新的事务标识
        /// </summary>
        public TransactionIdentity Create(bool isOneway)
        {
            return new TransactionIdentity
                       {
                           EndPoint = LocalEndPoint,
                           IsOneway = isOneway,
                           IsRequest = true,
                           MessageId = Interlocked.Increment(ref _msgId)
                       };
        }

        /// <summary>
        ///     调用契约接口事件
        /// </summary>
        public event EventHandler<ClientLowProxyRequestEventArgs> Calling;
        protected void CallingHandler(ClientLowProxyRequestEventArgs e)
        {
            EventHandler<ClientLowProxyRequestEventArgs> handler = Calling;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     调用契约接口完成后事件
        /// </summary>
        public event EventHandler<AfterCallEventArgs> AfterCall;
        protected void AfterCallHandler(AfterCallEventArgs e)
        {
            EventHandler<AfterCallEventArgs> handler = AfterCall;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}