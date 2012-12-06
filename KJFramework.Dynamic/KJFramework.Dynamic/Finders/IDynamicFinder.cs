using System;

namespace KJFramework.Dynamic.Finders
{
    /// <summary>
    ///     动态查找器，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">查找类型</typeparam>
    public interface IDynamicFinder<T> : IDisposable
    {
        /// <summary>
        ///     查找一个路径下所有的动态程序域组件
        /// </summary>
        /// <param name="path">查找路径</param>
        /// <returns>返回程序域组件入口点信息集合</returns>
        T Execute(String path);
    }
}