using System;
using System.Net;
using System.Net.Sockets;
using KJFramework.Buffers;
using KJFramework.EventArgs;
using KJFramework.Net.Enums;
using KJFramework.Net.Events;

namespace KJFramework.Net
{
    /// <summary>
    ///     传输通道抽象父类，提供了相关的基本操作。
    /// </summary>
    public abstract class TransportChannel : ServiceChannel, IRawTransportChannel
    {
        #region Constructor
        
        /// <summary>
        ///     传输通道抽象父类，提供了相关的基本操作。
        /// </summary>
        protected TransportChannel()
        {
        }

        #endregion

        #region Implementation of ITransportChannel

        protected IPEndPoint _address;
        protected Uri.Uri _logicalAddress;
        protected bool _connected;
        protected bool _supportSegment;
        protected IByteArrayBuffer _buffer;
        protected LingerOption _lingerState;

        /// <summary>
        ///   获取通信信道的类型
        /// </summary>
        public abstract TransportChannelTypes ChannelType { get; }

        /// <summary>
        ///     获取本地终结点地址
        /// </summary>
        public abstract EndPoint LocalEndPoint { get; }
        /// <summary>
        ///     获取远程终结点地址
        /// </summary>
        public abstract EndPoint RemoteEndPoint { get; }

        /// <summary>
        ///   获取缓冲区
        /// </summary>
        public IByteArrayBuffer Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        /// <summary>
        ///     获取或设置延迟设置
        /// </summary>
        public virtual LingerOption LingerState
        {
            get { return _lingerState; }
            set { _lingerState = value; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前通道是否处于连接状态
        /// </summary>
        public virtual bool IsConnected
        {
            get { return _connected; }
        }
        /// <summary>
        ///     连接
        /// </summary>
        public abstract void Connect();
        /// <summary>
        ///     断开
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        ///     发送数据
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <returns>返回发送的字节数</returns>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public int Send(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            return InnerSend(data);
        }

        /// <summary>
        ///     发送数据
        ///     <para>* 如果此方法进行发送的元数据，可能是自动分包后的数据。</para>
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <returns>返回发送的字节数</returns>
        protected abstract int InnerSend(byte[] data);

        /// <summary>
        ///     通道已连接事件
        /// </summary>
        public event EventHandler Connected;
        protected void ConnectedHandler(System.EventArgs e)
        {
            EventHandler connected = Connected;
            if (connected != null) connected(this, e);
        }
        /// <summary>
        ///     通道已断开事件
        /// </summary>
        public event EventHandler Disconnected;
        protected void DisconnectedHandler(System.EventArgs e)
        {
            EventHandler disconnected = Disconnected;
            if (disconnected != null) disconnected(this, e);
        }

        #endregion

        #region Implementation of ICommunicationChannelAddress

        /// <summary>
        ///     获取或设置物理地址
        /// </summary>
        public IPEndPoint Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        ///     获取或设置逻辑地址
        /// </summary>
        public Uri.Uri LogicalAddress
        {
            get { return _logicalAddress; }
            set { _logicalAddress = value; }
        }

        #endregion

        #region Implementation of IRawTransportChannel

        /// <summary>
        ///     获取或设置当前元数据信道是否支持以片段的方式接受网络流数据
        /// </summary>
        public bool SupportSegment
        {
            get { return _supportSegment; }
            set { _supportSegment = value; }
        }

        /// <summary>
        ///     接收到数据事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<byte[]>> ReceivedData;

        /// <summary>
        ///     接收到数据片段事件
        /// </summary>
        public event EventHandler<SegmentReceiveEventArgs> ReceivedDataSegment;
        protected void ReceivedDataSegmentHandler(SegmentReceiveEventArgs e)
        {
            EventHandler<SegmentReceiveEventArgs> handler = ReceivedDataSegment;
            if (handler != null) handler(this, e);
        }

        protected void ReceivedDataHandler(LightSingleArgEventArgs<byte[]> e)
        {
            EventHandler<LightSingleArgEventArgs<byte[]>> handler = ReceivedData;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}