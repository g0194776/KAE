using System;
using System.Collections.Generic;
using KJFramework.Containers;
using KJFramework.Proxy;

namespace KJFramework
{
    /// <summary>
    ///     缓存租赁者，提供了相关的基本操作
    /// </summary>
    public class CacheTenant : ICacheTenant
    {
        #region Members

        private object _lockObj = new object();
        protected readonly Dictionary<string, object> _containers = new Dictionary<string,object>();

        #endregion

        #region Implementation of ICacheTenat

        /// <summary>
        ///     放弃具有指定分类的缓存容器
        /// </summary>
        /// <param name="category">分类名称</param>
        public void Discard(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException("category");
            }
            lock (_lockObj)
            {
                _containers.Remove(category);
            }
        }

        /// <summary>
        ///     获取具有指定分类名称的缓存容器
        /// </summary>
        /// <typeparam name="T">缓存容器类型</typeparam>
        /// <param name="category">分类名称</param>
        /// <returns>返回缓存容器</returns>
        public T Get<T>(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException("category");
            }
            lock (_lockObj)
            {
                object obj;
                if (_containers.TryGetValue(category, out obj))
                {
                    return (T) obj;
                }
            }
            return default(T);
        }

        /// <summary>
        ///     租借一个新的固态缓存容器
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="category">分类名称</param>
        /// <param name="capacity">最大容量</param>
        /// <returns>返回缓存容器</returns>
        public IFixedCacheContainer<T> Rent<T>(string category, int capacity) where T : IClearable, new()
        {
            if (string.IsNullOrEmpty(category)) throw new ArgumentNullException("category");
            if (capacity <= 0) throw new ArgumentException("Illelgal capacity!");
            lock (_lockObj)
            {
                object obj;
                //already has this container.
                if (_containers.TryGetValue(category, out obj)) return (IFixedCacheContainer<T>)obj;
                IFixedCacheContainer<T> container = new FixedCacheContainer<T>(capacity);
                container.BuildPerformanceCounter(category);
                _containers.Add(category, container);
                return container;
            }
        }

        /// <summary>
        ///     租借一个新的本地缓存容器
        /// </summary>
        /// <typeparam name="K">缓存key类型</typeparam>
        /// <typeparam name="V">缓存对象类型</typeparam>
        /// <param name="category">分类名称</param>
        /// <returns>返回本地缓存容器</returns>
        public ICacheContainer<K, V> Rent<K, V>(string category)
        {
            if (string.IsNullOrEmpty(category)) throw new ArgumentNullException("category");
            lock (_lockObj)
            {
                //already has this container.
                if (_containers.ContainsKey(category)) return null;
                ICacheContainer<K, V> container = new CacheContainer<K, V>(category);
                _containers.Add(category, container);
                return container;
            }
        }

        /// <summary>
        ///     租借一个新的本地缓存容器
        /// </summary>
        /// <typeparam name="K">缓存key类型</typeparam>
        /// <typeparam name="V">缓存对象类型</typeparam>
        /// <param name="category">分类名称</param>
        /// <param name="comparer">比较器 </param>
        /// <returns>返回本地缓存容器</returns>
        public ICacheContainer<K, V> Rent<K, V>(string category, IEqualityComparer<K> comparer)
        {
            if (string.IsNullOrEmpty(category)) throw new ArgumentNullException("category");
            lock (_lockObj)
            {
                //already has this container.
                if (_containers.ContainsKey(category)) return null;
                ICacheContainer<K, V> container = comparer == null
                                                      ? new CacheContainer<K, V>(category)
                                                      : new CacheContainer<K, V>(category, comparer);
                _containers.Add(category, container);
                return container;
            }
        }

        /// <summary>
        ///     租借一个新的远程缓存容器
        /// </summary>
        /// <typeparam name="K">缓存key类型</typeparam>
        /// <typeparam name="V">缓存对象类型</typeparam>
        /// <param name="category">分类名称</param>
        /// <param name="proxy">远程缓存代理器</param>
        /// <returns>返回远程缓存容器</returns>
        public ICacheContainer<K, V> Rent<K, V>(string category, IRemoteCacheProxy<K, V> proxy)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException("category");
            }
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }
            lock (_lockObj)
            {
                //already has this container.
                if (_containers.ContainsKey(category))
                {
                    return null;
                }
                IRemoteCacheContainer<K, V> container = new RemoteCacheContainer<K, V>(category, proxy);
                _containers.Add(category, container);
                return container;
            }
        }

        #endregion
    }
}