using System;
using System.Net;
using System.Net.Sockets;
using KJFramework.IO.Buffers;
using KJFramework.Net.Channels.Enums;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     传输通道元接口，提供了相关的基本操作。
    /// </summary>
    public interface ITransportChannel : IServiceChannel, ICommunicationChannelAddress
    {
        /// <summary>
        ///   获取通信信道的类型
        /// </summary>
        TransportChannelTypes ChannelType { get; }
        /// <summary>
        ///     获取本地终结点地址
        /// </summary>
        EndPoint LocalEndPoint { get; }
        /// <summary>
        ///     获取远程终结点地址
        /// </summary>
        EndPoint RemoteEndPoint { get; }
        /// <summary>
        ///   获取或设置缓冲区
        /// </summary>
        IByteArrayBuffer Buffer { get; set; }
        /// <summary>
        ///     获取或设置延迟设置
        /// </summary>
        LingerOption LingerState { get; set; }
        /// <summary>
        ///     获取一个值，该值表示了当前通道是否处于连接状态
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        ///     连接
        /// </summary>
        void Connect();
        /// <summary>
        ///     断开
        /// </summary>
        void Disconnect();
        /// <summary>
        ///     发送数据
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <returns>返回发送的字节数</returns>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        int Send(byte[] data);
        /// <summary>
        ///     通道已连接事件
        /// </summary>
        event EventHandler Connected;
        /// <summary>
        ///     通道已断开事件
        /// </summary>
        event EventHandler Disconnected;
    }
}