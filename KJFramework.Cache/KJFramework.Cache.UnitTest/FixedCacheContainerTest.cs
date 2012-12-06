using System;
using System.Diagnostics;
using KJFramework.Cache.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KJFramework.Cache.Cores;

namespace KJFramework.Cache.UnitTest
{
    /// <summary>
    ///This is a test class for FixedCacheContainerTest and is intended
    ///to contain all FixedCacheContainerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FixedCacheContainerTest
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
        
        [TestMethod()]
        public void GivebackTest()
        {
            IFixedCacheContainer<TestClass> container = new FixedCacheContainer<TestClass>(100);
            IFixedCacheStub<TestClass> readonlyCacheStub = container.Rent();
            Assert.IsNotNull(readonlyCacheStub);
            container.Giveback(readonlyCacheStub);
        }

        [TestMethod()]
        public void ExceptionTest()
        {
            System.Exception exception = null;
            try
            {
                new FixedCacheContainer<TestClass>(-1);
            }
            catch (System.Exception ex)
            {
                exception = ex;
                Debug.WriteLine(ex.Message);
            }
            Assert.IsNotNull(exception);
        }

        [TestMethod()]
        public void RemainingTest()
        {
            Random random = new Random();
            IFixedCacheContainer<TestClass> container = new FixedCacheContainer<TestClass>(100);
            IFixedCacheStub<TestClass> fixedCacheStub;
            for (int i = 0; i < 5; i++)
            {
                fixedCacheStub = container.Rent();
                fixedCacheStub.Cache.Name = random.Next(1000000).ToString();
                container.Giveback(fixedCacheStub);
            }
            System.Exception exception = null;
            try
            {
                //giveback more!
                container.Giveback(new CacheStub<TestClass> { Fixed = true });
            }
            catch (System.Exception ex)
            {
                exception = ex;
                Debug.WriteLine(ex.Message);
            }
            Assert.IsNotNull(exception);
        }

        [TestMethod()]
        public void RentToNullTest()
        {
            Random random = new Random();
            IFixedCacheContainer<TestClass> container = new FixedCacheContainer<TestClass>(100);
            IFixedCacheStub<TestClass> fixedCacheStub;
            for (int i = 0; i < 100; i++)
            {
                fixedCacheStub = container.Rent();
                fixedCacheStub.Cache.Name = random.Next(1000000).ToString();
                //do not giveback!
            }
            fixedCacheStub = container.Rent();
            Assert.IsNull(fixedCacheStub);
        }
        
        [TestMethod()]
        public void RentTest()
        {
            IFixedCacheContainer<TestClass> container = new FixedCacheContainer<TestClass>(100);
            IFixedCacheStub<TestClass> readonlyCacheStub = container.Rent();
            Assert.IsNotNull(readonlyCacheStub);
        }

        [TestMethod()]
        public void PerformanceTestForRentAndGiveBack()
        {
            Random random = new Random();
            IFixedCacheContainer<TestClass> container = new FixedCacheContainer<TestClass>(1000000);
            IFixedCacheStub<TestClass> fixedCacheStub;
            for (int i = 0; i < 1000000; i++)
            {
                fixedCacheStub = container.Rent();
                fixedCacheStub.Cache.Name = random.Next(1000000).ToString();
                container.Giveback(fixedCacheStub);
            }
            Debug.WriteLine("Gen 0: " + GC.CollectionCount(0));
            Debug.WriteLine("Gen 1: " + GC.CollectionCount(1));
            Debug.WriteLine("Gen 2: " + GC.CollectionCount(2));
        }
    }
}
