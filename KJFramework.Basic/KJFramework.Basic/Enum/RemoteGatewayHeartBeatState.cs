namespace KJFramework.Basic.Enum
{
    /// <summary>
    ///     远程网关心跳状态枚举
    /// </summary>
    public enum RemoteGatewayHeartBeatState
    {
        /// <summary>
        ///     成功
        /// </summary>
        Successful,
        /// <summary>
        ///     失败
        /// </summary>
        Fail,
        /// <summary>
        ///     黑名单，堵塞中
        /// </summary>
        Block
    }
}