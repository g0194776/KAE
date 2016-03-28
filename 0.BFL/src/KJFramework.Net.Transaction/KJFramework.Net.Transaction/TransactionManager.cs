using System;
using System.Collections.Generic;
using System.Timers;
using KJFramework.EventArgs;
using KJFramework.Net.Identities;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     ������������ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="TMessage">��Ϣ���������ص���Ϣ����</typeparam>
    public class TransactionManager<TMessage> : ITransactionManager<TMessage> 
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
        ///     ��ȡ�������������ʱ����
        ///     <para>* ��λ: ����</para>
        /// </summary>
        public virtual int Interval
        {
            get { return _interval; }
        }

        #endregion

        #region Methods.

        /// <summary>
        ///     ����һ������
        /// </summary>
        /// <param name="key">����Ψһ��ֵ</param>
        /// <param name="transaction">����</param>
        /// <exception cref="T:System.ArgumentNullException">��������</exception>
        /// <returns>
        ///     ������Ӳ�����״̬
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
        ///     ��ȡһ�����ڹ����е�����
        /// </summary>
        /// <param name="key">����Ψһ��ֵ</param>
        /// <returns>����</returns>
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
        ///     �Ƴ�һ������Ҫ���������
        /// </summary>
        /// <param name="key">����Ψһ��ֵ</param>
        public virtual void Remove(TransactionIdentity key)
        {
            lock (_lockObj) _transactions.Remove(key);
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
            IMessageTransaction<TMessage> transaction = GetTransaction(key);
            if (transaction == null) return DateTime.MinValue;
            return transaction.GetLease().Renew(timeSpan);
        }

        /// <summary>
        ///     ���Ի�ȡһ������ָ��Ψһ��ʾ�����񣬲����ڻ�ȡ�����������Ƴ�����
        /// </summary>
        /// <param name="key">����Ψһ��ֵ</param>
        /// <returns>���ػ�ȡ��������</returns>
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
        ///     ����һ�����񣬲����Դ�����������Ӧ��Ϣ����
        /// </summary>
        /// <param name="identity">����Ψһ��ʾ</param>
        /// <param name="response">��Ӧ��Ϣ</param>
        public void Active(TransactionIdentity identity, TMessage response)
        {
            IMessageTransaction<TMessage> transaction = GetRemove(identity);
            if (transaction == null) return;
            transaction.SetResponse(response);
        } 

        /// <summary>
        ///     ��������¼�
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
        ///    ����ʱ���㺯��
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