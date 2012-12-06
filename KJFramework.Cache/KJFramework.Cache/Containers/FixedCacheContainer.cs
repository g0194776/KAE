using System;
using System.Collections.Concurrent;
using System.Threading;
using KJFramework.Cache.Cores;

namespace KJFramework.Cache.Containers
{
    /// <summary>
    ///     固态的缓存容器，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">缓存对象类型</typeparam>
    internal class FixedCacheContainer<T> : IFixedCacheContainer<T>
        where T : IClearable, new()
    {
        #region Constructor

        /// <summary>
        ///     固态的缓存容器，提供了相关的基本操作
        /// </summary>
        /// <param name="capacity">最大容量</param>
        public FixedCacheContainer(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException("Illegal capacity!");
            }
            _remainingCount = _capacity = capacity;
            _caches = new ConcurrentStack<ICacheStub<T>>();
            //initialize.
            for (int i = 0; i < _capacity; i++)
            {
                ICacheStub<T> stub = new CacheStub<T> { Fixed = true };
                stub.Cache = new CacheItem<T>();
                stub.Cache.SetValue(new T());
                _caches.Push(stub);
            }
        }

        #endregion

        #region Members

        private ConcurrentStack<ICacheStub<T>> _caches;
        private long _remainingCount;

        #endregion

        #region Implementation of IFixedCacheContainer<T>

        private readonly int _capacity;

        /// <summary>
        ///     获取当前容器的最大容量
        /// </summary>
        public int Capacity
        {
            get { return _capacity; }
        }

        /// <summary>
        ///     租借一个缓存
        /// </summary>
        /// <returns>返回一个新的缓存</returns>
        public IFixedCacheStub<T> Rent()
        {
            ICacheStub<T> cache;
            if (!_caches.IsEmpty && _caches.TryPop(out cache))
            {
                Interlocked.Decrement(ref _remainingCount);
                return (IFixedCacheStub<T>)cache;
            }
            return null;
        }

        /// <summary>
        ///     归还一个缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        public void Giveback(IFixedCacheStub<T> cache)
        {
            if (cache == null) throw new ArgumentNullException("cache");
            if (Interlocked.Read(ref _remainingCount) >= _capacity) throw new System.Exception("Can not giveback a cache, because target caches count has been full.");
            cache.Cache.Clear();
            cache.Tag = null;
            _caches.Push((ICacheStub<T>)cache);
            Interlocked.Increment(ref _remainingCount);
        }

        #endregion
    }
}