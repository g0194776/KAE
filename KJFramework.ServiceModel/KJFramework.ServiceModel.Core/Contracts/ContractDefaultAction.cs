using System;
using System.Net;
using System.Threading;
using KJFramework.Net.Transaction.Identities;
using KJFramework.ServiceModel.Core.EventArgs;

namespace KJFramework.ServiceModel.Core.Contracts
{
    /// <summary>
    ///     ��ԼЭ��Ĭ�϶������ṩ����صĻ�������
    /// </summary>
    public class ContractDefaultAction : IContractDefaultAction
    {
        #region Members

        protected IRequestManager _manager;
        private static int _msgId;

        #endregion

        #region Implementation of IContractDefaultAction

        /// <summary>
        ///     ��ȡ�����ñ����ս���ַ
        /// </summary>
        public IPEndPoint LocalEndPoint { get; set; }

        /// <summary>
        ///     ��ȡ���������������
        /// </summary>
        public IRequestManager Manager
        {
            get { return _manager; }
            set { _manager = value; }
        }

        /// <summary>
        ///     ����һ���µ������ʶ
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
        ///     ������Լ�ӿ��¼�
        /// </summary>
        public event EventHandler<ClientLowProxyRequestEventArgs> Calling;
        protected void CallingHandler(ClientLowProxyRequestEventArgs e)
        {
            EventHandler<ClientLowProxyRequestEventArgs> handler = Calling;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     ������Լ�ӿ���ɺ��¼�
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