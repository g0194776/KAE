using System;
using KJFramework.Containers;
using KJFramework.Cores;

namespace KJFramework.Proxy
{
    /// <summary>
    ///     远程缓存代理器抽象父类，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="K">缓存对象Key类型</typeparam>
    /// <typeparam name="V">缓存对象类型</typeparam>
    public abstract class RemoteCacheProxy<K, V> : IRemoteCacheProxy<K, V>
    {
        #region Constructor

        /// <summary>
        ///     远程缓存代理器抽象父类，提供了相关的基本操作
        /// </summary>
        protected RemoteCacheProxy()
            : this(null)
        {

        }

        /// <summary>
        ///     远程缓存代理器抽象父类，提供了相关的基本操作
        /// </summary>
        /// <param name="localContainer">本地缓存容器</param>
        protected RemoteCacheProxy(ICacheContainer<K, V> localContainer)
        {
            _localContainer = localContainer;
        }

        #endregion

        #region Implementation of IRemoteCacheProxy<K,V>

        protected bool _isAvailable;
        protected ICacheContainer<K, V> _localContainer;

        /// <summary>
        ///     获取当前远程缓存代理器的可用性
        /// </summary>
        public bool IsAvailable
        {
            get { return _isAvailable; }
        }

        /// <summary>
        ///     获取或设置本地缓存容器
        /// </summary>
        public ICacheContainer<K, V> LocalContainer
        {
            get { return _localContainer; }
            set { _localContainer = value; }
        }

        /// <summary>
        ///     放弃当前容器内所有的缓存
        /// </summary>
        public abstract void Discard();
        /// <summary>
        ///     查询具有指定key的缓存是否已经存在
        /// </summary>
        /// <param name="key">缓存对象Key</param>
        /// <returns>返回是否存在的状态</returns>
        public abstract bool IsExists(K key);
        /// <summary>
        ///     移除一个具有指定key的缓存对象
        /// </summary>
        /// <param name="key">缓存对象Key</param>
        public abstract void Remove(K key);
        /// <summary>
        ///     添加一个新的缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">要缓存对的象</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>返回缓存对象</returns>
        public abstract IReadonlyCacheStub<V> Add(K key, V obj, DateTime expireTime);
        /// <summary>
        ///     获取一个具有指定key的缓存对象
        /// </summary>
        /// <param name="key">缓存对象Key</param>
        /// <returns>返回缓存对象</returns>
        public abstract IReadonlyCacheStub<V> Get(K key);

        #endregion
    }
}