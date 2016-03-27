using System;
using System.Data;

namespace KJFramework.Datas
{
#if TDD
    public sealed class Database:MarshalByRefObject
#else 
    public sealed class Database
#endif
    {
        private DbType _dbType;
        private DatabaseOperation _innerDb;

        public Database(string connectionString, int commandTimeOut)
        {
            _innerDb = new MysqlDatabase(connectionString, commandTimeOut);
        }

        internal Database(DbType dbType, string connectionString)
        {
            _dbType = dbType;

            switch (_dbType)
            {
                case DbType.Mysql:
                    _innerDb = new MysqlDatabase(connectionString, 120);
                    break;
                case DbType.SqlServer2005:
                    _innerDb = new SqlServerDatabase(connectionString, 120);
                    break;
                default:
                    throw new NotSupportedException(string.Format("Not Support DbType:{0} {1}", dbType, connectionString));
            }
        }

        public int SpExecuteNonQuery(string spName, string[] parameters, params object[] values)
        {
            if (parameters != null && values != null && parameters.Length != values.Length)
            {
                string msg = string.Format("{0} Parameters({1}) != Values({2})", spName, parameters.Length, values.Length);
                throw new InvalidOperationException(msg);
            }

            return _innerDb.SpExecuteNonQuery(spName, parameters, values);
        }

        public T SpExecuteScalar<T>(string spName, string[] parameters, params object[] values)
        {
            if (parameters != null && values != null && parameters.Length != values.Length)
            {
                string msg = string.Format("{0} Parameters({1}) != Values({2})", spName, parameters.Length, values.Length);
                throw new InvalidOperationException(msg);
            }
            return _innerDb.SpExecuteScalar<T>(spName, parameters, values);
        }

        public DataTable SpExecuteTable(string spName, string[] parameters, params object[] values)
        {
            if (parameters != null && values != null && parameters.Length != values.Length)
            {
                string msg = string.Format("{0} Parameters({1}) != Values({2})", spName, parameters.Length, values.Length);
                throw new InvalidOperationException(msg);
            }

            return _innerDb.SpExecuteTable(spName, parameters, values);
        }

        public DataSet SpExecuteDataSet(string spName, string[] parameters, params object[] values)
        {
            if (parameters != null && values != null && parameters.Length != values.Length)
            {
                string msg = string.Format("{0} Parameters({1}) != Values({2})", spName, parameters.Length, values.Length);
                throw new InvalidOperationException(msg);
            }

            return _innerDb.SpExecuteDataSet(spName, parameters, values);
        }
    }
}
