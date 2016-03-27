using System;
using System.Collections.Generic;

namespace KJFramework.Dynamic.Tables
{
    /// <summary>
    ///     程序域对象访问规则表，提供了相关的基本操作。
    /// </summary>
    internal class DomainObjectVisitRuleTable : MarshalByRefObject, IDomainObjectVisitRuleTable
    {
        #region 析构函数

        ~DomainObjectVisitRuleTable()
        {
            Dispose();
        }

        #endregion

        #region 成员

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
        ///     添加一个访问规则
        /// </summary>
        /// <param name="key">访问键值</param>
        /// <param name="func">返回的对象</param>
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
        ///     移除一个具有指定访问键值的对象
        /// </summary>
        /// <param name="key">访问键值</param>
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
        ///     判断指定访问键值是否存在
        /// </summary>
        /// <param name="key">访问键值</param>
        /// <returns>获取是否存在的标示</returns>
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
        ///     获取具有指定名称的属性对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">属性名</param>
        /// <param name="args">调用参数</param>
        /// <returns>返回属性对象</returns>
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