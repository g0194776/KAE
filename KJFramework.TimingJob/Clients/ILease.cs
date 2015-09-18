using System;

namespace KJFramework.TimingJob.Clients
{
	/// <summary>
	/// 租约接口
	/// </summary>
	public interface ILease
	{
		#region Members

		/// <summary>
		/// 租约属于key
		/// </summary>
		string Key { get; }

		/// <summary>
		/// 该key是否为处于倒计时状态
		/// </summary>
		bool CanTimeout{ get; }

		/// <summary>
		/// 获取超时时间
		/// </summary>
		DateTime ExpireTime { get; }

		#endregion

		#region Methods.

		/// <summary>
		/// 对指定key进行生命周期的覆盖设置
		/// </summary>
		/// <param name="timeSpan">租约时间间隔</param>
		void Renew(TimeSpan timeSpan);

		#endregion
	}
}
