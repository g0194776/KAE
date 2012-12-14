namespace KJFramework.Data.Synchronization.Enums
{
    /// <summary>
    ///     远程数据广播者状态枚举
    /// </summary>
    public enum BroadcasterState
    {
        /// <summary>
        ///     已连接
        /// </summary>
        Connected,
        /// <summary>
        ///     正在重联当中
        /// </summary>
        Reconnecting,
        /// <summary>
        ///     已断开
        /// </summary>
        Disconnected
    }
}