using System;
using System.Diagnostics;
using System.Data;
using System.Data.Common;
using KJFramework.Tracing;

namespace KJFramework.Datas
{
    public enum DatabaseType { MsSql, MySql }

    public abstract class Database
    {
#if LocalDebug
#else
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(Database));
#endif

        public abstract DatabaseType Provider { get; }

        private string _connectionString;
        protected string ConnectionString
        {
            get { return _connectionString; }
        }

        private int _commandTimeout = 30;
        public int CommandTimeout
        {
            set { _commandTimeout = value; }
            get { return _commandTimeout; }
        }

        protected Database(string connString)
        {
            _connectionString = connString;
        }

        protected abstract DbConnection CreateConnection();
        protected abstract DbDataAdapter CreateAdapter();
        protected abstract DbCommand CreateCommand();
        protected abstract void SetParameter(DbParameterCollection param, string name, object value);

        //protected abstract void ExecuteNonQueryAsync(DbCommand command, CompletionCallback<int> callback, object state);
        //protected abstract void ExecuteReaderAsync(DbCommand command, CompletionCallback<DbDataReader> callback, object state);

        public abstract void ExecuteBulkCopy(DataTable table);

        protected DbConnection OpenConnection()
        {
            DbConnection conn = CreateConnection();
            try
            {
                DbPerfCounter.Total.ConcurrentOfConnectionAttempt.Increment();
                conn.ConnectionString = ConnectionString;
                conn.Open();
                DbPerfCounter.Total.RateOfConnectionSuccess.Increment();
            }
            catch
            {
                conn.Close();
                DbPerfCounter.Total.RateOfConnectionFailure.Increment();
                throw;
            }
            finally
            {
                DbPerfCounter.Total.ConcurrentOfConnectionAttempt.Decrement();
            }
            return conn;
        }

        protected void CloseConnection(DbConnection dbConnection)
        {
            try
            {
                if (dbConnection != null)
                    dbConnection.Close();
            }
            catch (System.Exception ex)
            {
#if LocalDebug
#else
                _tracing.Error("ErrorMsg:{0}\r\n{1}", ex.Message, ex.StackTrace);
#endif
            }
        }

        private DbCommand PrepareCommand(string procedure, CmdParameter[] parameters, out DbConnection connection)
        {
            connection = OpenConnection();
            try
            {
                DbCommand command = CreateCommand();
                command.CommandText = procedure;
                command.CommandTimeout = CommandTimeout;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                if (parameters != null)
                {
                    foreach (CmdParameter parameter in parameters)
                        SetParameter(command.Parameters, parameter.Name, parameter.Value);
                }
                return command;
            }
            catch
            {
                CloseConnection(connection);
                throw;
            }
        }

        public int ExecuteNonQuery(string procedure, params CmdParameter[] parameters)
        {
            DbPerfCounter perf = DbPerfCounter.GetInst(procedure);
            Stopwatch watch = Stopwatch.StartNew();
            DbPerfCounter.Total.ConcurrentOfCommandExecution.Increment();
            DbConnection conn = null;
            perf.ConcurrentOfCommandExecution.Increment();
            try
            {
                int rows = 0;
                using (DbCommand command = PrepareCommand(procedure, parameters, out conn))
                {
                    rows = command.ExecuteNonQuery();
                }
                DbPerfCounter.Total.RateOfCommandSuccess.Increment();
                perf.RateOfCommandSuccess.Increment();
                return rows;
            }
            catch
            {
                DbPerfCounter.Total.RateOfCommandFailure.Increment();
                perf.RateOfCommandFailure.Increment();
                throw;
            }
            finally
            {
                CloseConnection(conn);
                watch.Stop();
                DbPerfCounter.Total.ConcurrentOfCommandExecution.Decrement();
                perf.ConcurrentOfCommandExecution.Decrement();
                DbPerfCounter.Total.AvgTimeOfCommandExecution.IncrementBy(watch.ElapsedMilliseconds);
                perf.AvgTimeOfCommandExecution.IncrementBy(watch.ElapsedMilliseconds);
            }
        }

        public object ExecuteScalar(string procedure, params CmdParameter[] parameters)
        {
            DbPerfCounter perf = DbPerfCounter.GetInst(procedure);
            Stopwatch watch = Stopwatch.StartNew();
            DbPerfCounter.Total.ConcurrentOfCommandExecution.Increment();
            perf.ConcurrentOfCommandExecution.Increment();
            DbConnection conn = null;
            try
            {
                object scalar = null;
                using (DbCommand command = PrepareCommand(procedure, parameters, out conn))
                {
                    scalar = command.ExecuteScalar();
                }
                DbPerfCounter.Total.RateOfCommandSuccess.Increment();
                perf.RateOfCommandSuccess.Increment();
                return scalar;
            }
            catch
            {
                DbPerfCounter.Total.RateOfCommandFailure.Increment();
                perf.RateOfCommandFailure.Increment();
                throw;
            }
            finally
            {
                CloseConnection(conn);
                watch.Stop();
                DbPerfCounter.Total.ConcurrentOfCommandExecution.Decrement();
                perf.ConcurrentOfCommandExecution.Decrement();
                DbPerfCounter.Total.AvgTimeOfCommandExecution.IncrementBy(watch.ElapsedMilliseconds);
                perf.AvgTimeOfCommandExecution.IncrementBy(watch.ElapsedMilliseconds);
            }
        }

        public DbReader ExecuteReader(string procedure, params CmdParameter[] parameters)
        {
            DbPerfCounter perf = DbPerfCounter.GetInst(procedure);
            Stopwatch watch = Stopwatch.StartNew();
            DbPerfCounter.Total.ConcurrentOfCommandExecution.Increment();
            perf.ConcurrentOfCommandExecution.Increment();
            DbConnection conn = null;
            try
            {
                DbCommand command = PrepareCommand(procedure, parameters, out conn);
                DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                DbPerfCounter.Total.RateOfCommandSuccess.Increment();
                perf.RateOfCommandSuccess.Increment();
                return new DbReader(command, reader);
            }
            catch
            {
                CloseConnection(conn);
                DbPerfCounter.Total.RateOfCommandFailure.Increment();
                perf.RateOfCommandFailure.Increment();
                throw;
            }
            finally
            {
                watch.Stop();
                DbPerfCounter.Total.ConcurrentOfCommandExecution.Decrement();
                perf.ConcurrentOfCommandExecution.Decrement();
                DbPerfCounter.Total.AvgTimeOfCommandExecution.IncrementBy(watch.ElapsedMilliseconds);
                perf.AvgTimeOfCommandExecution.IncrementBy(watch.ElapsedMilliseconds);
            }
        }

        public DataTable ExecuteDataTable(string procedure, params CmdParameter[] parameters)
        {
            DbPerfCounter perf = DbPerfCounter.GetInst(procedure);
            Stopwatch watch = Stopwatch.StartNew();
            DbPerfCounter.Total.ConcurrentOfCommandExecution.Increment();
            perf.ConcurrentOfCommandExecution.Increment();
            DbConnection conn = null;
            try
            {
                DataTable data = new DataTable();
                using (DbCommand command = PrepareCommand(procedure, parameters, out conn))
                {
                    DbDataAdapter adapter = CreateAdapter();
                    adapter.SelectCommand = command;
                    adapter.Fill(data);
                }
                DbPerfCounter.Total.RateOfCommandSuccess.Increment();
                perf.RateOfCommandSuccess.Increment();
                return data;
            }
            catch
            {
                DbPerfCounter.Total.RateOfCommandFailure.Increment();
                perf.RateOfCommandFailure.Increment();
                throw;
            }
            finally
            {
                CloseConnection(conn);
                watch.Stop();
                DbPerfCounter.Total.ConcurrentOfCommandExecution.Decrement();
                perf.ConcurrentOfCommandExecution.Decrement();
                DbPerfCounter.Total.AvgTimeOfCommandExecution.IncrementBy(watch.ElapsedMilliseconds);
                perf.AvgTimeOfCommandExecution.IncrementBy(watch.ElapsedMilliseconds);
            }
        }

        public DataSet ExecuteDataSet(string procedure, params CmdParameter[] parameters)
        {
            DbPerfCounter perf = DbPerfCounter.GetInst(procedure);
            Stopwatch watch = Stopwatch.StartNew();
            DbPerfCounter.Total.ConcurrentOfCommandExecution.Increment();
            perf.ConcurrentOfCommandExecution.Increment();
            DbConnection conn = null;
            try
            {
                DataSet data = new DataSet();
                using (DbCommand command = PrepareCommand(procedure, parameters, out conn))
                {
                    DbDataAdapter adapter = CreateAdapter();
                    adapter.SelectCommand = command;
                    adapter.Fill(data);
                }
                DbPerfCounter.Total.RateOfCommandSuccess.Increment();
                perf.RateOfCommandSuccess.Increment();
                return data;
            }
            catch
            {
                DbPerfCounter.Total.RateOfCommandFailure.Increment();
                perf.RateOfCommandFailure.Increment();
                throw;
            }
            finally
            {
                CloseConnection(conn);
                watch.Stop();
                DbPerfCounter.Total.ConcurrentOfCommandExecution.Decrement();
                perf.ConcurrentOfCommandExecution.Decrement();
                DbPerfCounter.Total.AvgTimeOfCommandExecution.IncrementBy(watch.ElapsedMilliseconds);
                perf.AvgTimeOfCommandExecution.IncrementBy(watch.ElapsedMilliseconds);
            }
        }

        //#region
        //public void ExecuteNonQueryAsync(string procedure, CompletionCallback<int> callback, object state, params CmdParameter[] parameters)
        //{
        //    DbPerfCounter.Total.ConcurrentOfCommandExecution.Increment();
        //    DbPerfCounter.GetInst(procedure).ConcurrentOfCommandExecution.Increment();
        //    DbConnection conn = null;
        //    ExecuteNonQueryAsync(
        //        PrepareCommand(procedure, parameters, out conn),
        //        ExecuteNonQueryAsyncCallback,
        //        new CompletionState<int>(callback, state, Stopwatch.StartNew())
        //    );
        //}

        //private void ExecuteNonQueryAsyncCallback(object command, Exception exception, int rows, object state)
        //{
        //    CloseConnection(((DbCommand)command).Connection);

        //    CompletionState<int> callback = state as CompletionState<int>;
        //    ((Stopwatch)callback.Context).Stop();

        //    DbPerfCounter perf = DbPerfCounter.GetInst(((DbCommand)command).CommandText);
        //    DbPerfCounter.Total.ConcurrentOfCommandExecution.Decrement();
        //    perf.ConcurrentOfCommandExecution.Decrement();
        //    DbPerfCounter.Total.AvgTimeOfCommandExecution.IncrementBy(((Stopwatch)callback.Context).ElapsedMilliseconds);
        //    perf.AvgTimeOfCommandExecution.IncrementBy(((Stopwatch)callback.Context).ElapsedMilliseconds);
        //    if (exception == null)
        //    {
        //        DbPerfCounter.Total.RateOfCommandSuccess.Increment();
        //        perf.RateOfCommandSuccess.Increment();
        //    }
        //    else
        //    {
        //        DbPerfCounter.Total.RateOfCommandFailure.Increment();
        //        perf.RateOfCommandFailure.Increment();
        //    }

        //    callback.Invoke(this, exception, rows);
        //}

        //public void ExecuteReaderAsync(string procedure, CompletionCallback<DbReader> callback, object state, params CmdParameter[] parameters)
        //{
        //    DbPerfCounter.Total.ConcurrentOfCommandExecution.Increment();
        //    DbPerfCounter.GetInst(procedure).ConcurrentOfCommandExecution.Increment();
        //    DbConnection conn = null;
        //    ExecuteReaderAsync(
        //        PrepareCommand(procedure, parameters, out conn),
        //        ExecuteReaderAsyncCallback,
        //        new CompletionState<DbReader>(callback, state, Stopwatch.StartNew())
        //    );
        //}

        //private void ExecuteReaderAsyncCallback(object command, Exception exception, DbDataReader reader, object state)
        //{
        //    CompletionState<DbReader> callback = state as CompletionState<DbReader>;
        //    ((Stopwatch)callback.Context).Stop();

        //    DbPerfCounter perf = DbPerfCounter.GetInst(((DbCommand)command).CommandText);
        //    DbPerfCounter.Total.ConcurrentOfCommandExecution.Decrement();
        //    perf.ConcurrentOfCommandExecution.Decrement();
        //    DbPerfCounter.Total.AvgTimeOfCommandExecution.IncrementBy(((Stopwatch)callback.Context).ElapsedMilliseconds);
        //    perf.AvgTimeOfCommandExecution.IncrementBy(((Stopwatch)callback.Context).ElapsedMilliseconds);

        //    if (exception == null)
        //    {
        //        DbPerfCounter.Total.RateOfCommandSuccess.Increment();
        //        perf.RateOfCommandSuccess.Increment();
        //        callback.Invoke(this, exception, new DbReader((DbCommand)command, reader));
        //    }
        //    else
        //    {
        //        DbPerfCounter.Total.RateOfCommandFailure.Increment();
        //        perf.RateOfCommandFailure.Increment();
        //        callback.Invoke(this, exception, null);
        //    }
        //}

        //public void ExecuteScalarAsync(string procedure, CompletionCallback<object> callback, object state, params CmdParameter[] parameters)
        //{
        //    DbPerfCounter.Total.ConcurrentOfCommandExecution.Increment();
        //    DbPerfCounter.GetInst(procedure).ConcurrentOfCommandExecution.Increment();
        //    DbConnection conn = null;
        //    ExecuteReaderAsync(
        //        PrepareCommand(procedure, parameters, out conn),
        //        ExecuteScalarAsyncCallback,
        //        new CompletionState<object>(callback, state, Stopwatch.StartNew())
        //    );
        //}

        //private void ExecuteScalarAsyncCallback(object command, Exception exception, DbDataReader reader, object state)
        //{
        //    CompletionState<object> callback = state as CompletionState<object>;
        //    ((Stopwatch)callback.Context).Stop();

        //    DbPerfCounter perf = DbPerfCounter.GetInst(((DbCommand)command).CommandText);
        //    DbPerfCounter.Total.ConcurrentOfCommandExecution.Decrement();
        //    perf.ConcurrentOfCommandExecution.Decrement();
        //    DbPerfCounter.Total.AvgTimeOfCommandExecution.IncrementBy(((Stopwatch)callback.Context).ElapsedMilliseconds);
        //    perf.AvgTimeOfCommandExecution.IncrementBy(((Stopwatch)callback.Context).ElapsedMilliseconds);

        //    if (exception == null)
        //    {
        //        DbPerfCounter.Total.RateOfCommandSuccess.Increment();
        //        perf.RateOfCommandSuccess.Increment();
        //        callback.Invoke(this, exception, reader.GetValue(0));
        //    }
        //    else
        //    {
        //        DbPerfCounter.Total.RateOfCommandFailure.Increment();
        //        perf.RateOfCommandFailure.Increment();
        //        callback.Invoke(this, exception, null);
        //    }

        //    if (reader != null)
        //        reader.Close();
        //}
        //#endregion

        private static DatabaseType GetDatabaseType(ref string connString, bool modify)
        {
            int i = connString.IndexOf(':');
            if (i > 0)
            {
                string prefix = connString.Substring(0, i);
                if (string.Compare(prefix, "MySql", true) == 0)
                {
                    if (modify)
                        connString = connString.Substring(i + 1);
                    return DatabaseType.MySql;
                }
                if (string.Compare(prefix, "MsSql", true) == 0)
                {
                    if (modify)
                        connString = connString.Substring(i + 1);
                    return DatabaseType.MsSql;
                }
            }
            return DatabaseType.MySql;
        }

        public static Database GetDatabase(string connString)
        {
            switch (GetDatabaseType(ref connString, true))
            {
                case DatabaseType.MsSql: return new MsSqlDatabase(connString);
                default: throw new NotSupportedException();
            }
        }
    }
}