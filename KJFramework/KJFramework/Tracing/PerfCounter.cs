using System.Diagnostics;
using KJFramework.PerformanceProvider;

namespace KJFramework.Tracing
{
    [DebuggerStepThrough]
    [PerfCategory("KJFramework:TracingManager:Tracing")]
    internal sealed class TracingPerfCounter
    {
        private static TracingPerfCounter _default = PerfCounterFactory.GetCounters<TracingPerfCounter>();
        public static TracingPerfCounter Default
        {
            get { return _default; }
        }

        public static void Initialize()
        {
            // trigger .cctor
            _default.ToString();
        }

        [PerfCounter("Cache Size.", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter SizeOfCache = null;

        [PerfCounter("Drop Total.", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter NumberOfDrops = null;
        [PerfCounter("Drop /Second.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfDrops = null;

        [PerfCounter("Commit Total.", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter NumberOfCommit = null;
        [PerfCounter("Commit /Second.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfCommit = null;
    }
}
