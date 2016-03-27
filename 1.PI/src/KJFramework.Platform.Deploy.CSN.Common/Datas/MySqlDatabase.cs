using System;
using System.Text;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace KJFramework.Platform.Deploy.CSN.Common.Datas
{
    public class MySqlDatabase : Database
    {
        public override DatabaseType Provider
        {
            get { return DatabaseType.MySql; }
        }

        internal protected MySqlDatabase(string connString) : base(connString) { }

        protected override DbConnection CreateConnection()
        {
            return new MySqlConnection();
        }

        protected override DbDataAdapter CreateAdapter()
        {
            return new MySqlDataAdapter();
        }

        protected override DbCommand CreateCommand()
        {
            return new MySqlCommand();
        }

        protected override void SetParameter(DbParameterCollection param, string name, object value)
        {
            //if (value is Guid)
            //    value = ((Guid)value).KmppEncode();
            ((MySqlParameterCollection)param).AddWithValue(name, value);
        }

        public override void ExecuteBulkCopy(DataTable table)
        {
            if (table == null || string.IsNullOrEmpty(table.TableName))
                throw new ArgumentException("table name not available");
            if (table.Rows.Count < 1 || table.Columns.Count < 1)
                return;

            StringBuilder text = new StringBuilder(36 + table.TableName.Length + table.Columns.Count * 10);
            text.Append("insert into `").Append(table.TableName).Append("` (");
            for (int i = 0; i < table.Columns.Count; ++i)
            {
                text.Append('`').Append(table.Columns[i].ColumnName).Append("`,");
            }
            text[text.Length - 1] = ')';
            text.Append(" values (");
            string proceed = text.ToString();

            using (DbCommand command = CreateCommand())
            {
                text = new StringBuilder((proceed.Length << 1) * table.Rows.Count);
                for (int i = 0; i < table.Rows.Count; ++i)
                {
                    text.Append(proceed);
                    DataRow row = table.Rows[i];
                    for (int j = 0; j < table.Columns.Count; ++j)
                    {
                        string param = "@p" + (i * table.Columns.Count + j);
                        text.Append(param).Append(',');
                        SetParameter(command.Parameters, param, row[j]);
                    }
                    text[text.Length - 1] = ')';
                    text.Append(';');
                }

                using (DbConnection connection = OpenConnection())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = CommandTimeout;
                    command.Connection = connection;
                    command.CommandText = text.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }

        //#region obsolete function


        //protected override void ExecuteNonQueryAsync(DbCommand command, CompletionCallback<int> callback, object state)
        //{
        //    //throw new NotImplementedException();
        //    //((MySqlCommand)command).BeginExecuteNonQuery(
        //    //    ExecuteNonQueryAsyncCallback,
        //    //    new CompletionState<int>(callback, state, command)
        //    //);
        //}

        //private static void ExecuteNonQueryAsyncCallback(IAsyncResult ar)
        //{
        //    CompletionState<int> callback = ar.AsyncState as CompletionState<int>;
        //    Exception exception = null;
        //    int rows = -1;
        //    try
        //    {
        //        rows = ((MySqlCommand)callback.Context).EndExecuteNonQuery(ar);
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _tracing.Warn(ex, "failed to execute {0}", ((MySqlCommand)callback.Context).CommandText);
        //    }
        //    callback.Invoke(callback.Context, exception, rows);
        //}

        //protected override void ExecuteReaderAsync(DbCommand command, CompletionCallback<DbDataReader> callback, object state)
        //{
        //    throw new NotImplementedException();
        //    //((MySqlCommand)command).BeginExecuteReader(
        //    //    ExecuteReaderAsyncCallback, 
        //    //    new CompletionState<DbDataReader>(callback, state, command), 
        //    //    CommandBehavior.CloseConnection
        //    //);
        //}

        //private void ExecuteReaderAsyncCallback(IAsyncResult ar)
        //{
        //    CompletionState<DbDataReader> callback = ar.AsyncState as CompletionState<DbDataReader>;
        //    Exception exception = null;
        //    MySqlDataReader reader = null;
        //    try
        //    {
        //        reader = ((MySqlCommand)callback.Context).EndExecuteReader(ar);
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _tracing.Warn(ex, "failed to execute {0}", ((MySqlCommand)callback.Context).CommandText);
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
