using System;
using KJFramework.Matcher;

namespace KJFramework.Loader
{
    /// <summary>
    ///     装载器元接口，提供了相关的基本操作。
    /// </summary>
    public interface ILoader
    {
        /// <summary>
        ///     获取或设置匹配器
        /// </summary>
        IMatcher Matcher { get; set; }
        /// <summary>
        ///     将自身类型作为要装载的类型，进行自动注入填充
        /// </summary>
        /// <param name="type">自身的类型 [ref]</param>
        /// <param name="path">文件全路径</param>
        /// <param name="obj">自身实例</param>
        /// <returns>返回装载的状态</returns>
        bool Load(Type type, String path, Object obj);
        /// <summary>
        ///     将自身类型作为要装载的类型，进行自动注入填充
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <returns>返回装载的状态</returns>
        bool Load(String path);
        /// <summary>
        ///     将一个文件自动注入到给定类型的必要字段中
        /// </summary>
        /// <typeparam name="T">要注入的类型</typeparam>
        /// <param name="path">文件全路径</param>
        /// <returns>返回装载的后对象，如果返回null, 则表示装载失败</returns>
        T Load<T> (String path);
    }
}