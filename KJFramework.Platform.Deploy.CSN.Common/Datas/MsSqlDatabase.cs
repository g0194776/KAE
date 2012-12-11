using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace KJFramework.Platform.Deploy.CSN.Common.Datas
{
    public class MsSqlDatabase : Database
    {
        public override DatabaseType Provider
        {
            get { return DatabaseType.MsSql; }
        }

        internal protected MsSqlDatabase(string connString) : base(connString) { }

        protected override DbConnection CreateConnection()
        {
            return new SqlConnection();
        }

        protected override DbDataAdapter CreateAdapter()
        {
            return new SqlDataAdapter();
        }

        protected override DbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        protected override void SetParameter(DbParameterCollection param, string name, object value)
        {
            ((SqlParameterCollection)param).AddWithValue(name, value);
        }

        public override void ExecuteBulkCopy(DataTable table)
        {
            if (table == null || string.IsNullOrEmpty(table.TableName))
                throw new ArgumentException("table name not available");
            if (table.Rows.Count < 1 || table.Columns.Count < 1)
                return;

            using (SqlConnection conn = (SqlConnection)OpenConnection())
            using (SqlBulkCopy copy = new SqlBulkCopy(conn))
            {
                copy.BulkCopyTimeout = CommandTimeout;
                copy.BatchSize = table.Rows.Count;
                copy.WriteToServer(table);
            }
        }

        //#region
        //protected override void ExecuteNonQueryAsync(DbCommand command, CompletionCallback<int> callback, object state)
        //{
        //    ((SqlCommand)command).BeginExecuteNonQuery(
        //        ExecuteNonQueryAsyncCallback,
        //        new CompletionState<int>(callback, state, command)
        //    );
        //}

        //private static void ExecuteNonQueryAsyncCallback(IAsyncResult ar)
        //{
        //    CompletionState<int> callback = ar.AsyncState as CompletionState<int>;
        //    Exception exception = null;
        //    int rows = -1;
        //    try
        //    {
        //        rows = ((SqlCommand)callback.Context).EndExecuteNonQuery(ar);
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _tracing.Warn(ex, "failed to execute {0}", ((SqlCommand)callback.Context).CommandText);
        //    }
        //    callback.Invoke(callback.Context, exception, rows);
        //}

        //protected override void ExecuteReaderAsync(DbCommand command, CompletionCallback<DbDataReader> callback, object state)
        //{
        //    ((SqlCommand)command).BeginExecuteReader(
        //        ExecuteReaderAsyncCallback,
        //        new CompletionState<DbDataReader>(callback, state, command),
        //        CommandBehavior.CloseConnection
        //    );
        //}

        //private void ExecuteReaderAsyncCallback(IAsyncResult ar)
        //{
        //    CompletionState<DbDataReader> callback = ar.AsyncState as CompletionState<DbDataReader>;
        //    Exception exception = null;
        //    SqlDataReader reader = null;
        //    try
        //    {
        //        reader = ((SqlCommand)callback.Context).EndExecuteReader(ar);
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _tracing.Warn(ex, "failed to execute {0}", ((SqlCommand)callback.Context).CommandText);
        //    }
        //    try
        //    {
        //        callback.Invoke(callback.Context, exception, reader);
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //            reader.Close();
        //    }
        //}
        //#endregion
    }
}
