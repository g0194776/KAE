using System;
using System.Collections.Generic;
using KJFramework.Data.ObjectDB.Exceptions;

namespace KJFramework.Data.ObjectDB
{
    /// <summary>
    ///     对象数据库接口
    /// </summary>
    public interface IObjectDatabase
    {
        /// <summary>
        ///     存储一个对象
        /// </summary>
        /// <param name="obj">需要被存储的对象</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="HookProcessException">数据钩子处理失败</exception>
        void Store(object obj);
        /// <summary>
        ///     获取保存在数据库中所有指定类型的对象集合
        /// </summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <returns>返回相关对象集合, 如果无法查找到有效对象则返回null.</returns>
        IList<T> Get<T>() where T : new();
    }
}