namespace KJFramework.Data.Synchronization.Enums
{
    /// <summary>
    ///     订阅者状态枚举
    /// </summary>
    public enum SubscriberState
    {
        /// <summary>
        ///     已连接
        /// </summary>
        Connected,
        /// <summary>
        ///     等待重连
        /// </summary>
        WaitReconnect,
        /// <summary>
        ///     已断开
        /// </summary>
        Disconnected,
        /// <summary>
        ///     出现异常
        /// </summary>
        Exception,
        /// <summary>
        ///     已成功订阅
        /// </summary>
        Subscribed,
        /// <summary>
        ///     正在订阅中
        /// </summary>
        ToBeSubscribe,
        /// <summary>
        ///     未知
        /// </summary>
        Unknown
    }
}