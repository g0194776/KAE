using System;
using System.Diagnostics;
using KJFramework.PerformanceProvider;

namespace KJFramework.Platform.Deploy.CSN.Common.Datas
{
    [DebuggerStepThrough]
    [PerfCategory("WI:Database::TaskQueue")]
    public class DBTaskQueuePerfCounter
    {
        private DBTaskQueuePerfCounter() { }

        public static void Initialize()
        {
            // trigger .cctor
            _default.ToString();
            Console.WriteLine("DBTaskQueuePerfCounter initialized.");
        }

        private static DBTaskQueuePerfCounter _default = PerfCounterFactory.GetCounters<DBTaskQueuePerfCounter>();
        public static DBTaskQueuePerfCounter Default
        {
            get { return _default; }
        }

        [PerfCounter("Execute task /Second.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfExecuteTask = null;
        [PerfCounter("Task total.", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter NumberOfTotalTask = null;
    }
}