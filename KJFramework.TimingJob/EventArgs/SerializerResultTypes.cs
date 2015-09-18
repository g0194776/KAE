namespace KJFramework.TimingJob.EventArgs
{
    /// <summary>
    ///    序列化器操作结果枚举
    /// </summary>
    public enum SerializerResultTypes : byte
    {
        /// <summary>
        ///    操作成功
        /// </summary>
        Succeed = 0x00,
        /// <summary>
        ///    操作成功 (当前操作被忽略，可继续执行后续操作)
        /// </summary>
        Ignored = 0x01,
        /// <summary>
        ///    操作失败，需要抛弃后续操作
        /// </summary>
        Failed = 0x02
    }
}