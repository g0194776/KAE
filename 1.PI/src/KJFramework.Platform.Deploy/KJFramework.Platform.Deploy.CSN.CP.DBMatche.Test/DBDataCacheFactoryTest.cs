using System;
using System.Diagnostics;
using KJFramework.Datas;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Caches;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.CP.DBMatche.Test
{
    /// <summary>
    ///This is a test class for DBDataCacheFactoryTest and is intended
    ///to contain all DBDataCacheFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DBDataCacheFactoryTest
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


        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest_MNAV_AmsConfig()
        {
            DBDataCacheFactory target = new DBDataCacheFactory(); // TODO: Initialize to an appropriate value
            Database mnavDB = new Database(@"server=192.168.110.210\Global01;database=MNAVDB;uid=sa;pwd=Password01!");
            mnavDB.Open();
            target.RegistDatabase("MNAVDB", mnavDB);
            Stopwatch stopwatch = Stopwatch.StartNew();
            IDataCache<DataTable> actual = target.Create("MNAVDB", "MNAV_AmsConfig");
            stopwatch.Stop();
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                actual.Item.Bind();
                watch.Stop();
            }
            catch (System.Exception ex)
            {

            }
            Assert.IsNull(actual);
            Debug.Print("Create cache elaps time: " + stopwatch.Elapsed);
            Debug.Print("Gen 0:"  + GC.CollectionCount(0));
            Debug.Print("Gen 1: " + GC.CollectionCount(1));
            Debug.Print("Gen 2: " + GC.CollectionCount(2));
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest_MNAV_GlobalConfig()
        {
            DBDataCacheFactory target = new DBDataCacheFactory(); // TODO: Initialize to an appropriate value
            Database mnavDB = new Database(@"server=192.168.110.210\Global01;database=MNAVDB;uid=sa;pwd=Password01!");
            mnavDB.Open();
            target.RegistDatabase("MNAVDB", mnavDB);
            for (int i = 0; i < 1000; i++)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                IDataCache<DataTable> actual = target.Create("MNAVDB", "MNAV_GlobalConfig");
                stopwatch.Stop();
                try
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    actual.Item.Bind();
                    watch.Stop();
                    Debug.Print("Bind data elaps time: " + watch.Elapsed);
                }
                catch (System.Exception ex)
                {

                }
                Debug.Print("Create cache elaps time: " + stopwatch.Elapsed);
            }
            Debug.Print("Gen 0: " + GC.CollectionCount(0));
            Debug.Print("Gen 1: " + GC.CollectionCount(1));
            Debug.Print("Gen 2: " + GC.CollectionCount(2));
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
