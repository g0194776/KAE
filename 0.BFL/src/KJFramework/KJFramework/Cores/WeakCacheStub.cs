using System;

namespace KJFramework.Cores
{
    /// <summary>
    ///     �����������ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    internal class WeakCacheStub<T> : ICacheStub<T>, IReadonlyCacheStub<T>
    {
        #region Constructor

        /// <summary>
        ///     �����������ṩ����صĻ�������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        public WeakCacheStub() : this(DateTime.MaxValue)
        {
            
        }

        /// <summary>
        ///     �����������ṩ����صĻ�������
        /// </summary>
        /// <param name="expireTime">����ʱ��</param>
        /// <typeparam name="T">��������</typeparam>
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
        ///     ��ȡ��ǰ���������ڲ����
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�������Ƿ��ʾΪһ�ֹ�̬�Ļ���״̬
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
        ///     ��ȡ�����û�����
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
        ///     ��ȡ�������������
        /// </summary>
        public ICacheLease Lease
        {
            get { return _lease; }
        }

        /// <summary>
        ///     ��ȡ������������
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
        ///     ��ȡ����
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