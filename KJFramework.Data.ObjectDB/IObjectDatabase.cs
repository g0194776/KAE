using System;

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
        void Store(object obj);
    }
}