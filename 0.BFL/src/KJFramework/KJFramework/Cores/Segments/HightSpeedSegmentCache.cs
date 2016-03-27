using System;
using System.Collections.Generic;

namespace KJFramework.Cores.Segments
{
    /// <summary>
    ///     高速片段缓存，提供了相关的基本操作
    /// </summary>
    internal class HightSpeedSegmentCache : IHightSpeedSegmentCache
    {
        #region Constructor

        /// <summary>
        ///     高速片段缓存，提供了相关的基本操作
        /// </summary>
        /// <param name="lease">生命周期租约</param>
        /// <param name="realSize">内部真实缓存数据大小</param>
        public HightSpeedSegmentCache(ICacheLease lease, int realSize)
        {
            if (lease == null)
            {
                throw new ArgumentNullException("lease");
            }
            if (realSize < 0)
            {
                throw new ArgumentException("Illegal real size.");
            }
            _realSize = realSize;
            _lease = lease;
        }

        #endregion

        #region Members

        protected readonly ICacheLease _lease;
        protected IList<ISegmentCacheStub> _stubs = new List<ISegmentCacheStub>();
        private byte[] _data;
        private readonly int _realSize;

        #endregion

        #region Implementation of ILeasable

        protected bool _changed;

        /// <summary>
        ///     获取一个值，该值表示了当前的缓存是否已经处于死亡的状态
        /// </summary>
        public bool IsDead
        {
            get { return _lease.IsDead; }
        }

        /// <summary>
        ///     获取生命周期创建的时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _lease.CreateTime; }
        }

        /// <summary>
        ///     获取超时时间
        /// </summary>
        public DateTime ExpireTime
        {
            get { return _lease.ExpireTime; }
        }

        /// <summary>
        ///     将当前缓存的生命周期置为死亡状态
        /// </summary>
        public void Discard()
        {
           _lease.Discard();
        }

        /// <summary>
        ///     将当前租期续约一段时间
        /// </summary>
        /// <param name="timeSpan">续约时间</param>
        /// <returns>返回续约后的到期时间</returns>
        /// <exception cref="System.Exception">更新失败</exception>
        public DateTime Renew(TimeSpan timeSpan)
        {
            return _lease.Renew(timeSpan);
        }

        #endregion

        #region Implementation of IHightSpeedSegmentCache

        /// <summary>
        ///     获取内部真实缓存数据大小
        /// </summary>
        public int RealSize
        {
            get { return _realSize; }
        }

        /// <summary>
        ///     获取一个值，该值标示了当前的值是否已经发生了变化
        /// </summary>
        public bool Changed
        {
            get { return _changed; }
        }

        /// <summary>
        ///     打入新的片段缓存存根
        /// </summary>
        /// <param name="stub">片段缓存存根</param>
        public void Add(ISegmentCacheStub stub)
        {
            if (stub == null)
            {
                throw new ArgumentNullException("stub");
            }
            _changed = true;
            _stubs.Add(stub);
        }

        /// <summary>
        ///     获取内部所有的缓存存根
        /// </summary>
        /// <returns>返回缓存存根集合</returns>
        public IList<ISegmentCacheStub> GetStubs()
        {
            return _stubs;
        }

        /// <summary>
        ///     返回内部数据
        /// </summary>
        /// <returns>内部数据</returns>
        public byte[] GetBody()
        {
            if (!_changed) return _data;
            _data = new byte[_realSize];
            int offset = 0;
            foreach (ISegmentCacheStub segmentCacheStub in _stubs)
            {
                byte[] temp = segmentCacheStub.Cache.GetValue();
                Buffer.BlockCopy(temp, 0, _data, offset, temp.Length);
                offset += temp.Length;
            }
            _changed = false;
            return _data;
        }

        #endregion
    }
}