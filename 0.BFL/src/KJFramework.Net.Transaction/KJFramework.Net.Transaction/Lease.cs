using System;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     生命周期租约，提供了相关的基本操作
    /// </summary>
    public sealed class Lease : ILease
    {
        #region Constructor

        /// <summary>
        ///     生命周期租约，提供了相关的基本操作
        /// </summary>
        public Lease() : this(DateTime.MaxValue)
        {
            
        }

        /// <summary>
        ///     生命周期租约，提供了相关的基本操作
        /// </summary>
        /// <param name="expireTime">过期时间</param>
        public Lease(DateTime expireTime)
        {
            _createTime = DateTime.Now;
            _expireTime = expireTime;
            _canTimeout = _expireTime != DateTime.MaxValue;
        }

        #endregion

        #region Implementation of ICacheLease

        private bool _canTimeout;
        private bool _isDead;
        private DateTime _createTime;
        private DateTime _expireTime;

        /// <summary>
        ///     获取或设置一个值，该值表示了当前的缓存是否支持超时检查
        ///     <para>* 如果CanTimeout = false, 则ExpireTime = max(DateTime)</para>
        /// </summary>
        public bool CanTimeout
        {
            get { return _canTimeout; }
            set { _canTimeout = value; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前的缓存是否已经处于死亡的状态
        /// </summary>
        public bool IsDead
        {
            get { return (_isDead = DateTime.Now >= _expireTime); }
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
        ///     变更当前生命租期的时间
        /// </summary>
        /// <param name="dateTime">死亡时间</param>
        public void Change(DateTime dateTime)
        {
            _expireTime = dateTime;
            _canTimeout = _expireTime != DateTime.MaxValue;
        }

        /// <summary>
        ///     将当前缓存的生命周期置为死亡状态
        /// </summary>
        public void Discard()
        {
            _expireTime = DateTime.MinValue;
            _isDead = true;
        }

        /// <summary>
        ///     将当前租期续约一段时间
        /// </summary>
        /// <param name="timeSpan">续约时间</param>
        /// <returns>返回续约后的到期时间</returns>
        /// <exception cref="System.Exception">更新失败</exception>
        public DateTime Renew(TimeSpan timeSpan)
        {
            if (IsDead) throw new System.Exception("Can not renew a dead lease.");
            return _canTimeout ? (_expireTime = _expireTime.Add(timeSpan)) : _expireTime;
        }

        #endregion

        #region Members

        /// <summary>
        ///     表示一个已经死亡的租赁契约
        /// </summary>
        public static readonly ILease DeadLease = new Lease(DateTime.MinValue);
        /// <summary>
        ///     表示一个永远存货的租赁契约
        /// </summary>
        public static readonly ILease ALiveLease = new Lease(DateTime.MaxValue);

        #endregion
    }
}