using System;

namespace KJFramework.TimingJob.Clients
{
	/// <summary>
	/// 租约对象
	/// </summary>
	public class Lease : ILease
	{
		#region Constructor.

		/// <summary>
		///	构造函数
		/// </summary>
		/// <param name="key">被操作的key</param>
		/// <param name="client">redisClient实例</param>
		public Lease(string key, RedisClient client)
		{
			Key = key;
			Client = client;
			_expireTime = DateTime.MaxValue;
		}

		#endregion

		#region Members.

		protected RedisClient Client;
		private DateTime _expireTime;

		/// <summary>
		/// 租约所属的key
		/// </summary>
		public string Key { get; private set; }

		/// <summary>
		/// 获取key的租约是否处于倒计时状态。
		/// 当设置false时会将key的生存时间设置为永久
		/// </summary>
		public bool CanTimeout
		{
			get
			{
				if (Client.TimeToLive(Key) == null) return false;
				return true;
			}
			set { if (value == false) Client.Persist(Key); }
		}

		/// <summary>
		/// 获取租约到期时间
		/// </summary>
		public DateTime ExpireTime
		{
			get
			{
				TimeSpan? timeSpan = Client.TimeToLive(Key);
				if (timeSpan != null)
				{
					_expireTime = DateTime.Now.Add((TimeSpan)timeSpan);
				}
				return _expireTime;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// 对可以设置失效时间。会覆盖之前的失效时间。
		/// </summary>
		/// <param name="timeSpan">设置新租约的时间长度</param>
		public void Renew(TimeSpan timeSpan)
		{
			if (timeSpan == TimeSpan.MaxValue || timeSpan == TimeSpan.MinValue) return;
			if (Client.Expire(Key, timeSpan))
			{
				_expireTime = DateTime.Now.Add(timeSpan);
			}
		}

		#endregion
	}
}
