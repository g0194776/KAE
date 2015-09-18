using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.TimingJob.Enums;
using StackExchange.Redis;

namespace KJFramework.TimingJob.Clients
{
	/// <summary>
	/// redis客户端类
	/// </summary>
	public class RedisClient : IRedisClient
	{
		#region Members.

		private readonly IDatabase _database;
		private static uint _keyTtl;

		#endregion

		#region Constructor

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="connector">redis的连接地址与端口。例如redis0:6379,redis1:6379</param>
		/// <param name="ttl">key生存时间，默认为1小时</param>
		public RedisClient(string connector, uint ttl = 1)
		{
			if (string.IsNullOrEmpty(connector)) throw new ArgumentNullException(nameof(connector));
			_keyTtl = ttl;
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connector);
			_database = redis.GetDatabase();
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="connector">redis的连接地址与端口(ip:port)，port默认为6379</param>
		/// <param name="timeout">连接超时时间，单位为milliseconds</param>
		/// <param name="ttl">设置默认生存时间间隔，单位为小时</param>
		public RedisClient(string connector, uint timeout, uint ttl)
		{
			if (string.IsNullOrEmpty(connector)) throw new ArgumentNullException("connector");
			ConfigurationOptions options = ConfigurationOptions.Parse(connector);
			options.ConnectTimeout = (int)timeout;
			_keyTtl = ttl;
			ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(options);
			_database = redis.GetDatabase();
		}

		#endregion

		#region Implementation of IRedisOperator

		/// <summary>
		/// 获取指定字符串
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <returns>返回key对应的字符串。key不存在，返回null。</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		/// <exception cref="TypeAccessException">redis中key类型不为string</exception>
		public string Get(string key)
		{
			if (key == null) throw new ArgumentNullException("key");
			RedisType type = _database.KeyType(key);
			if (type == RedisType.None) return null;
			if (type != RedisType.String)
			{
				throw new TypeAccessException(string.Format("not a string type, key:{0} type:{1}", key, type));
			}
			return _database.StringGet(key);
		}

		/// <summary>
		/// 获取指定字符串和其租约对象
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <param name="lease">租约对象</param>
		/// <returns>返回字符串。key不存在，返回null。</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		/// <exception cref="TypeAccessException">redis中key类型不为string</exception>
		public string Get(string key, out ILease lease)
		{
			string value = Get(key);
			if (value != null)
			{
				lease = new Lease(key, this);
				return _database.StringGet(key);
			}
			lease = null;
			return null;
		}

		/// <summary>
		/// 设置一个字符串
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <param name="value">字符串内容</param>
		/// <returns>返回关于key的租约对象</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		public ILease Set(string key, string value)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (value == null) throw new ArgumentNullException("value");
			return Set(key, value, TimeSpan.FromHours(_keyTtl));
		}

		/// <summary>
		/// 向指定列表插入指定字符串
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <param name="value">待插入字符串</param>
		/// <param name="pushTypes">PushTypes可选择插入列表的端</param>
		/// <returns>返回Redis列表对象</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		public void Push(string key, string value, PushTypes pushTypes = PushTypes.Right)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (value == null) throw new ArgumentNullException("value");
			if (pushTypes == PushTypes.Left)
			{
				_database.ListLeftPush(key, value);
			}
			else if (pushTypes == PushTypes.Right)
			{
				_database.ListRightPush(key, value);
			}
			_database.KeyExpire(key, TimeSpan.FromHours(_keyTtl));
		}

		/// <summary>
		/// 获取列表对象
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <returns>返回Redis列表对象</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		/// <exception cref="TypeAccessException">远端key类型不为List</exception>
		public RedisList GetList(string key)
		{
			if (key == null) throw new ArgumentNullException("key");
			RedisType type = _database.KeyType(key);
			if (type == RedisType.None) return null;
			if (type != RedisType.List)
			{
				throw new TypeAccessException(string.Format("Not a list type, key:{0} type:{1}", key, type));
			}
			return new RedisList(key, this);
		}

		/// <summary>
		/// 删除指定key
		/// </summary>
		/// <param name="key">key字符串</param>
		public void Delete(string key)
		{
			_database.KeyDelete(key, CommandFlags.FireAndForget);
		}

		/// <summary>
		///	计数器递增操作
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <param name="value">递增值</param>
		/// <returns>递增操作后的结果值</returns>
		public long Increase(string key, long value = 1)
		{
			if (key == null) throw new ArgumentNullException("key");
			long result = _database.StringIncrement(key, value, CommandFlags.FireAndForget);
			_database.KeyExpire(key,TimeSpan.FromHours(_keyTtl), CommandFlags.FireAndForget);
			return result;
		}
		#endregion

		#region instance method

		/// <summary>
		/// 设置一个字符串和其声明周期间隔
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <param name="value">字符串内容</param>
		/// <param name="ttl">声明周期间隔</param>
		/// <returns>返回关于key的租约对象</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		public ILease Set(string key, string value, TimeSpan ttl)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (value == null) throw new ArgumentNullException("value");
			if (ttl.Equals(TimeSpan.Zero))
			{
				_database.StringSet(key, value);
			}
			else
			{
				_database.StringSet(key, value, ttl);
			}
			ILease lease = new Lease(key, this);
			return lease;
		}

		/// <summary>
		/// 获取列表中指定索引处的字符串
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <param name="index">列表中的索引值</param>
		/// <returns>返回列表中指定索引处的字符串</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		internal string GetByIndex(string key, int index)
		{
			if (key == null) throw new ArgumentNullException("key");
			return _database.ListGetByIndex(key, index);
		}

		/// <summary>
		/// 对key设置生存时间
		/// </summary>
		/// <param name="key">被操作key</param>
		/// <param name="ttl">时间间隔</param>
		/// <returns>返回是否设置成功</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		internal bool Expire(string key, TimeSpan ttl)
		{
			if (key == null) throw new ArgumentNullException("key");
			return _database.KeyExpire(key, ttl);
		}

		/// <summary>
		/// 获取key的生存时间
		/// </summary>
		/// <param name="key">被操作key</param>
		/// <returns>key的生存时间间隔</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		internal TimeSpan? TimeToLive(string key)
		{
			if (key == null) throw new ArgumentNullException("key");
			return _database.KeyTimeToLive(key);
		}

		/// <summary>
		/// 将key设置为永久有效
		/// </summary>
		/// <param name="key">被操作key</param>
		/// <returns>操作是否成功</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		internal bool Persist(string key)
		{
			if (key == null) throw new ArgumentNullException("key");
			return _database.KeyPersist(key);
		}

		/// <summary>
		/// 获取一个完整List
		/// </summary>
		/// <param name="key">被操作key</param>
		/// <returns>返回一个列表</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		internal List<string> GetAllList(string key)
		{
			if (key == null) throw new ArgumentNullException("key");
			RedisValue[] l = _database.ListRange(key);
			return l.Select(element => (string) element).ToList();
		}

		/// <summary>
		/// 对List指定索引位置进行覆盖修改
		/// </summary>
		/// <param name="key">被操作key</param>
		/// <param name="value">字符串值</param>
		/// <param name="index">list的索引位置</param>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		internal void ListSet(string key, string value, int index)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (value == null) throw new ArgumentNullException("value");
			_database.ListSetByIndex(key, index, value);
		}

		#endregion
		
	}
}
