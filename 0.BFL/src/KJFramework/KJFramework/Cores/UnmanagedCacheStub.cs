using System;

namespace KJFramework.Cores
{
    /// <summary>
    ///     非托管缓存存根，提供了相关的基本操作
    /// </summary>
    internal class UnmanagedCacheStub : IUnmanagedCacheStub, IUnmanagedCacheSlot
    {
        #region Constructor

        /// <summary>
        ///     非托管缓存存根，提供了相关的基本操作
        /// </summary>
        /// <param name="maxSize">最大容量</param>
        public UnmanagedCacheStub(int maxSize) : this(maxSize, DateTime.MaxValue)
        { }

        /// <summary>
        ///     非托管缓存存根，提供了相关的基本操作
        /// </summary>
        /// <param name="maxSize">最大容量</param>
        /// <param name="expireTime">过期时间</param>
        public UnmanagedCacheStub(int maxSize, DateTime expireTime)
        {
            _fixed = false;
            _innerCache = new UnmanagedCacheItem(maxSize);
            _lease = new CacheLease(expireTime);
        }

        /// <summary>
        ///     非托管缓存存根，提供了相关的基本操作
        /// </summary>
        /// <param name="ptr">内存句柄</param>
        /// <param name="maxSize">最大容量</param>
        /// <param name="useageSize">已使用大小</param>
        public UnmanagedCacheStub(IntPtr ptr, int maxSize, int useageSize)
            : this(ptr, maxSize, useageSize, DateTime.MaxValue)
        {
        }

        /// <summary>
        ///     非托管缓存存根，提供了相关的基本操作
        /// </summary>
        /// <param name="ptr">内存句柄</param>
        /// <param name="maxSize">最大容量</param>
        /// <param name="useageSize">已使用大小</param>
        /// <param name="expireTime">过期时间</param>
        public UnmanagedCacheStub(IntPtr ptr, int maxSize, int useageSize, DateTime expireTime)
        {
            _fixed = false;
            _innerCache = new UnmanagedCacheItem(ptr, maxSize, useageSize);
            _lease = new CacheLease(expireTime);
        }

        #endregion

        #region Implementation of ICacheStub<byte[]>

        private bool _fixed;
        protected int _maxSize;
        protected ICacheLease _lease;
        protected IUnmanagedCacheItem _innerCache;

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
            set { _fixed = value; }
        }

        /// <summary>
        ///     获取或设置缓存项
        /// </summary>
        public ICacheItem<byte[]> Cache
        {
            get { return _innerCache; }
            set { _innerCache = (IUnmanagedCacheItem) value; }
        }

        /// <summary>
        ///     获取缓存生命周期
        /// </summary>
        /// <returns></returns>
        public ICacheLease GetLease()
        {
            return _lease;
        }

        /// <summary>
        ///     获取缓存数据
        /// </summary>
        /// <returns>返回缓存内容</returns>
        public byte[] GetValue()
        {
            if (_lease.IsDead)
            {
                //lazy clear.
                _innerCache.Free();
                throw new System.Exception("Cannot get value from a dead cache stub.");
            }
            return _innerCache.GetValue();
        }

        /// <summary>
        ///     设置缓存数据
        /// </summary>
        /// <param name="data">要设置的数据</param>
        public void SetValue(byte[] data)
        {
            if (_lease.IsDead)
            {
                //lazy clear.
                _innerCache.Free();
                throw new System.Exception("Cannot set value to a dead cache stub.");
            }
            _innerCache.SetValue(data);
        }

        #endregion

        #region Implementation of IUnmanagedCacheStub

        /// <summary>
        ///     获取内存句柄
        /// </summary>
        IntPtr IUnmanagedCacheStub.Handle
        {
            get { return _innerCache.Handle; }
        }

        /// <summary>
        ///     获取当前内部的缓存使用量
        /// </summary>
        public int CurrentSize
        {
            get { return _innerCache.UsageSize; }
        }

        /// <summary>
        ///     获取当前缓存的最大容量
        /// </summary>
        public int MaxSize
        {
            get { return _innerCache.MaxSize; }
        }

        /// <summary>
        ///     获取内存句柄
        /// </summary>
        IntPtr IUnmanagedCacheSlot.Handle
        {
            get { return _innerCache.Handle; }
        }

        /// <summary>
        ///     放弃当前缓存
        /// </summary>
        public void Discard()
        {
            _lease.Discard();
            _innerCache.Free();
        }

        #endregion
    }
}