using System.Diagnostics;
using KJFramework.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KJFramework.Test
{
    /// <summary>
    ///This is a test class for ConcurrentQueueEventProcessorTest and is intended
    ///to contain all ConcurrentQueueEventProcessorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConcurrentQueueEventProcessorTest
    {
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


        [TestMethod()]
        [DeploymentItem("KJFramework.dll")]
        public void PickupTest()
        {
            ConcurrentQueueEventProcessor<int> target = new ConcurrentQueueEventProcessor<int>(8);
            //add 100,000 test datas.
            for (int i = 0; i < 100000; i++)
            {
                target.Add(i);
            }
            target.Action = delegate(int obj) { Debug.Print(obj.ToString()); };
            target.Start();
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
