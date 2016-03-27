using System;

namespace KJFramework.Cores
{
    /// <summary>
    ///     弱缓存存根，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">缓存类型</typeparam>
    internal class WeakCacheStub<T> : ICacheStub<T>, IReadonlyCacheStub<T>
    {
        #region Constructor

        /// <summary>
        ///     弱缓存存根，提供了相关的基本操作
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        public WeakCacheStub() : this(DateTime.MaxValue)
        {
            
        }

        /// <summary>
        ///     弱缓存存根，提供了相关的基本操作
        /// </summary>
        /// <param name="expireTime">过期时间</param>
        /// <typeparam name="T">缓存类型</typeparam>
        public WeakCacheStub(DateTime expireTime)
        {
            _lease = new CacheLease(expireTime);
        }

        #endregion

        #region Implementation of ICacheStub<T>

        protected bool _fixed;
        protected WeakReference _cache;
        protected ICacheLease _lease;

        /// <summary>
        ///     获取当前缓存存根的内部编号
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        ///     获取或设置一个值，该值表示了当前缓存存根是否表示为一种固态的缓存状态
        /// </summary>
        public bool Fixed
        {
            get { return _fixed; }
            set
            {
                _fixed = value;
                if (_fixed)
                {
                    throw new System.Exception("Can not set \"fixed\" state for a weak reference cache!");
                }
            }
        }

        /// <summary>
        ///     获取或设置缓存项
        /// </summary>
        public ICacheItem<T> Cache
        {
            get
            {
                if (_cache == null || !_cache.IsAlive)
                {
                    return null;
                }
                //target may be is null.
                return _cache.Target as ICacheItem<T>;
            }
            set
            {
                _cache = new WeakReference(value);
            }
        }

        /// <summary>
        ///     获取缓存的生命周期
        /// </summary>
        public ICacheLease Lease
        {
            get { return _lease; }
        }

        /// <summary>
        ///     获取缓存生命周期
        /// </summary>
        /// <returns></returns>
        public ICacheLease GetLease()
        {
            //weak cache dead? go a dead lease.
            if (_cache != null && !_cache.IsAlive)
            {
                return CacheLease.DeadLease;
            }
            return _lease;
        }

        #endregion

        #region Implementation of IReadonlyCacheStub<T>

        /// <summary>
        ///     获取缓存
        /// </summary>
        T IReadonlyCacheStub<T>.Cache
        {
            get 
            {
                ICacheItem<T> cache = Cache;
                return cache == null ? default(T) : cache.GetValue();
            }
        }

        #endregion
    }
}