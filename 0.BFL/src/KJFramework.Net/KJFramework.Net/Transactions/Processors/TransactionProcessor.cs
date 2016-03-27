using System;
using System.Collections.Generic;
using KJFramework.Enums;
using KJFramework.Statistics;

namespace KJFramework.Net.Transactions.Processors
{
    /// <summary>
    ///     ���ﴦ���������࣬�ṩ����صĲ�����
    /// </summary>
    /// <typeparam name="TIn">��������</typeparam>
    /// <typeparam name="TOut">�������</typeparam>
    public abstract class TransactionProcessor<TIn, TOut> : ITransactionProcessor<TIn, TOut>
    {
        #region Implementation of IDisposable

        protected Dictionary<StatisticTypes,IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        /// <summary>
        /// ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of ITransactionProcessor<TIn,TOut>

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="obj">����</param>
        /// <returns>���ؽ��</returns>
        public abstract List<TOut> Process(TIn obj);

        #endregion
    }
}