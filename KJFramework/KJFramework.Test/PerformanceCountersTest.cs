using System.Diagnostics;
using KJFramework.Attribute;
using KJFramework.Extend;
using KJFramework.PerformanceProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KJFramework.Test
{
    /// <summary>
    ///This is a test class for PerformanceCountersTest and is intended
    ///to contain all PerformanceCountersTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PerformanceCountersTest
    {
        #region Members

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Regist
        ///</summary>
        [TestMethod()]
        public void RegistTest()
        {
            TestClass testClass = new TestClass();
            PerformanceCounters.Regist(testClass.GetType());
            PerformanceCounter counter1 = testClass.GetPerformanceCounter(0);
            PerformanceCounter counter2 = testClass.GetPerformanceCounter(1);
            Assert.IsNotNull(counter1);
            Assert.IsNotNull(counter2);
        }


        /// <summary>
        ///A test for Regist
        ///</summary>
        [TestMethod()]
        public void GetCounterTest()
        {
            TestClass testClass = new TestClass();
            PerformanceCounters.Regist(testClass.GetType());
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                PerformanceCounter counter1 = testClass.GetPerformanceCounter(0);
                PerformanceCounter counter2 = testClass.GetPerformanceCounter(1);
            }
            stopwatch.Stop();
            Debug.Print("#Elapsed time: " + stopwatch.Elapsed);
            Debug.Print("#Gen 0: " + GC.CollectionCount(0));
            Debug.Print("#Gen 1: " + GC.CollectionCount(1));
            Debug.Print("#Gen 2: " + GC.CollectionCount(2));
        }

        /// <summary>
        ///A test for Regist
        ///</summary>
        [TestMethod()]
        public void RegistTestWithCustomer()
        {
            TestClass1 testClass = new TestClass1();
            PerformanceCounters.Regist(testClass.GetType());
            PerformanceCounter counter1 = testClass.GetPerformanceCounter(0);
            PerformanceCounter counter2 = testClass.GetPerformanceCounter(1);
            PerformanceCounter counter3 = testClass.GetPerformanceCounter(2);
            Assert.IsNotNull(counter1);
            Assert.IsNotNull(counter2);
        }

        /// <summary>
        ///A test for Regist
        ///</summary>
        [TestMethod()]
        public void GetCounterTestWithCustomer()
        {
            TestClass1 testClass = new TestClass1();
            PerformanceCounters.Regist(testClass.GetType());
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                PerformanceCounter counter1 = testClass.GetPerformanceCounter(0);
                PerformanceCounter counter2 = testClass.GetPerformanceCounter(1);
                PerformanceCounter counter3 = testClass.GetPerformanceCounter(2);
            }
            stopwatch.Stop();
            Debug.Print("#Elapsed time: " + stopwatch.Elapsed);
            Debug.Print("#Gen 0: " + GC.CollectionCount(0));
            Debug.Print("#Gen 1: " + GC.CollectionCount(1));
            Debug.Print("#Gen 2: " + GC.CollectionCount(2));
        }
    }

    [PerformanceCounter(0, "Processor", "% Processor Time", true)]
    [PerformanceCounter(1, "Memory", "% Committed Bytes In Use", true)]
    [PerformanceCounter(2, "AverageCounter64SampleCategory", "AverageCounter64Sample")]
    public class TestClass : IPerformanceCounterOwner
    {
        
    }


    [PerformanceCounter(0, "Processor", "% Processor Time", true)]
    [PerformanceCounter(1, "Memory", "% Committed Bytes In Use", true)]
    [PerformanceCounter(2, "AverageCounter64SampleCategory", PerformanceCounterType.NumberOfItems32)]
    public class TestClass1 : IPerformanceCounterOwner
    {

    }
}
