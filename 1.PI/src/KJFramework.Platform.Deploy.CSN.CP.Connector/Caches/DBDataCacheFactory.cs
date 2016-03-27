using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Datas;
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
        private Dictionary<string, string> _dtProcMapping = new Dictionary<string, string>();

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
            string databaseName = (string)args[0];
            string tableName = (string)args[1];
            Database database = GetDatabase(databaseName);
            if (database == null)
            {
                Logs.Logger.Log("Can not create a new data cache, because the dest database obj is null.");
                return null;
            }
            DbReader dataReader = null;
            DBDataCache cache = new DBDataCache();
            try
            {
                List<CmdParameter> list = new List<CmdParameter>();
                if (args.Length > 2 && args[2] != null)
                {
                    CmdParameter cmdParamter = new CmdParameter("p_ServiceName", (string)args[2]);
                    list.Add(cmdParamter);

                }
                string proc = _dtProcMapping.ContainsKey(tableName.ToLower()) ? _dtProcMapping[tableName.ToLower()] : null;
                if (string.IsNullOrEmpty(proc))
                {
                    proc = "UPS_ExcuteDynamicProc";
                    list.Clear();
                    list.Add(new CmdParameter("p_TableName", args[1]));
                }
                dataReader = database.ExecuteReader(proc, list.ToArray());

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

        public IDataCache<KeyValueItem[]> Create(string dbName, string table, string servicename)
        {
            Database database = GetDatabase(dbName);
            if (database == null)
            {
                Logs.Logger.Log("Can not create a new data cache, because the dest database obj is null.");
                return null;
            }
            DbReader dataReader = null;
            KeyDataCache cache = new KeyDataCache();
            try
            {
                List<CmdParameter> list = new List<CmdParameter>();
                CmdParameter cmdParamter = new CmdParameter("p_ServiceName", servicename);
                list.Add(cmdParamter);
                dataReader = database.ExecuteReader(_dtProcMapping[table.ToLower()], list.ToArray());
                List<KeyValueItem> kviList = new List<KeyValueItem>();
                KeyValueItem kvi;
                while (dataReader.Read())
                {
                    kvi = new KeyValueItem();
                    kvi.Key = dataReader["ConfigKey"].ToString();
                    kvi.Value = dataReader["ConfigValue"].ToString();
                    kvi.CreateTime = Convert.ToDateTime(dataReader["CreateTime"]);
                    kvi.LastOprTime = Convert.ToDateTime(dataReader["LastOprTime"]);
                    kviList.Add(kvi);
                }
                cache.Item = kviList.ToArray();
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

        public void Initialize()
        {
            _dtProcMapping["ha_configinfo"] = "USP_GetConfigInfo";
            _dtProcMapping["ha_serviceinfo"] = "USP_GetServiceInfo";
            _dtProcMapping["ha_serviceroutetable"] = "USP_GetServiceRouteTable";
        }

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