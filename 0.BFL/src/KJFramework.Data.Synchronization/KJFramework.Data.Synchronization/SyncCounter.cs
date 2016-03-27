using System.Diagnostics;
using KJFramework.PerformanceProvider;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///   内部计数器
    /// </summary>
    [PerfCategory("KJFramework.Data.Synchronization", PerformanceCounterCategoryType.MultiInstance)]
    internal class SyncCounter
    {
        #region Members

        public static readonly SyncCounter Instance = PerfCounterFactory.GetCounters<SyncCounter>();

        /// <summary>
        ///   每秒事务发送的请求数
        /// </summary>
        [PerfCounter("transaction send request count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfSendTranRequest;
        /// <summary>
        ///   每秒事务发送的应答数
        /// </summary>
        [PerfCounter("transaction send response count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfSendTranResponse;
        /// <summary>
        ///   每秒事务收到的请求数
        /// </summary>
        [PerfCounter("transaction recv request count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfRecvTranRequest;
        /// <summary>
        ///   每秒事务收到的应答数
        /// </summary>
        [PerfCounter("transaction recv response count /sec.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfRecvTranResponse;
        /// <summary>
        ///   一共在线用户数
        /// </summary>
        [PerfCounter("Total online user count", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter TotalOnlineUser;
        /// <summary>
        ///   用户连接数
        /// </summary>
        [PerfCounter("Total user connections", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter TotalUserConnections;

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