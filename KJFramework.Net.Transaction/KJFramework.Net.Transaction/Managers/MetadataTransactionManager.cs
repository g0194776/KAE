using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.Net.Transaction.Managers
{
    /// <summary>
    ///     消息事务管理器，提供了相关的基本操作
    /// </summary>
    public class MetadataTransactionManager : TransactionManager<MetadataMessageTransaction>
    {
        #region Constructor

        /// <summary>
        ///     消息事务管理器，提供了相关的基本操作
        ///     * 默认时间：从配置文件中读取.
        /// </summary>
        /// <param name="comparer">比较器</param>
        public MetadataTransactionManager(IEqualityComparer<TransactionIdentity> comparer)
            : this(comparer, Global.TransactionCheckInterval)
        {
        }

        /// <summary>
        ///     消息事务管理器，提供了相关的基本操作
        ///     * 默认时间：30s.
        /// </summary>
        /// <param name="interval">事务检查时间间隔</param>
        /// <param name="comparer">比较器</param>
        public MetadataTransactionManager(IEqualityComparer<TransactionIdentity> comparer, int interval = 30000)
            : base(interval, comparer)
        {
        }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(MetadataTransactionManager));

        #endregion

        #region Methods

        /// <summary>
        ///     创建一个新的消息事务，并将其加入到当前的事务列表中
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="channel">消息通信信道</param>
        /// <returns>返回一个新的消息事务</returns>
        /// <exception cref="ArgumentNullException">通信信道不能为空</exception>
        public MetadataMessageTransaction Create(TransactionIdentity identity, IMessageTransportChannel<MetadataContainer> channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            MetadataMessageTransaction transaction = new MetadataMessageTransaction(new Lease(DateTime.MaxValue), channel) { TransactionManager = this, Identity = (TransactionIdentity)identity };
            return Add(identity, transaction) ? transaction : null;
        }

        /// <summary>
        ///     激活一个事务，并尝试处理该事务的响应消息流程
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="response">响应消息</param>
        public void Active(TransactionIdentity identity, MetadataContainer response)
        {
            MetadataMessageTransaction transaction;
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
        public override bool Add(TransactionIdentity key, MetadataMessageTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            MetadataMessageTransaction temp;
            if ((temp = GetTransaction(key)) != null)
            {
                _tracing.Error(
                    "#Cannot add MessageTransaction to current T-Manager, because the target identity has been dup. \r\nDetails below:\r\nIdentity: {0}\r\nCreate Time: {1}\r\nRequest: {2}\r\nResponse: {3}",
                    key, 
                    temp.CreateTime, 
                    (temp.Request == null ? "" : temp.Request.ToString()),
                    (temp.Response == null ? "" : temp.Response.ToString()));
                return false;
            }
            if (!_transactions.TryAdd(key, transaction))
            {
                _tracing.Error("#Add MessageTransaction to current T-Manager failed. #key: " + key);
                return false;
            }
            return true;
        } 

        #endregion

        #region Events

        protected override void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_transactions.Count == 0) return;
            IList<TransactionIdentity> expireValues = new List<TransactionIdentity>();
            //check dead flag for transaction.
            foreach (KeyValuePair<TransactionIdentity, MetadataMessageTransaction> pair in _transactions)
                if (pair.Value.GetLease().IsDead) expireValues.Add(pair.Key);
            if (expireValues.Count == 0) return;
            //remove expired transactions.
            foreach (TransactionIdentity expireValue in expireValues)
            {
                MetadataMessageTransaction transaction;
                if (_transactions.TryRemove(expireValue, out transaction))
                {
                    transaction.SetTimeout();
                    TransactionExpiredHandler(new LightSingleArgEventArgs<MetadataMessageTransaction>(transaction));
                }
            }
        }

        #endregion
    }
}