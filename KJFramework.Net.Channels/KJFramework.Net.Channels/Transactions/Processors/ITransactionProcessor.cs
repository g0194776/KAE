using System;
using System.Collections.Generic;
using KJFramework.Statistics;

namespace KJFramework.Net.Channels.Transactions.Processors
{
    /// <summary>
    ///     事物处理器元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="TIn">输入类型</typeparam>
    /// <typeparam name="TOut">输出类型</typeparam>
    public interface ITransactionProcessor<TIn, TOut> : IDisposable, IStatisticable<IStatistic>
    {
        /// <summary>
        ///     处理
        /// </summary>
        /// <param name="obj">参数</param>
        /// <returns>返回结果</returns>
        List<TOut> Process(TIn obj);
    }
}
