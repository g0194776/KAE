using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using KJFramework.EventArgs;
using KJFramework.Net.Channels.Identities;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     事务管理器，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="V">事务类型</typeparam>
    public class TransactionManager<V> : ITransactionManager<V> 
        where V : ITransaction
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
            _transactions = comparer == null ? new ConcurrentDictionary<TransactionIdentity, V>() : new ConcurrentDictionary<TransactionIdentity, V>(comparer); 
            _timer = new System.Timers.Timer { Interval = _interval };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        #endregion

        #region Members

        protected readonly System.Timers.Timer _timer;
        protected readonly ConcurrentDictionary<TransactionIdentity, V> _transactions;

        #endregion

        #region Methods

        #endregion

        #region Implementation of ITransactionManager<K,V>

        protected readonly int _interval;

        /// <summary>
        ///     获取或设置事务检查的时间间隔
        ///     <para>* 单位: 毫秒</para>
        /// </summary>
        public virtual int Interval
        {
            get { return _interval; }
        }

        /// <summary>
        ///     管理一个事务
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <param name="transaction">事务</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        /// <returns>返回添加操作的状态</returns>
        public virtual bool Add(TransactionIdentity key, V transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            return GetTransaction(key) != null ? false : _transactions.TryAdd(key, transaction);
        }

        /// <summary>
        ///     获取一个正在管理中的事务
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <returns>事务</returns>
        public virtual V GetTransaction(TransactionIdentity key)
        {
            V transaction;
            if (_transactions.TryGetValue(key, out transaction))
            {
                return transaction;
            }
            return default(V);
        }

        /// <summary>
        ///     移除一个不需要管理的事务
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        public virtual void Remove(TransactionIdentity key)
        {
            V transaction;
            _transactions.TryRemove(key, out transaction);
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
            V transaction = GetTransaction(key);
            if (transaction == null) return DateTime.MinValue;
            return transaction.GetLease().Renew(timeSpan);
        }

        /// <summary>
        ///     尝试获取一个具有指定唯一标示的事务，并且在获取该事务后进行移除操作
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <returns>返回获取到的事务</returns>
        public virtual V GetRemove(TransactionIdentity key)
        {
            V transaction;
            return _transactions.TryRemove(key, out transaction) ? transaction : default(V);
        }

        /// <summary>
        ///     事务过期事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<V>> TransactionExpired;
        protected void TransactionExpiredHandler(LightSingleArgEventArgs<V> e)
        {
            EventHandler<LightSingleArgEventArgs<V>> handler = TransactionExpired;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Events

        //check time.
        protected virtual void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_transactions.Count == 0) return;
            IList<TransactionIdentity> expireValues = new List<TransactionIdentity>();
            //check dead flag for transaction.
            foreach (KeyValuePair<TransactionIdentity, V> pair in _transactions)
                if (pair.Value.GetLease().IsDead) expireValues.Add(pair.Key);
            if (expireValues.Count == 0) return;
            //remove expired transactions.
            foreach (TransactionIdentity expireValue in expireValues)
            {
                V transaction;
                if (_transactions.TryRemove(expireValue, out transaction))
                    TransactionExpiredHandler(new LightSingleArgEventArgs<V>(transaction));
            }
        }

        #endregion
    }
}