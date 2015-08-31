using System;
using KJFramework.Cores;
using KJFramework.EventArgs;

namespace KJFramework.Containers
{
    /// <summary>
    ///     缓存容器元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="K">缓存对象Key类型</typeparam>
    /// <typeparam name="V">缓存对象类型</typeparam>
    public interface ICacheContainer<K, V> : ILeasable
    {
        /// <summary>
        ///     获取当前缓存容器的分类名称
        /// </summary>
        string Category { get; }
        /// <summary>
        ///     获取一个值，该值表示了当前的缓存容器是否为远程缓存的代理器
        /// </summary>
        bool IsRemotable { get; }
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
        /// <returns>返回删除后的状态</returns>
        bool Remove(K key);
        /// <summary>
        ///     添加一个新的缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">要缓存对的象</param>
        /// <returns>返回缓存对象</returns>
        IReadonlyCacheStub<V> Add(K key, V obj);
        /// <summary>
        ///     添加一个新的缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">要缓存对的象</param>
        /// <param name="timeSpan">续租时间</param>
        /// <returns>返回缓存对象</returns>
        IReadonlyCacheStub<V> Add(K key, V obj, TimeSpan timeSpan);
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
        /// <summary>
        ///     缓存过期事件
        /// </summary>
        event EventHandler<ExpiredCacheEventArgs<K, V>> CacheExpired;
    }
}