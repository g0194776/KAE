using System;
using System.Collections.Generic;
using System.Data;

namespace KJFramework.Datas
{
    public class SqlServerDatabase : DatabaseOperation
    {
        internal SqlServerDatabase(string connStr, int commandTimeout)
            : base(connStr, commandTimeout)
        {
        }

        public override int SpExecuteNonQuery(string spName, string[] paramters, params object[] values)
        {
            throw new NotImplementedException();
        }
        public override T SpExecuteScalar<T>(string spName, string[] parameters, params object[] values)
        {
            throw new NotImplementedException();
        }

        public override DataTable SpExecuteTable(string spName, string[] parameters, params object[] values)
        {
            throw new NotImplementedException();
        }
        public override DataSet SpExecuteDataSet(string spName, string[] paramters, params object[] values)
        {
            throw new NotImplementedException();
        }

        public override string FormatSql(string spName, string[] parameters, params object[] values)
        {
            throw new NotImplementedException();
        }
        public override int BulkInsert<T>(string tableName, IEnumerable<T> values)
        {
            throw new NotImplementedException();
        }
    }
}
