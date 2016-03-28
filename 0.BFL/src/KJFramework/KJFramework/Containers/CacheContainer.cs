using System;
using System.Collections.Generic;
using System.Threading;
using KJFramework.Cores;
using KJFramework.EventArgs;
using KJFramework.Timer;

namespace KJFramework.Containers
{
    /// <summary>
    ///     缓存容器，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="K">缓存对象Key类型</typeparam>
    /// <typeparam name="V">缓存对象类型</typeparam>
    internal class CacheContainer<K, V> : ICacheContainer<K, V>
    {
        #region Constructor

        /// <summary>
        ///     缓存容器，提供了相关的基本操作
        /// </summary>
        /// <param name="category">分类名称</param>
        public CacheContainer(string category)
            : this(category, null)
        {
        }

        /// <summary>
        ///     缓存容器，提供了相关的基本操作
        /// </summary>
        /// <param name="category">分类名称</param>
        /// <param name="comparer">比较器 </param>
        public CacheContainer(string category, IEqualityComparer<K> comparer)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException("category");
            }
            _category = category;
            _createTime = DateTime.Now;
            _expireTime = DateTime.MaxValue;
            _caches = (comparer == null
                          ? new Dictionary<K, ICacheStub<V>>()
                          : new Dictionary<K, ICacheStub<V>>(comparer));
            //10s.
            Initialize();
        }

        #endregion

        #region Members

        private LightTimer _timer;

        #endregion

        #region Methods

        /// <summary>
        ///     初始化
        /// </summary>
        protected virtual void Initialize()
        {
            //10s.
            _timer = LightTimer.NewTimer(10000, -1).Start(
                delegate
                    {
                        //clear dead.
                        if (!_isDead)
                        {
                            //container has been daed.
                            DateTime now = DateTime.Now;
                            if (now >= _expireTime)
                            {
                                Discard();
                                return;
                            }
                            lock (_lockObj)
                            {
                                IList<K> keys = new List<K>();
                                foreach (KeyValuePair<K, ICacheStub<V>> pair in _caches)
                                    if (pair.Value.GetLease().IsDead) keys.Add(pair.Key);
                                //clear it.
                                if (keys.Count <= 0) return;
                                foreach (K key in keys)
                                {
                                    ICacheStub<V> stub;
                                    if(!_caches.TryGetValue(key, out stub)) continue;
                                    _caches.Remove(key);
                                    K tempKey = key;
                                    ThreadPool.QueueUserWorkItem(delegate { CacheExpiredHandler(new ExpiredCacheEventArgs<K, V>(tempKey, ((IFixedCacheStub<V>)stub).Cache)); });
                                }
                            }
                        }
                        //clear all.
                        else
                        {
                            RecycleResource();
                        }
                    }, delegate { /*nothing to do.*/});
        }

        /// <summary>
        ///     回收资源
        /// </summary>
        protected virtual void RecycleResource()
        {
            _timer.Stop();
            _timer = null;
            _caches = null;
        }

        #endregion

        #region Implementation of ILeasable

        protected bool _isDead;
        protected DateTime _createTime;
        protected DateTime _expireTime;
        protected string _category;
        protected bool _isRemotable;
        protected readonly object _lockObj = new object();
        protected Dictionary<K, ICacheStub<V>> _caches;

        /// <summary>
        ///     获取一个值，该值表示了当前的缓存是否已经处于死亡的状态
        /// </summary>
        public bool IsDead
        {
            get { return _isDead; }
        }

        /// <summary>
        ///     获取生命周期创建的时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        /// <summary>
        ///     获取超时时间
        /// </summary>
        public DateTime ExpireTime
        {
            get { return _expireTime; }
        }

        /// <summary>
        ///     将当前缓存的生命周期置为死亡状态
        /// </summary>
        public void Discard()
        {
            if (_isDead) throw new Exception("Cannot execute discard operation on a dead container.");
            _isDead = true;
            lock (_lockObj)
            {
                //discard all cache.
                foreach (ICacheStub<V> stub in _caches.Values)
                    stub.GetLease().Discard();
                RecycleResource();
            }
        }

        /// <summary>
        ///     将当前租期续约一段时间
        /// </summary>
        /// <param name="timeSpan">续约时间</param>
        /// <returns>返回续约后的到期时间</returns>
        /// <exception cref="System.Exception">更新失败</exception>
        public DateTime Renew(TimeSpan timeSpan)
        {
            if (_isDead) throw new Exception("Cannot execute renew operation on a dead container.");
            _expireTime = (_expireTime == DateTime.MaxValue ? DateTime.Now.Add(timeSpan) : _expireTime.Add(timeSpan));
            lock (_lockObj)
            {
                //renew all.
                foreach (ICacheStub<V> stub in _caches.Values)
                    stub.GetLease().Renew(timeSpan);
                return _expireTime;
            }
        }

        #endregion

        #region Implementation of ICacheContainer<K,V>

        /// <summary>
        ///     获取当前缓存容器的分类名称
        /// </summary>
        public string Category
        {
            get { return _category; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前的缓存容器是否为远程缓存的代理器
        /// </summary>
        public bool IsRemotable
        {
            get { return _isRemotable; }
            internal set { _isRemotable = value; }
        }

        /// <summary>
        ///     查询具有指定key的缓存是否已经存在
        /// </summary>
        /// <param name="key">缓存对象Key</param>
        /// <returns>返回是否存在的状态</returns>
        public bool IsExists(K key)
        {
            if (_isDead) throw new Exception("Cannot execute isexists operation on a dead container.");
            lock (_lockObj) return _caches.ContainsKey(key);
        }

        /// <summary>
        ///     移除一个具有指定key的缓存对象
        /// </summary>
        /// <param name="key">缓存对象Key</param>
        /// <returns>返回删除后的状态</returns>
        public bool Remove(K key)
        {
            if (_isDead) throw new Exception("Cannot execute remove operation on a dead container.");
            lock (_lockObj)
            {
                _caches.Remove(key);
                return true;
            }
        }

        /// <summary>
        ///     添加一个新的缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">要缓存对的象</param>
        /// <returns>返回缓存对象</returns>
        public IReadonlyCacheStub<V> Add(K key, V obj)
        {
            if (_isDead) throw new Exception("Cannot execute add operation on a dead container.");
            return Add(key, obj, DateTime.MaxValue);
        }

        /// <summary>
        ///     添加一个新的缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">要缓存对的象</param>
        /// <param name="timeSpan">续租时间</param>
        /// <returns>返回缓存对象</returns>
        public IReadonlyCacheStub<V> Add(K key, V obj, TimeSpan timeSpan)
        {
            if (_isDead) throw new Exception("Cannot execute add operation on a dead container.");
            return Add(key, obj, DateTime.Now.Add(timeSpan));
        }

        /// <summary>
        ///     添加一个新的缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">要缓存对的象</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>返回缓存对象</returns>
        public IReadonlyCacheStub<V> Add(K key, V obj, DateTime expireTime)
        {
            if (_isDead) throw new Exception("Cannot execute add operation on a dead container.");
            ICacheStub<V> stub = new CacheStub<V>(expireTime) {Cache = new CacheItem<V>()};
            stub.Cache.SetValue(obj);
            lock (_lockObj)
            {
                ICacheStub<V> tmpStub;
                if(!_caches.TryGetValue(key, out tmpStub)) _caches.Add(key, stub);
                return (IReadonlyCacheStub<V>) stub;
            }
        }

        /// <summary>
        ///     获取一个具有指定key的缓存对象
        /// </summary>
        /// <param name="key">缓存对象Key</param>
        /// <returns>返回缓存对象</returns>
        public IReadonlyCacheStub<V> Get(K key)
        {
            if (_isDead) throw new Exception("Cannot execute get operation on a dead container.");
            lock (_lockObj)
            {
                ICacheStub<V> stub;
                if (_caches.TryGetValue(key, out stub))
                    return (IReadonlyCacheStub<V>) stub;
                return null;
            }
        }

        /// <summary>
        ///     缓存过期事件
        /// </summary>
        public event EventHandler<ExpiredCacheEventArgs<K, V>> CacheExpired;
        protected void CacheExpiredHandler(ExpiredCacheEventArgs<K, V> e)
        {
            EventHandler<ExpiredCacheEventArgs<K, V>> handler = CacheExpired;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}