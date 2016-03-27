using System;
using KJFramework.Containers;
using KJFramework.Cores;
using KJFramework.EventArgs;
using KJFramework.Net.Caches;

namespace KJFramework.Net.Managers
{
    /// <summary>
    ///     封包片管理器，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public abstract class MultiPacketManager<T> : IMultiPacketManager<T>
    {
        #region Constructor

        /// <summary>
        ///     封包片管理器，提供了相关的基本操作
        /// </summary>
        public MultiPacketManager()
        {
            _caches.CacheExpired += CacheExpired;
        }

        #endregion

        #region Members

        private static readonly ICacheTenant _tenant = new CacheTenant();
        private readonly ICacheContainer<int, IMultiPacketStub<T>> _caches = _tenant.Rent<int, IMultiPacketStub<T>>("Cache:MultiPacketManager:" + Guid.NewGuid());

        #endregion

        #region Implementation of IMultiPacketManager<T>

        /// <summary>
        ///     添加一个封包片
        /// </summary>
        /// <param name="key">唯一消息Id</param>
        /// <param name="message">封包片</param>
        /// <param name="maxPacketCount">
        ///     最大封包片数
        ///     <para>* 第一次调用时设置此值，以后默认传-1即可。</para>
        /// </param>
        /// <returns>如果返回值不为null, 则证明已经拼接为一个完整的消息</returns>
        public T Add(int key, T message, int maxPacketCount = -1)
        {
            return Add(key, message, new TimeSpan(0, 1, 0), maxPacketCount);
        }

        /// <summary>
        ///     添加一个封包片
        /// </summary>
        /// <param name="key">唯一消息Id</param>
        /// <param name="message">封包片</param>
        /// <param name="timeSpan">过期时间</param>
        /// <param name="maxPacketCount">
        ///     最大封包片数
        ///     <para>* 第一次调用时设置此值，以后默认传-1即可。</para>
        /// </param>
        /// <returns>如果返回值不为null, 则证明已经拼接为一个完整的消息</returns>
        public T Add(int key, T message, TimeSpan timeSpan, int maxPacketCount = -1)
        {
            IReadonlyCacheStub<IMultiPacketStub<T>> stub = _caches.Get(key);
            if (stub != null)
                return stub.Cache.AddPacket(message) ? PickupMessage(stub.Cache) : default(T);
            IMultiPacketStub<T> multiPacketStub = new MultiPacketStub<T>(key, maxPacketCount);
            multiPacketStub.AddPacket(message);
            _caches.Add(key, multiPacketStub, timeSpan);
            return default(T);
        }

        /// <summary>
        ///     封包消息过期事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<IMultiPacketStub<T>>> Expired;
        protected void ExpiredHandler(LightSingleArgEventArgs<IMultiPacketStub<T>> e)
        {
            EventHandler<LightSingleArgEventArgs<IMultiPacketStub<T>>> handler = Expired;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     将一系列的封包片组合成一个完整的消息
        /// </summary>
        /// <param name="stub">封包片存根</param>
        /// <returns>返回一个完整的消息</returns>
        protected abstract T PickupMessage(IMultiPacketStub<T> stub);

        #endregion

        #region Events

        //cache expired.
        void CacheExpired(object sender, ExpiredCacheEventArgs<int, IMultiPacketStub<T>> e)
        {
            ExpiredHandler(new LightSingleArgEventArgs<IMultiPacketStub<T>>(e.Obj));
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _caches.CacheExpired -= CacheExpired;
        }

        #endregion
    }
}