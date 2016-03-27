using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.SubscribeObjs
{
    /// <summary>
    ///     数据库订阅对象
    /// </summary>
    public class DBSubscribeObject : IDBSubscribeObject
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DBSubscribeObject));
        private Dictionary<string, List<string>> _databases = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> _tables = new Dictionary<string, List<string>>();

        #endregion

        #region Implementation of IDBSubscribeObject

        /// <summary>
        ///     订阅数据库
        /// </summary>
        /// <param name="db">数据库名称集合</param>
        public void AddSubscribe(params string[] db)
        {
            try
            {
                if (db == null || db.Length == 0)
                {
                    return;
                }
                foreach (string s in db)
                {
                    if (!_databases.ContainsKey(s))
                    {
                        _databases.Add(s, new List<string>());
                    }
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
            }
        }

        /// <summary>
        ///     订阅指定DB中的数据表
        ///     <para>* 目前不支持增量订阅</para>
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="tables">数据表名称集合</param>
        public void AddSubscribe(string db, params string[] tables)
        {
            if (db == null) return;
            AddSubscribe(new[] {db});
            _databases[db] = tables == null ? null : tables.ToList();
            if (tables == null) return;
            foreach (string table in tables.Where(table => !_tables.ContainsKey(table)))
            {
                _tables.Add(table, new List<string>());
            }
        }

        /// <summary>
        ///     订阅指定DB中，指定数据表的相关列
        ///     <para>* 目前不支持增量订阅</para>
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="table">数据表名称</param>
        /// <param name="column">列名称集合</param>
        public void AddSubscribe(string db, string table, params string[] column)
        {
            if (db == null) return;
            AddSubscribe(db, new[] {table});
            _tables[table] = column == null ? null : column.ToList();
        }

        /// <summary>
        ///     移除对于指定DB的订阅
        /// </summary>
        /// <param name="db">数据库名称集合</param>
        public void RemoveSubscribe(params string[] db)
        {
            if (db == null) return;
            List<string> tables;
            for (int i = 0; i < db.Length; i++)
            {
                if (_databases.TryGetValue(db[i], out tables))
                {
                    if (tables != null)
                    {
                        foreach (string table in tables)
                        {
                            _tables.Remove(table);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     移除针对于指定DB中表的订阅
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="tables">数据表名称集合</param>
        public void RemoveSubscribe(string db, params string[] tables)
        {
            if (db == null) return;
            List<string> t;
            if (_databases.TryGetValue(db, out t))
            {
                if (t != null)
                {
                    foreach (string s in tables)
                    {
                        if (t.Remove(s))
                        {
                            _tables.Remove(s);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     移除针对于指定DB指定表的字段订阅
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="table">数据表名称集合</param>
        /// <param name="column">列名称集合</param>
        public void RemoveSubscribe(string db, string table, params string[] column)
        {
            if (db == null) return;
            List<string> tables;
            if (!_databases.TryGetValue(db, out tables)) return;
            if (tables == null) return;
            foreach (string t in tables)
            {
                if (t == table)
                {
                    List<string> columns = _tables[table];
                    if (columns != null && column != null)
                    {
                        foreach (string col in column)               
                        {
                            columns.Remove(col);
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        ///     检测当前对象是否订阅了指定数据库
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <returns>返回是否订阅的状态</returns>
        public bool CheckIntersting(string db)
        {
            if (db == null) throw new ArgumentNullException("db");
            return _databases.ContainsKey(db);
        }

        /// <summary>
        ///     检测当前对象是否订阅了指定数据库和这个数据库中的表
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="table">表名</param>
        /// <returns>返回是否订阅的状态</returns>
        public bool CheckIntersting(string db, string table)
        {
            List<string> tables;
            if (_databases.TryGetValue(db, out tables))
            {
                return tables.Contains(table);
            }
            return false;
        }

        /// <summary>
        ///     检测当前对象是否订阅了指定数据库和这个数据库中的表以及相关列
        /// </summary>
        /// <param name="db">数据库名称</param>
        /// <param name="table">表名</param>
        /// <param name="column">列名</param>
        /// <returns>返回是否订阅的状态</returns>
        public bool CheckIntersting(string db, string table, string column)
        {
            List<string> tables;
            if (_databases.TryGetValue(db, out tables))
            {
                List<string> columns;
                if (tables.Contains(table) && _tables.TryGetValue(table, out columns))
                {
                    if (columns.Contains(column))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}