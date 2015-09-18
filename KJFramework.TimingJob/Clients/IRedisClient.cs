using System;
using KJFramework.TimingJob.Enums;

namespace KJFramework.TimingJob.Clients
{
	/// <summary>
	/// RedisClient接口
	/// </summary>
	public interface IRedisClient
	{
		#region Methods.

		/// <summary>
		/// 获取指定字符串
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <returns>返回key对应的字符串。key不存在，返回null。</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		/// <exception cref="TypeAccessException">redis中key类型不为string</exception>>
		string Get(string key);


		/// <summary>
		/// 获取指定字符串和其租约对象
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <param name="lease">租约对象</param>
		/// <returns>返回字符串。key不存在，返回null。</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		/// <exception cref="TypeAccessException">redis中key类型不为string</exception>
		string Get(string key, out ILease lease);

		/// <summary>
		/// 设置一个字符串
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <param name="value">字符串内容</param>
		/// <returns>返回关于key的租约对象</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		ILease Set(string key, string value);

		/// <summary>
		/// 向指定列表插入字符串
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <param name="value">待插入列表的字符串</param>
		/// <returns>返回Redis列表对象</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		void Push(string key, string value, PushTypes type = PushTypes.Right);

		/// <summary>
		/// 获取列表对象
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <returns>返回Redis列表对象</returns>
		/// <exception cref="ArgumentNullException">参数不能为空</exception>
		/// <exception cref="TypeAccessException">远端key类型不为List</exception>
		RedisList GetList(string key);

		/// <summary>
		/// 删除指定key
		/// </summary>
		/// <param name="key">待删除的key</param>
		void Delete(string key);

		/// <summary>
		///	计数器递增操作
		/// </summary>
		/// <param name="key">key字符串</param>
		/// <param name="value">递增值</param>
		/// <returns>递增操作后的结果值</returns>
		long Increase(string key, long value = 1);

		#endregion
	}
}
