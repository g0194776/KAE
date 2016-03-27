namespace KJFramework.Data.Synchronization.Enums
{
    /// <summary>
    ///     注销订阅模式枚举
    /// </summary>
    public enum UnSubscribeMode : byte
    {
        /// <summary>
        ///     订阅者主动关闭与发布者的连接
        /// </summary>
        Initiative = 0x00,
        /// <summary>
        ///     发布者关闭订阅者的连接
        /// </summary>
        KickOff = 0x01
    }
}