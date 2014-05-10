using System;
using System.Data;
using KJFramework.Datas;

namespace KJFramework.Tracing 
{
    [Obsolete("Useless TracingProvider", true)]
    public class DbTracingProvider : ITracingProvider
    {
        private Database _db;

        public DbTracingProvider(string connStr)
        {
            _db = new Database(connStr, 12000);
        }

        public void Write(string pid, string pname, string machine, TraceItem[] items)
        {
            DataTable table = new DataTable("CMN_ServiceTrace");
            table.Columns.Add("Timestamp", typeof(DateTime));
            table.Columns.Add("Pid", typeof(string));
            table.Columns.Add("Level", typeof(int));
            table.Columns.Add("PName", typeof(string));
            table.Columns.Add("Logger", typeof(string));
            table.Columns.Add("Message", typeof(string));
            table.Columns.Add("Exception", typeof(string));
            table.Columns.Add("Machine", typeof(string));
            for (int i = 0; i < items.Length; ++i)
            {
                table.Rows.Add(
                    items[i].Timestamp, pid,
                    (int)items[i].Level, pname,
                    items[i].Logger, items[i].Message,
                    items[i].Error == null ? string.Empty : items[i].Error.ToString(),
                    machine
                );
            }
            //_db.ExecuteBulkCopy(table);
        }
    }
}
