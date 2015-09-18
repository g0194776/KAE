namespace KJFramework.TimingJob.Enums
{
	/// <summary>
	/// Redis列表操作的方向枚举
	/// </summary>
	public enum PushTypes : byte
	{
		/// <summary>
		///	从list左端操作
		/// </summary>
		Left = 0x00,
		/// <summary>
		/// 从list右端操作
		/// </summary>
		Right = 0x01
	}
}
