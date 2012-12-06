using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using KJFramework.PerformanceProvider;

namespace KJFramework.Datas
{
    [PerfCategory("WI:Database")]
    public class DbPerfCounter
    {
        private static DbPerfCounter _total = PerfCounterFactory.GetCounters<DbPerfCounter>();
        public static DbPerfCounter Total
        {
            get { return _total; }
        }

        private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private static Dictionary<string, DbPerfCounter> _inst = new Dictionary<string, DbPerfCounter>();
        public static DbPerfCounter GetInst(string proc)
        {
            _lock.EnterReadLock();
            try
            {
                DbPerfCounter inst;
                if (_inst.TryGetValue(proc, out inst))
                    return inst;
            }
            finally
            {
                _lock.ExitReadLock();
            }
            _lock.EnterWriteLock();
            try
            {
                DbPerfCounter inst;
                if (!_inst.TryGetValue(proc, out inst))
                    _inst[proc] = inst = PerfCounterFactory.GetCounters<DbPerfCounter>(proc);
                return inst;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        [PerfCounter("ConnectionSuccess /Second.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfConnectionSuccess = null;
        [PerfCounter("ConnectionFailure /Second.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfConnectionFailure = null;
        [PerfCounter("ConnectionAttempt Concurrent.", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter ConcurrentOfConnectionAttempt = null;

        [PerfCounter("CommandSuccess /Second.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfCommandSuccess = null;
        [PerfCounter("CommandFailure /Second.", PerformanceCounterType.RateOfCountsPerSecond32)]
        public PerfCounter RateOfCommandFailure = null;

        [PerfCounter("CommandExecution Concurrent.", PerformanceCounterType.NumberOfItems32)]
        public PerfCounter ConcurrentOfCommandExecution = null;
        [PerfCounter("CommandExecution AvgTime.", PerformanceCounterType.AverageCount64)]
        public PerfCounter AvgTimeOfCommandExecution = null;
    }

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