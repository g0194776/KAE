using System;
using System.Collections.Generic;
using System.Timers;
using KJFramework.EventArgs;
using KJFramework.Net.Identities;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     事务管理器，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="TMessage">消息事务所承载的消息类型</typeparam>
    public class TransactionManager<TMessage> : ITransactionManager<TMessage> 
    {
        #region Constructor

        /// <summary>
        ///     事务管理器，提供了相关的基本操作
        /// </summary>
        /// <param name="interval">
        ///     事务检查时间间隔
        ///     <para>* 默认时间: 30s</para>
        /// </param>
        /// <param name="comparer">比较器</param>
        public TransactionManager(int interval, IEqualityComparer<TransactionIdentity> comparer)
        {
            if (interval <= 0) throw new ArgumentException("Illegal check time interval!");
            _interval = interval;
            _transactions = comparer == null ? new Dictionary<TransactionIdentity, IMessageTransaction<TMessage>>() : new Dictionary<TransactionIdentity, IMessageTransaction<TMessage>>(comparer); 
            _timer = new System.Timers.Timer { Interval = _interval };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        #endregion

        #region Members

        protected readonly int _interval;
        protected readonly System.Timers.Timer _timer;
        protected readonly object _lockObj = new object();
        protected readonly Dictionary<TransactionIdentity, IMessageTransaction<TMessage>> _transactions;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(TransactionManager<TMessage>));

        /// <summary>
        ///     获取或设置事务检查的时间间隔
        ///     <para>* 单位: 毫秒</para>
        /// </summary>
        public virtual int Interval
        {
            get { return _interval; }
        }

        #endregion

        #region Methods.

        /// <summary>
        ///     管理一个事务
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <param name="transaction">事务</param>
        /// <exception cref="T:System.ArgumentNullException">参数错误</exception>
        /// <returns>
        ///     返回添加操作的状态
        /// </returns>
        public bool Add(TransactionIdentity key, IMessageTransaction<TMessage> transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            IMessageTransaction<TMessage> temp;
            if ((temp = GetTransaction(key)) != null)
            {
                _tracing.Error(
                    "#Cannot add MessageTransaction to current TransactionManager, because the target identity has been dup. \r\nDetails below:\r\nIdentity: {0}\r\nCreate Time: {1}\r\nRequest: {2}\r\nResponse: {3}",
                    key,
                    temp.CreateTime,
                    (temp.Request == null ? "" : temp.Request.ToString()),
                    (temp.Response == null ? "" : temp.Response.ToString()));
                return false;
            }
            lock (_lockObj) _transactions.Add(key, transaction);
            return true;
        }

        /// <summary>
        ///     获取一个正在管理中的事务
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <returns>事务</returns>
        public virtual IMessageTransaction<TMessage> GetTransaction(TransactionIdentity key)
        {
            lock (_lockObj)
            {
                IMessageTransaction<TMessage> transaction;
                if (_transactions.TryGetValue(key, out transaction)) return transaction;
                return default(IMessageTransaction<TMessage>);
            }
        }

        /// <summary>
        ///     移除一个不需要管理的事务
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        public virtual void Remove(TransactionIdentity key)
        {
            lock (_lockObj) _transactions.Remove(key);
        }

        /// <summary>
        ///     为一个管理中的事务进行续约操作
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <param name="timeSpan">续约时间</param>
        /// <returns>
        ///     返回续约后的时间
        ///     <para>* 如果返回值 = MIN(DateTime), 则表示续约失败</para>
        /// </returns>
        public virtual DateTime Renew(TransactionIdentity key, TimeSpan timeSpan)
        {
            IMessageTransaction<TMessage> transaction = GetTransaction(key);
            if (transaction == null) return DateTime.MinValue;
            return transaction.GetLease().Renew(timeSpan);
        }

        /// <summary>
        ///     尝试获取一个具有指定唯一标示的事务，并且在获取该事务后进行移除操作
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <returns>返回获取到的事务</returns>
        public virtual IMessageTransaction<TMessage> GetRemove(TransactionIdentity key)
        {
            lock (_lockObj)
            {
                IMessageTransaction<TMessage> transaction;
                if (!_transactions.TryGetValue(key, out transaction)) return null;
                _transactions.Remove(key);
                return transaction;
            }
        }        
        
        /// <summary>
        ///     激活一个事务，并尝试处理该事务的响应消息流程
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="response">响应消息</param>
        public void Active(TransactionIdentity identity, TMessage response)
        {
            IMessageTransaction<TMessage> transaction = GetRemove(identity);
            if (transaction == null) return;
            transaction.SetResponse(response);
        } 

        /// <summary>
        ///     事务过期事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<IMessageTransaction<TMessage>>> TransactionExpired;
        protected void TransactionExpiredHandler(LightSingleArgEventArgs<IMessageTransaction<TMessage>> e)
        {
            EventHandler<LightSingleArgEventArgs<IMessageTransaction<TMessage>>> handler = TransactionExpired;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Events

        /// <summary>
        ///    事务超时计算函数
        /// </summary>
        protected virtual void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_transactions.Count == 0) return;
            IList<TransactionIdentity> expireValues = new List<TransactionIdentity>();
            //check dead flag for transaction.
            foreach (KeyValuePair<TransactionIdentity, IMessageTransaction<TMessage>> pair in _transactions)
                if (pair.Value.GetLease().IsDead) expireValues.Add(pair.Key);
            if (expireValues.Count == 0) return;
            //remove expired transactions.
            foreach (TransactionIdentity expireValue in expireValues)
            {
                IMessageTransaction<TMessage> transaction = GetRemove(expireValue);
                if (transaction != null)
                {
                    ((MessageTransaction<TMessage>)transaction).SetTimeout();
                    TransactionExpiredHandler(new LightSingleArgEventArgs<IMessageTransaction<TMessage>>(transaction));
                }
            }
        }

        #endregion
    }
}