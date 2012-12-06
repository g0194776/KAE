using System;

namespace KJFramework.Dynamic.Tables
{
    /// <summary>
    ///     程序域对象访问规则表元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDomainObjectVisitRuleTable : IDisposable
    {
        /// <summary>
        ///     添加一个访问规则
        /// </summary>
        /// <param name="key">访问键值</param>
        /// <param name="func">返回的对象</param>
        void Add(String key, Func<Object[], Object> func);
        /// <summary>
        ///     移除一个具有指定访问键值的对象
        /// </summary>
        /// <param name="key">访问键值</param>
        void Remove(String key);
        /// <summary>
        ///     判断指定访问键值是否存在
        /// </summary>
        /// <param name="key">访问键值</param>
        /// <returns>获取是否存在的标示</returns>
        bool Exists(String key);

        /// <summary>
        ///     获取具有指定名称的属性对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">属性名</param>
        /// <param name="args">调用参数</param>
        /// <returns>返回属性对象</returns>
        T Get<T>(String name, params Object[] args);
    }
}