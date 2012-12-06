using System.Diagnostics;
using KJFramework.PerformanceProvider;

namespace ConsoleApplication1
{
    /// <summary>
    ///   内部计数器
    /// </summary>
    [PerfCategory("Test.Server", PerformanceCounterCategoryType.MultiInstance)]
    public class Counter
    {
        #region Members

        public static readonly Counter Instance = PerfCounterFactory.GetCounters<Counter>();

        /// <summary>
        ///   客户端每秒请求数
        /// </summary>
        [PerfCounter("Client request count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfReq;
        /// <summary>
        ///   客户端每秒应答数
        /// </summary>
        [PerfCounter("Client request total count", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter TotalReq;
        /// <summary>
        ///   客户端每秒请求数
        /// </summary>
        [PerfCounter("Client response count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfRsp;
        /// <summary>
        ///   客户端每秒应答数
        /// </summary>
        [PerfCounter("Client response total count", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter TotalRsp;
        /// <summary>
        ///   客户端每秒应答数
        /// </summary>
        [PerfCounter("Channel total count", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter ChannelCount;

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