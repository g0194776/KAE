using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using MySql.Data.MySqlClient;

namespace KJFramework.Datas
{
    public class MysqlDatabase : DatabaseOperation
    {
        internal MysqlDatabase(string connStr, int commandTimeout)
            : base(connStr, commandTimeout)
        {
        }

        public override int SpExecuteNonQuery(string spName, string[] parameters, params object[] values)
        {
            int result;
            MySqlConnection cnn = GetConnection();
            MySqlCommand cmd = new MySqlCommand(spName, cnn);
            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandType = CommandType.StoredProcedure;
            SpFillParameters(cmd, parameters, values);

            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                result = cmd.ExecuteNonQuery();
                watch.Stop();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return result;
        }

        public override T SpExecuteScalar<T>(string spName, string[] parameters, params object[] values)
        {
            T result = default(T);

            MySqlConnection cnn = GetConnection();
            MySqlCommand cmd = new MySqlCommand(spName, cnn);

            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandType = CommandType.StoredProcedure;
            SpFillParameters(cmd, parameters, values);

            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                result = (T)Convert.ChangeType(cmd.ExecuteScalar(), typeof(T));
                watch.Stop();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return result;
        }

        public override DataTable SpExecuteTable(string spName, string[] parameters, params object[] values)
        {
            DataTable result = new DataTable();
            MySqlConnection cnn = GetConnection();
            MySqlCommand cmd = new MySqlCommand(spName, cnn);

            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandType = CommandType.StoredProcedure;
            SpFillParameters(cmd, parameters, values);
                        
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                adp.FillError += new FillErrorEventHandler(
                    delegate(object sender, FillErrorEventArgs e)
                    {
                        e.Continue = true;
                        e.DataTable.Rows.Add(e.Values);
                    }
                );
                adp.Fill(result);
                watch.Stop();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }

            return result;
        }

        public override DataSet SpExecuteDataSet(string spName, string[] parameters, params object[] values)
        {
            DataSet result = new DataSet();
            MySqlConnection cnn = GetConnection();
            MySqlCommand cmd = new MySqlCommand(spName, cnn);

            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandType = CommandType.StoredProcedure;
            SpFillParameters(cmd, parameters, values);

            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                adp.Fill(result);
                watch.Stop();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return result;
        }

        public override int BulkInsert<T>(string tableName, IEnumerable<T> values)
        {
            throw new NotImplementedException();
        }

        public override string FormatSql(string spName, string[] parameters, params object[] values)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                str.Append("CALL ");
                str.Append(spName);
                if (values != null && values.Length != 0)
                {
                    str.Append("(");
                    for (int i = 0; i < values.Length; i++)
                    {
                        str.Append(SqlUtils.FormatSql(values[i]));
                        if (i != values.Length - 1)
                            str.Append(", ");
                    }
                    str.Append(")");
                }
                return str.ToString();
            }
            catch (System.Exception ex)
            {
                return "Format Failed:" + ex;
            }
        }

        private MySqlConnection GetConnection()
        {
            MySqlConnection cnn = new MySqlConnection(ConnectionString);
            try
            {
                cnn.Open();
            }
            catch (System.Exception)
            {
                cnn.Close();
                throw;
            }

            return cnn;
        }

        private void SpFillParameters(MySqlCommand command, string[] parameters, object[] values)
        {
            if (parameters == null || parameters.Length == 0)
                return;

            if (values == null || values.Length == 0)
                return;

            for (int i = 0; i < parameters.Length; i++)
            {
                string parameter = parameters[i];
                object value = values[i];
                if (value != null)
                    command.Parameters.AddWithValue(parameter.Insert(0, "p_"), value);
                else
                    command.Parameters.AddWithValue(parameter.Insert(0, "p_"), DBNull.Value);
            }
        }

    }
}
