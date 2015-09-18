using System;
using System.Collections.Generic;
using KJFramework.TimingJob.Enums;

namespace KJFramework.TimingJob.Clients
{
	/// <summary>
	/// Redis列表对象。在实例化和调用FetchAll时会从远端获取整个列表对象，缓存在本地。
	/// </summary>
	public class RedisList : Lease
	{
		#region Constructor.

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="key">列表对应的key</param>
		/// <param name="client">RedisClient实例</param>
		public RedisList(string key, RedisClient client) : base(key, client)
		{
			FetchAll();
		}

		#endregion

		#region Members.

		private List<string> _list;

		/// <summary>
		/// 获取列表长度
		/// </summary>
		public uint Length
		{
			get
			{
				if (_list == null) return 0;
				return (uint)_list.Count;
			}
		}

		/// <summary>
		/// 索引范围超过本地list缓存的索引范围时，会重新从远端获取一次列表内容。对指定索引处修改也会自动更新本地缓存。
		/// </summary>
		/// <param name="index">指定索引值</param>
		/// <returns>获取指定索引处字符串</returns>
		/// <exception cref="ArgumentOutOfRangeException">index out of range</exception>
		public string this[int index]
		{
			get
			{
				if ( index+1 > Length)
				{
					FetchAll();
					if (index + 1 > Length) throw new ArgumentOutOfRangeException("index");
				}
				return _list[index];
			}
			set
			{
				RedisClient redisClient = Client;
				if (redisClient != null) redisClient.ListSet(Key, value, index);
				FetchAll();
			}
		}

		#endregion

		#region Methods.

		/// <summary>
		/// 向list加入一个元素，默认右端。插入后会自动更新本地缓存。
		/// </summary>
		/// <param name="value">待插入的字符串</param>
		/// <param name="pushTypes">设置插入的端</param>
		public void Add(string value, PushTypes pushTypes = PushTypes.Right)
		{
			if (Key != null) Client.Push(Key, value, pushTypes);
			FetchAll();
		}

		/// <summary>
		/// 获取远端list
		/// </summary>
		public void FetchAll()
		{
			if (Client != null) _list = Client.GetAllList(Key);
		}

		/// <summary>
		/// 获取字符串形式列表
		/// </summary>
		/// <returns>返回字符串形式列表</returns>
		public List<string> GetList()
		{
			return _list;
		}

		#endregion
	}
}
