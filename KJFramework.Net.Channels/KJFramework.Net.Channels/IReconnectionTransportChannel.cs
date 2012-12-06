namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     支持断线重连的传输通道员接口，提供了相关的基本操作。
    /// </summary>
    public interface IReconnectionTransportChannel : ITransportChannel
    {
        /// <summary>
        ///     重新尝试建立连接
        /// </summary>
        /// <returns>返回尝试后的状态</returns>
        bool Reconnect();
    }
}