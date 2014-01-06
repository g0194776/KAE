using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.Net.Transaction.Managers
{
    /// <summary>
    ///     消息事务管理器，提供了相关的基本操作
    /// </summary>
    public class MessageTransactionManager : TransactionManager<BusinessMessageTransaction>
    {
        #region Constructor

        /// <summary>
        ///     消息事务管理器，提供了相关的基本操作
        ///     * 默认时间：从配置文件中读取.
        /// </summary>
        /// <param name="comparer">比较器</param>
        public MessageTransactionManager(IEqualityComparer<BasicIdentity> comparer)
            : this(comparer, Global.TransactionCheckInterval)
        {
        }

        /// <summary>
        ///     消息事务管理器，提供了相关的基本操作
        ///     * 默认时间：30s.
        /// </summary>
        /// <param name="interval">事务检查时间间隔</param>
        /// <param name="comparer">比较器</param>
        public MessageTransactionManager(IEqualityComparer<BasicIdentity> comparer, int interval = 30000)
            : base(interval, comparer)
        {
        }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (MessageTransactionManager));

        #endregion

        #region Methods

        /// <summary>
        ///     创建一个新的消息事务，并将其加入到当前的事务列表中
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="channel">消息通信信道</param>
        /// <returns>返回一个新的消息事务</returns>
        /// <exception cref="ArgumentNullException">通信信道不能为空</exception>
        public BusinessMessageTransaction Create(BasicIdentity identity, IMessageTransportChannel<BaseMessage> channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            BusinessMessageTransaction transaction = new BusinessMessageTransaction(new Lease(DateTime.MaxValue), channel) { TransactionManager = this, Identity = (TransactionIdentity)identity };
            return Add(identity, transaction) ? transaction : null;
        }

        /// <summary>
        ///     激活一个事务，并尝试处理该事务的响应消息流程
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="response">响应消息</param>
        public void Active(BasicIdentity identity, BaseMessage response)
        {
            BusinessMessageTransaction transaction;
            if (!_transactions.TryRemove(identity, out transaction)) return;
            transaction.SetResponse(response);
        }

        /// <summary>
        ///     管理一个事务
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <param name="transaction">事务</param>
        /// <exception cref="T:System.ArgumentNullException">参数错误</exception>
        /// <returns>
        ///     返回添加操作的状态
        /// </returns>
        public override bool Add(BasicIdentity key, BusinessMessageTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            BusinessMessageTransaction temp;
            if ((temp = GetTransaction(key)) != null)
            {
                _tracing.Error(
                    "#Cannot add WXMessageTransaction to current T-Manager, because the target identity has been dup. \r\nDetails below:\r\nIdentity: {0}\r\nCreate Time: {1}\r\nRequest: {2}\r\nResponse: {3}",
                    key, 
                    temp.CreateTime, 
                    (temp.Request == null ? "" : temp.Request.ToString()),
                    (temp.Response == null ? "" : temp.Response.ToString()));
                return false;
            }
            if (!_transactions.TryAdd(key, transaction))
            {
                _tracing.Error("#Add WXMessageTransaction to current T-Manager failed. #key: " + key);
                return false;
            }
            return true;
        } 

        #endregion

        #region Events

        protected override void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_transactions.Count == 0) return;
            IList<BasicIdentity> expireValues = new List<BasicIdentity>();
            //check dead flag for transaction.
            foreach (KeyValuePair<BasicIdentity, BusinessMessageTransaction> pair in _transactions)
                if (pair.Value.GetLease().IsDead) expireValues.Add(pair.Key);
            if (expireValues.Count == 0) return;
            //remove expired transactions.
            foreach (BasicIdentity expireValue in expireValues)
            {
                BusinessMessageTransaction transaction;
                if (_transactions.TryRemove(expireValue, out transaction))
                {
                    transaction.SetTimeout();
                    TransactionExpiredHandler(new LightSingleArgEventArgs<BusinessMessageTransaction>(transaction));
                }
            }
        }

        #endregion
    }
}