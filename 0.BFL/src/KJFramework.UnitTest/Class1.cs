using System.Diagnostics;
using KJFramework.PerformanceProvider;
using KJFramework.Tracing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.UnitTest
{
    [TestClass]
    public class Class1
    {
        #region Members.

        [TestMethod]
        public void InitializePerformanceCountTest()
        {
            var counter1 = PerfCounterFactory.GetCounters<TracingPerfCounter>();
            var counter2 = PerfCounterFactory.GetCounters<TracingPerfCounter>("yangjie-instance");
            for (int i = 0; i < 5; i++)
            {
                counter1.NumberOfCommit.Increment();
            }
            for (int i = 0; i < 2; i++)
            {
                counter2.NumberOfCommit.Increment();
            }
            PerformanceCounterCategory category = new PerformanceCounterCategory("KJFramework:TracingManager:Tracing");
            PerformanceCounter[] performanceCounters = category.GetCounters("yangjie-instance");
            foreach (PerformanceCounter performanceCounter in performanceCounters)
            {
                performanceCounter.RemoveInstance();
            }
        }

        #endregion
    }
}
