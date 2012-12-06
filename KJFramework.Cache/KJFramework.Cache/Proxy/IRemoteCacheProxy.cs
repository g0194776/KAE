using System;
using KJFramework.Cache.Containers;
using KJFramework.Cache.Cores;

namespace KJFramework.Cache.Proxy
{
    /// <summary>
    ///     远程缓存代理器元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="K">缓存对象Key类型</typeparam>
    /// <typeparam name="V">缓存对象类型</typeparam>
    public interface IRemoteCacheProxy<K, V>
    {
        /// <summary>
        ///     获取当前远程缓存代理器的可用性
        /// </summary>
        bool IsAvailable { get; }
        /// <summary>
        ///     获取或设置本地缓存容器
        /// </summary>
        ICacheContainer<K, V> LocalContainer { get; set; }
        /// <summary>
        ///     放弃当前容器内所有的缓存
        /// </summary>
        void Discard();
        /// <summary>
        ///     查询具有指定key的缓存是否已经存在
        /// </summary>
        /// <param name="key">缓存对象Key</param>
        /// <returns>返回是否存在的状态</returns>
        bool IsExists(K key);
        /// <summary>
        ///     移除一个具有指定key的缓存对象
        /// </summary>
        /// <param name="key">缓存对象Key</param>
        void Remove(K key);
        /// <summary>
        ///     添加一个新的缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">要缓存对的象</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>返回缓存对象</returns>
        IReadonlyCacheStub<V> Add(K key, V obj, DateTime expireTime);
        /// <summary>
        ///     获取一个具有指定key的缓存对象
        /// </summary>
        /// <param name="key">缓存对象Key</param>
        /// <returns>返回缓存对象</returns>
        IReadonlyCacheStub<V> Get(K key);
    }
}