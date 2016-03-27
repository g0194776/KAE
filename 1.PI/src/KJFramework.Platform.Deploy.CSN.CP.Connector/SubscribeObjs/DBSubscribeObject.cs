using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.SubscribeObjs
{
    /// <summary>
    ///     ���ݿⶩ�Ķ���
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
        ///     �������ݿ�
        /// </summary>
        /// <param name="db">���ݿ����Ƽ���</param>
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
        ///     ����ָ��DB�е����ݱ�
        ///     <para>* Ŀǰ��֧����������</para>
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="tables">���ݱ����Ƽ���</param>
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
        ///     ����ָ��DB�У�ָ�����ݱ�������
        ///     <para>* Ŀǰ��֧����������</para>
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="table">���ݱ�����</param>
        /// <param name="column">�����Ƽ���</param>
        public void AddSubscribe(string db, string table, params string[] column)
        {
            if (db == null) return;
            AddSubscribe(db, new[] {table});
            _tables[table] = column == null ? null : column.ToList();
        }

        /// <summary>
        ///     �Ƴ�����ָ��DB�Ķ���
        /// </summary>
        /// <param name="db">���ݿ����Ƽ���</param>
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
        ///     �Ƴ������ָ��DB�б�Ķ���
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="tables">���ݱ����Ƽ���</param>
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
        ///     �Ƴ������ָ��DBָ������ֶζ���
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="table">���ݱ����Ƽ���</param>
        /// <param name="column">�����Ƽ���</param>
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
        ///     ��⵱ǰ�����Ƿ�����ָ�����ݿ�
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <returns>�����Ƿ��ĵ�״̬</returns>
        public bool CheckIntersting(string db)
        {
            if (db == null) throw new ArgumentNullException("db");
            return _databases.ContainsKey(db);
        }

        /// <summary>
        ///     ��⵱ǰ�����Ƿ�����ָ�����ݿ��������ݿ��еı�
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="table">����</param>
        /// <returns>�����Ƿ��ĵ�״̬</returns>
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
        ///     ��⵱ǰ�����Ƿ�����ָ�����ݿ��������ݿ��еı��Լ������
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="table">����</param>
        /// <param name="column">����</param>
        /// <returns>�����Ƿ��ĵ�״̬</returns>
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