namespace KJFramework.Data.Synchronization.Enums
{
    /// <summary>
    ///     订阅结果枚举
    /// </summary>
    public enum SubscribeResult : byte
    {
        /// <summary>
        ///     允许
        /// </summary>
        Allow = 0x00,
        /// <summary>
        ///     拒绝
        /// </summary>
        Reject = 0x01
    }
}