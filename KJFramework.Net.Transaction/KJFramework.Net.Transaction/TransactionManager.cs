using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using KJFramework.EventArgs;
using KJFramework.Net.Channels.Identities;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     ������������ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="V">��������</typeparam>
    public class TransactionManager<V> : ITransactionManager<V> 
        where V : ITransaction
    {
        #region Constructor

        /// <summary>
        ///     ������������ṩ����صĻ�������
        /// </summary>
        /// <param name="interval">
        ///     ������ʱ����
        ///     <para>* Ĭ��ʱ��: 30s</para>
        /// </param>
        /// <param name="comparer">�Ƚ���</param>
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
        ///     ��ȡ�������������ʱ����
        ///     <para>* ��λ: ����</para>
        /// </summary>
        public virtual int Interval
        {
            get { return _interval; }
        }

        /// <summary>
        ///     ����һ������
        /// </summary>
        /// <param name="key">����Ψһ��ֵ</param>
        /// <param name="transaction">����</param>
        /// <exception cref="ArgumentNullException">��������</exception>
        /// <returns>������Ӳ�����״̬</returns>
        public virtual bool Add(TransactionIdentity key, V transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            return GetTransaction(key) != null ? false : _transactions.TryAdd(key, transaction);
        }

        /// <summary>
        ///     ��ȡһ�����ڹ����е�����
        /// </summary>
        /// <param name="key">����Ψһ��ֵ</param>
        /// <returns>����</returns>
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
        ///     �Ƴ�һ������Ҫ���������
        /// </summary>
        /// <param name="key">����Ψһ��ֵ</param>
        public virtual void Remove(TransactionIdentity key)
        {
            V transaction;
            _transactions.TryRemove(key, out transaction);
        }

        /// <summary>
        ///     Ϊһ�������е����������Լ����
        /// </summary>
        /// <param name="key">����Ψһ��ֵ</param>
        /// <param name="timeSpan">��Լʱ��</param>
        /// <returns>
        ///     ������Լ���ʱ��
        ///     <para>* �������ֵ = MIN(DateTime), ���ʾ��Լʧ��</para>
        /// </returns>
        public virtual DateTime Renew(TransactionIdentity key, TimeSpan timeSpan)
        {
            V transaction = GetTransaction(key);
            if (transaction == null) return DateTime.MinValue;
            return transaction.GetLease().Renew(timeSpan);
        }

        /// <summary>
        ///     ���Ի�ȡһ������ָ��Ψһ��ʾ�����񣬲����ڻ�ȡ�����������Ƴ�����
        /// </summary>
        /// <param name="key">����Ψһ��ֵ</param>
        /// <returns>���ػ�ȡ��������</returns>
        public virtual V GetRemove(TransactionIdentity key)
        {
            V transaction;
            return _transactions.TryRemove(key, out transaction) ? transaction : default(V);
        }

        /// <summary>
        ///     ��������¼�
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