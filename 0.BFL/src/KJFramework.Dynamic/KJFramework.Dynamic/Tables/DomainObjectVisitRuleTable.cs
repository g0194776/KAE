using System;
using System.Collections.Generic;

namespace KJFramework.Dynamic.Tables
{
    /// <summary>
    ///     �����������ʹ�����ṩ����صĻ���������
    /// </summary>
    internal class DomainObjectVisitRuleTable : MarshalByRefObject, IDomainObjectVisitRuleTable
    {
        #region ��������

        ~DomainObjectVisitRuleTable()
        {
            Dispose();
        }

        #endregion

        #region ��Ա

        private Object _lockObj = new Object();
        protected Dictionary<String, Func<object[], object>> _rules = new Dictionary<String, Func<object[], object>>();

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IDomainObjectVisitRuleTable

        /// <summary>
        ///     ���һ�����ʹ���
        /// </summary>
        /// <param name="key">���ʼ�ֵ</param>
        /// <param name="func">���صĶ���</param>
        public void Add(string key, Func<object[], object> func)
        {
            if (String.IsNullOrEmpty(key) || func == null)
            {
                return;
            }
            lock (_lockObj)
            {
                if (_rules.ContainsKey(key))
                {
                    _rules[key] = func;
                    return;
                }
                _rules.Add(key, func);
            }
        }

        /// <summary>
        ///     �Ƴ�һ������ָ�����ʼ�ֵ�Ķ���
        /// </summary>
        /// <param name="key">���ʼ�ֵ</param>
        public void Remove(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return;
            }
            lock (_lockObj)
            {
                if (_rules.ContainsKey(key))
                {
                    _rules.Remove(key);
                }
            }
        }

        /// <summary>
        ///     �ж�ָ�����ʼ�ֵ�Ƿ����
        /// </summary>
        /// <param name="key">���ʼ�ֵ</param>
        /// <returns>��ȡ�Ƿ���ڵı�ʾ</returns>
        public bool Exists(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return false;
            }
            lock (_lockObj)
            {
                return _rules.ContainsKey(key);
            }
        }

        /// <summary>
        ///     ��ȡ����ָ�����Ƶ����Զ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="name">������</param>
        /// <param name="args">���ò���</param>
        /// <returns>�������Զ���</returns>
        public T Get<T>(string name, params Object[] args)
        {
            if (String.IsNullOrEmpty(name))
            {
                return default(T);
            }
            lock (_lockObj)
            {
                if (_rules.ContainsKey(name))
                {
                    return (T)_rules[name](args);
                }
            }
            return default(T);
        }

        #endregion
    }
}