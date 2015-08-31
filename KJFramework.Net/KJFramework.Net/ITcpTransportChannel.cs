using System.Net.Sockets;

namespace KJFramework.Net
{
    /// <summary>
    ///     基于TCP协议的传输通道元接口，提供了相关的基本操作。
    /// </summary>
    internal interface ITcpTransportChannel : ITransportChannel
    {
        /// <summary>
        ///     获取内部核心套接字
        /// </summary>
        /// <returns>返回内部核心套接字</returns>
        Socket GetStream();
        /// <summary>
        ///     获取当前TCP协议传输通道的唯一键值
        /// </summary>
        int ChannelKey { get; }
    }
}