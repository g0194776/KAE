namespace KJFramework.TimingJob.Enums
{
    /// <summary>
    ///    任务池状态枚举
    /// </summary>
    public enum TaskPoolStates : byte
    {
        /// <summary>
        ///    已开启
        /// </summary>
        Started = 0x00,
        /// <summary>
        ///    已暂停
        /// </summary>
        Paused = 0x01,
        /// <summary>
        ///    已停止
        /// </summary>
        Stoped = 0x02
    }
}