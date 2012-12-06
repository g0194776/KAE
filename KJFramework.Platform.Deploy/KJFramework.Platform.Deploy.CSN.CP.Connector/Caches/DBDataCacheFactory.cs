using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using KJFramework.Datas;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Caches
{
    /// <summary>
    ///     ���ݿ����ݻ�����󹤳�
    /// </summary>
    public class DBDataCacheFactory : IDataCacheFactory<DataTable>
    {
        #region Members

        private static Dictionary<string, Database> _databases = new Dictionary<string, Database>();
        private object _lockObj = new object();

        #endregion

        #region Implementation of IDataCacheFactory<DataTable>

        /// <summary>
        ///     ����һ���������
        /// </summary>
        /// <param name="args">
        ///     ����������������
        ///     <para>* index 0: Database Name</para>
        ///     <para>* index 1: Table Name</para>
        /// </param>
        /// <returns>���ش����Ļ������</returns>
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
        ///     ��ȡһ�����ݿ����
        /// </summary>
        /// <param name="name">���ݿ�����</param>
        /// <returns>�������ݿ����</returns>
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
        ///     ע��һ�����ݿ����
        /// </summary>
        /// <param name="name">���ݿ�����</param>
        /// <param name="database">���ݿ����</param>
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