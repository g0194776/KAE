using System;
using System.Collections.Generic;
using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction;
using KJFramework.Platform.Deploy.CSN.Common.Comparers;
using KJFramework.Platform.Deploy.CSN.Common.Objects;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.BNL.Transactions
{
    /// <summary>
    ///     CSN事务管理器，提供了相关的基本操作
    /// </summary>
    public class CSNTransactionManager : TransactionManager<CSNTransactionIdentity, CSNMessageTransaction>
    {
        #region Constructor

        /// <summary>
        ///     CSN事务管理器，提供了相关的基本操作
        ///     * 默认时间：30s.
        /// </summary>
        /// <param name="interval">事务检查时间间隔</param>
        public CSNTransactionManager(int interval = 30000)
            : base(interval, new CSNTransactionIdentityComparer())
        { }

        #endregion

        #region Methods

        /// <summary>
        ///     创建一个新的消息事务，并将其加入到当前的事务列表中
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="channel">消息通信信道</param>
        /// <returns>返回一个新的消息事务</returns>
        /// <exception cref="ArgumentNullException">通信信道不能为空</exception>
        public CSNMessageTransaction Create(CSNTransactionIdentity identity, IMessageTransportChannel<CSNMessage> channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            CSNMessageTransaction transaction = new CSNMessageTransaction(new Lease(DateTime.MaxValue), channel) { TransactionManager = this, Identity = identity };
            return Add(identity, transaction) ? transaction : null;
        }

        /// <summary>
        ///     激活一个事务，并尝试处理该事务的响应消息流程
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="response">响应消息</param>
        public void Active(CSNTransactionIdentity identity, CSNMessage response)
        {
            CSNMessageTransaction transaction;
            if (!_transactions.TryRemove(identity, out transaction)) return;
            transaction.SetResponse(response);
        }

        #endregion

        #region Events

        protected override void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_transactions.Count == 0) return;
            IList<CSNTransactionIdentity> expireValues = new List<CSNTransactionIdentity>();
            //check dead flag for transaction.
            foreach (KeyValuePair<CSNTransactionIdentity, CSNMessageTransaction> pair in _transactions)
                if (pair.Value.GetLease().IsDead) expireValues.Add(pair.Key);
            if (expireValues.Count == 0) return;
            //remove expired transactions.
            foreach (CSNTransactionIdentity expireValue in expireValues)
            {
                CSNMessageTransaction transaction;
                if (_transactions.TryRemove(expireValue, out transaction))
                {
                    transaction.SetTimeout();
                    TransactionExpiredHandler(new LightSingleArgEventArgs<CSNMessageTransaction>(transaction));
                }
            }
        }

        #endregion
    }
}