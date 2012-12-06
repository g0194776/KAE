using System;
using KJFramework.Net.Cloud.Accessors.Rules;

namespace KJFramework.Net.Cloud.Accessors
{
    /// <summary>
    ///     访问器元接口，提供了相关的今本操作。
    /// </summary>
    public interface IAccessor : IDisposable
    {
        /// <summary>
        ///     获取一个对象访问规则
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="taget">对象</param>
        /// <returns>返回访问规则</returns>
        IAccessRule GetAccessRule<T>(T taget);
    }
}