using System;
using System.Collections.Generic;
using KJFramework.Enums;
using KJFramework.Statistics;

namespace KJFramework.Net.Transactions.Processors
{
    /// <summary>
    ///     事物处理器抽象父类，提供了相关的操作。
    /// </summary>
    /// <typeparam name="TIn">输入类型</typeparam>
    /// <typeparam name="TOut">输出类型</typeparam>
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
        /// 获取或设置统计器
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of ITransactionProcessor<TIn,TOut>

        /// <summary>
        ///     处理
        /// </summary>
        /// <param name="obj">参数</param>
        /// <returns>返回结果</returns>
        public abstract List<TOut> Process(TIn obj);

        #endregion
    }
}