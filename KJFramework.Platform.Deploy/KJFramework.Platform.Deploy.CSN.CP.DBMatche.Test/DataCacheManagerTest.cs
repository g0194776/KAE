using System.Collections.Generic;
using System.Data.SqlClient;
using KJFramework.Datas;
using KJFramework.Logger;
using KJFramework.Messages.Engine;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Caches;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KJFramework.Platform.Deploy.CSN.CP.DBMatche.Test
{
    /// <summary>
    ///This is a test class for DataCacheManagerTest and is intended
    ///to contain all DataCacheManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataCacheManagerTest
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
        ///A test for GetCache
        ///</summary>
        [TestMethod()]
        public void GetCacheTestHelper()
        {
                //DataCacheManager<DataTable> dbCacheManager = new DataCacheManager<DataTable>();
                DBDataCacheFactory dbCacheFactory = new DBDataCacheFactory();
                Database database = new Database(@"server=192.168.110.210\Global01;database=MNAVDB;uid=sa;pwd=Password01!");
                database.Open();
                dbCacheFactory.RegistDatabase("MNAVDB", database);
                CSNGetDataTableResponseMessage csnGetDataTableResponseMessage = new CSNGetDataTableResponseMessage();
                csnGetDataTableResponseMessage.Header.SessionId = 1;
                csnGetDataTableResponseMessage.Header.ServiceKey = "SERVICE-KEY";
                csnGetDataTableResponseMessage.Header.ClientTag = "";
                csnGetDataTableResponseMessage.Tables = new DataTable[1];
                string key = string.Intern(string.Format("__{0}__{1}", "MNAVDB", "MNAV_AmsConfig"));
                //try to get a cache from cache manager.
                DBDataCache dataCache = (DBDataCache)Create(database, "MNAVDB", "MNAV_AmsConfig");
                dataCache.Key = key;
                csnGetDataTableResponseMessage.Tables[0] = dataCache.Item;
                csnGetDataTableResponseMessage.Bind();
                CSNGetDataTableResponseMessage newObj1 = IntellectObjectEngine.GetObject<CSNGetDataTableResponseMessage>(typeof(CSNGetDataTableResponseMessage), csnGetDataTableResponseMessage.Body);
                
        }

        #region Implementation of IDataCacheFactory<DataTable>

        /// <summary>
        ///     创建一个缓存对象
        /// </summary>
        /// <param name="args">
        ///     创建缓存对象的条件
        ///     <para>* index 0: Database Name</para>
        ///     <para>* index 1: Table Name</para>
        /// </param>
        /// <returns>返回创建的缓存对象</returns>
        public IDataCache<DataTable> Create(Database database, params object[] args)
        {
            string databaseName = (string)args[0];
            string tableName = (string)args[1];
            if (database == null)
            {
                Logs.Logger.Log("Can not create a new data cache, because the dest database obj is null.");
                return null;
            }
            SqlDataReader dataReader = null;
            DBDataCache cache = new DBDataCache();
            try
            {
                dataReader = database.ExecuteReader(tableName, null);
                if (!dataReader.HasRows)
                {
                    return cache;
                }
                DataTable dataTable = new DataTable();
                dataTable.Columns = new string[dataReader.FieldCount];
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    dataTable.Columns[i] = dataReader.GetName(i);
                }
                List<DataRow> rows = new List<DataRow>();
                while (dataReader.Read())
                {
                    DataRow dataRow = new DataRow();
                    dataRow.Columns = new DataColumn[dataReader.FieldCount];
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        DataColumn column = new DataColumn();
                        column.Value = dataReader.GetValue(i).ToString();
                        dataRow.Columns[i] = column;
                    }
                    rows.Add(dataRow);
                }
                dataTable.Rows = rows.ToArray();
                cache.Item = dataTable;
                return cache;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return cache;
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
            }
        }

        #endregion
    }
}
