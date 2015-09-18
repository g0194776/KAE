namespace KJFramework.TimingJob.Enums
{
    /// <summary>
    ///    任务池类型
    /// </summary>
    public enum TaskPoolTypes : byte
    {
        /// <summary>
        ///    只读
        /// </summary>
        ReadOnly = 0x00,
        /// <summary>
        ///    只写
        /// </summary>
        WriteOnly = 0x01,
        /// <summary>
        ///    读写
        /// </summary>
        ReadWrite = 0x02
    }
}