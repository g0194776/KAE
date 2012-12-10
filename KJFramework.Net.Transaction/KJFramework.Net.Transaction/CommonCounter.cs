using System.Diagnostics;
using KJFramework.PerformanceProvider;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///   内部计数器
    /// </summary>
    [PerfCategory("KJFramework.Infrastructure", PerformanceCounterCategoryType.MultiInstance)]
    public class CommonCounter
    {
        #region Members

        public static readonly CommonCounter Instance = PerfCounterFactory.GetCounters<CommonCounter>();

        /// <summary>
        ///   客户端每秒请求数
        /// </summary>
        [PerfCounter("Client request count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfClientRequest;
        /// <summary>
        ///   客户端每秒应答数
        /// </summary>
        [PerfCounter("Client response count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfClientResponse;
        /// <summary>
        ///   服务器端每秒请求数
        /// </summary>
        [PerfCounter("Server request count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfServerRequest;
        /// <summary>
        ///   服务器端每秒应答数
        /// </summary>
        [PerfCounter("Server response count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfServerResponse;
        /// <summary>
        ///   内部服务通信信道总数
        /// </summary>
        [PerfCounter("Total inner service channel count.", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter TotalOfServiceChannel;

        #endregion

        #region Methods

        /// <summary>
        ///     Active the counters of performance.
        /// </summary>
        public void Initialize()
        {
            /*nothing to do.*/
        }

        #endregion
    }
}