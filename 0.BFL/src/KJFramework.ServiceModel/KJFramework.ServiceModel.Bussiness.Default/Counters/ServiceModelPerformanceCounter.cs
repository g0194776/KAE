using System.Diagnostics;
using KJFramework.PerformanceProvider;

namespace KJFramework.ServiceModel.Bussiness.Default.Counters
{
    /// <summary>
    ///   内部计数器
    /// </summary>
    [PerfCategory("KJFramework.ServiceModel", PerformanceCounterCategoryType.MultiInstance)]
    internal class ServiceModelPerformanceCounter
    {
        #region Members

        public static readonly ServiceModelPerformanceCounter Instance = PerfCounterFactory.GetCounters<ServiceModelPerformanceCounter>();

        /// <summary>
        ///   每秒请求数
        /// </summary>
        [PerfCounter("Request count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfRequest;
        /// <summary>
        ///   每秒应答数
        /// </summary>
        [PerfCounter("Response count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfResponse;
        /// <summary>
        ///   每秒执行成功数
        /// </summary>
        [PerfCounter("Execute successed /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfExecuteSuccessed;
        /// <summary>
        ///   每秒执行失败数
        /// </summary>
        [PerfCounter("Execute failed /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfExecuteFailed;
        /// <summary>
        ///   每秒执行回调数
        /// </summary>
        [PerfCounter("Callback /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfCallback;

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