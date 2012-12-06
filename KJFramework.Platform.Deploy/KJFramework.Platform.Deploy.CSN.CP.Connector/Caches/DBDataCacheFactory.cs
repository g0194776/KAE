using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using KJFramework.Datas;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Caches
{
    /// <summary>
    ///     数据库数据缓存对象工厂
    /// </summary>
    public class DBDataCacheFactory : IDataCacheFactory<DataTable>
    {
        #region Members

        private static Dictionary<string, Database> _databases = new Dictionary<string, Database>();
        private object _lockObj = new object();

        #endregion

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
        public IDataCache<DataTable> Create(params object[] args)
        {
            string databaseName = (string) args[0];
            string tableName = (string) args[1];
            Database database = GetDatabase(databaseName);
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

        #region Methods

        /// <summary>
        ///     获取一个数据库对象
        /// </summary>
        /// <param name="name">数据库名称</param>
        /// <returns>返回数据库对象</returns>
        private Database GetDatabase(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            lock (_lockObj)
            {
                Database database;
                if (_databases.TryGetValue(name, out database))
                {
                    return database;
                }
                return null;
            }
        }

        /// <summary>
        ///     注册一个数据库对象
        /// </summary>
        /// <param name="name">数据库名称</param>
        /// <param name="database">数据库对象</param>
        public void RegistDatabase(string name, Database database)
        {
            if (string.IsNullOrEmpty(name) || database == null)
            {
                throw new ArgumentException("Invaild args.");
            }
            lock (_lockObj)
            {
                if (!_databases.ContainsKey(name))
                {
                    _databases.Add(name, database);
                }
            }
        }

        #endregion
    }
}