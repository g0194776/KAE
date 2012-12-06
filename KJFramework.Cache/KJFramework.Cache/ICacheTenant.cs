using System.Collections.Generic;
using KJFramework.Cache.Containers;
using KJFramework.Cache.Proxy;

namespace KJFramework.Cache
{
    /// <summary>
    ///     缓存租赁者元接口，提供了相关的基本操作
    /// </summary>
    public interface ICacheTenant
    {
        /// <summary>
        ///     放弃具有指定分类的缓存容器
        /// </summary>
        /// <param name="category">分类名称</param>
        void Discard(string category);
        /// <summary>
        ///     获取具有指定分类名称的缓存容器
        /// </summary>
        /// <typeparam name="T">缓存容器类型</typeparam>
        /// <param name="category">分类名称</param>
        /// <returns>返回缓存容器</returns>
        T Get<T>(string category);
        /// <summary>
        ///     租借一个新的固态缓存容器
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="category">分类名称</param>
        /// <param name="capacity">最大容量</param>
        /// <returns>返回缓存容器</returns>
        IFixedCacheContainer<T> Rent<T>(string category, int capacity) where T : IClearable, new();
        /// <summary>
        ///     租借一个新的本地缓存容器
        /// </summary>
        /// <typeparam name="K">缓存key类型</typeparam>
        /// <typeparam name="V">缓存对象类型</typeparam>
        /// <param name="category">分类名称</param>
        /// <returns>返回本地缓存容器</returns>
        ICacheContainer<K, V> Rent<K, V>(string category);
        /// <summary>
        ///     租借一个新的本地缓存容器
        /// </summary>
        /// <typeparam name="K">缓存key类型</typeparam>
        /// <typeparam name="V">缓存对象类型</typeparam>
        /// <param name="category">分类名称</param>
        /// <param name="comparer">比较器 </param>
        /// <returns>返回本地缓存容器</returns>
        ICacheContainer<K, V> Rent<K, V>(string category, IEqualityComparer<K> comparer);
        /// <summary>
        ///     租借一个新的远程缓存容器
        /// </summary>
        /// <typeparam name="K">缓存key类型</typeparam>
        /// <typeparam name="V">缓存对象类型</typeparam>
        /// <param name="category">分类名称</param>
        /// <param name="proxy">远程缓存代理器</param>
        /// <returns>返回远程缓存容器</returns>
        ICacheContainer<K, V> Rent<K, V>(string category, IRemoteCacheProxy<K, V> proxy);
    }
}