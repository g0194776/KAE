using System;
using System.Collections.Generic;
using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Identities;
using KJFramework.ServiceModel.Bussiness.Default.Helpers;
using KJFramework.ServiceModel.Bussiness.Default.Messages;
using KJFramework.Tracing;

namespace KJFramework.ServiceModel.Bussiness.Default.Transactions
{
    /// <summary>
    ///     RPC事务管理器
    /// </summary>
    internal class RPCTransactionManager : TransactionManager<RPCTransaction>
    {
        #region Constructor

        /// <summary>
        ///     消息事务管理器，提供了相关的基本操作
        ///     * 默认时间：60s.
        /// </summary>
        public RPCTransactionManager()
            : base(60000, new TransactionIdentityComparer())
        {

        }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (RPCTransactionManager));

        #endregion

        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <param name="channel">通信信道</param>
        /// <returns>返回创建后的新事务</returns>
        public RPCTransaction CreateTransaction(IMessageTransportChannel<Message> channel)
        {
            return CreateTransaction(TransactionIdentityHelper.Create(channel, false, true), channel);
        }

        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <param name="identity">事务唯一标识 </param>
        /// <param name="channel">通信信道</param>
        /// <returns>返回创建后的新事务</returns>
        public RPCTransaction CreateTransaction(TransactionIdentity identity, IMessageTransportChannel<Message> channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            if (identity == null) throw new ArgumentNullException("identity");
            if (!channel.IsConnected) throw new ArgumentException("Illegal channel status, IsConnected = false");
            RPCTransaction transaction = new RPCTransaction(identity, channel);
            if (!Add(transaction.Identity, transaction))
            {
                _tracing.Error("Cannot create transaction from target channel, because the transaction identity has been existed. #tid: " + transaction.Identity);
                return null;
            }
            return transaction;
        }

        /// <summary>
        ///     创建一个新的单向通信事务
        /// </summary>
        /// <param name="channel">通信信道</param>
        /// <returns>返回创建后的新事务</returns>
        public RPCTransaction CreateOnewayTransaction(IMessageTransportChannel<Message> channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            if (!channel.IsConnected) throw new ArgumentException("Illegal channel status, IsConnected = false");
            RPCTransaction transaction = new RPCTransaction(TransactionIdentityHelper.Create(channel, true, true), channel);
            if (!Add(transaction.Identity, transaction))
            {
                _tracing.Error("Cannot create transaction from target channel, because the transaction identity has been existed. #tid: " + transaction.Identity);
                return null;
            }
            return transaction;
        }

        #region Events

        protected override void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_transactions.Count == 0) return;
            IList<TransactionIdentity> expireValues = new List<TransactionIdentity>();
            //check dead flag for transaction.
            foreach (KeyValuePair<TransactionIdentity, RPCTransaction> pair in _transactions)
                if (pair.Value.GetLease().IsDead) expireValues.Add(pair.Key);
            if (expireValues.Count == 0) return;
            //remove expired transactions.
            foreach (TransactionIdentity expireValue in expireValues)
            {
                RPCTransaction transaction;
                if (_transactions.TryRemove(expireValue, out transaction))
                {
                    transaction.SetTimeout();
                    TransactionExpiredHandler(new LightSingleArgEventArgs<RPCTransaction>(transaction));
                }
            }
        }

        #endregion
    }
}