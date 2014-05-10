using System.Collections.Generic;
using System.Data;

namespace KJFramework.Datas
{
    public abstract class DatabaseOperation
    {
        public abstract int SpExecuteNonQuery(string spName, string[] paramters, params object[] values);
        public abstract T SpExecuteScalar<T>(string spName, string[] parameters, params object[] values);
        public abstract DataTable SpExecuteTable(string spName, string[] parameters, params object[] values);
        public abstract DataSet SpExecuteDataSet(string spName, string[] paramters, params object[] values);
        public abstract string FormatSql(string spName, string[] parameters, params object[] values);
        public abstract int BulkInsert<T>(string tableName, IEnumerable<T> values);

        protected DatabaseOperation(string connStr, int commandTimeout)
        {
            _connectionString = connStr;
            _commandTimeout = commandTimeout;
        }

        protected int CommandTimeout
        {
            get { return _commandTimeout; }
        }

        protected string ConnectionString
        {
            get { return _connectionString; }
        }

        private int _commandTimeout;
        private string _connectionString;
    }
}
