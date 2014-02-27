using KJFramework.Basic.Enum;
using KJFramework.Cache.Cores;
using KJFramework.Net.Channels.Caches;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.Events;
using KJFramework.Net.Channels.Receivers;
using KJFramework.Net.Channels.Statistics;
using KJFramework.Net.EventArgs;
using KJFramework.Statistics;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     基于TCP协议的传输通道，提供了相关的基本操作。
    ///     <para>* 此信道支持外抛元数据事件。</para>
    /// </summary>
    public class TcpTransportChannel : TransportChannel, ITcpTransportChannel, IReconnectionTransportChannel
    {
        #region 成员

        protected Socket _socket;
        protected int _channelKey;
        protected TcpAsynDataRecevier _receiver;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(TcpTransportChannel));

        /// <summary>
        ///     获取本地终结点地址
        /// </summary>
        public override EndPoint LocalEndPoint
        {
            get { return _socket.LocalEndPoint; }
        }

        /// <summary>
        ///     获取远程终结点地址
        /// </summary>
        public override EndPoint RemoteEndPoint
        {
            get { return _socket.RemoteEndPoint; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前通道是否处于连接状态
        /// </summary>
        public override bool IsConnected
        {
            get { return (_connected = _socket.Connected); }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     基于TCP协议的传输通道，提供了相关的基本操作
        /// </summary>
        /// <param name="ip">远程终结点IP地址</param>
        /// <param name="port">远程终结点端口</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public TcpTransportChannel(string ip, int port)
            : this(new IPEndPoint(IPAddress.Parse(ip), port))
        { }

        /// <summary>
        ///     基于TCP协议的传输通道，提供了相关的基本操作
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public TcpTransportChannel(IPEndPoint iep)
        {
            if (iep == null) throw new ArgumentNullException("iep");
            _supportSegment = true;
            _address = iep;
            InitializeStatistics();
        }

        /// <summary>
        ///     基于TCP协议的传输通道，提供了相关的基本操作
        /// </summary>
        /// <param name="socket" type="System.Net.Sockets.Socket">网络套接字</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public TcpTransportChannel(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            _supportSegment = true;
            _socket = socket;
            _connected = _socket.Connected;
            _channelKey = _socket.GetHashCode();
            InitializeStatistics();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     初始化统计器
        /// </summary>
        protected void InitializeStatistics()
        {
            #if (DEBUG)
            {
                _statistics = new Dictionary<StatisticTypes, IStatistic>();
                TcpTransportChannelStatistic statistic = new TcpTransportChannelStatistic();
                statistic.Initialize(this);
                _statistics.Add(StatisticTypes.Network, statistic);
            }
            #endif          
        }

        /// <summary>
        ///     初始化消息接收器
        /// </summary>
        protected void InitializeReceiver()
        {
            if (_receiver != null) return;
            _receiver = new TcpAsynDataRecevier(_socket);
            _receiver.ReceivedData += RecvData;
            _receiver.Disconnected += ReceiverDisconnected;
            _receiver.Start();
        }

        #endregion

        #region Overrides of ServiceChannel

        /// <summary>
        ///     停止
        /// </summary>
        protected override void InnerAbort()
        {
            InnerClose();
        }

        /// <summary>
        ///     打开
        /// </summary>
        protected override void InnerOpen()
        {
            if (!_connected) throw new System.Exception("Cannot open a tcp transport channel, because current channel has been disconnected.");
            InitializeReceiver();
        }

        /// <summary>
        ///     关闭
        /// </summary>
        protected override void InnerClose()
        {
            Disconnect();
        }

        #endregion

        #region Overrides of TransportChannel

        /// <summary>
        ///   获取通信信道的类型
        /// </summary>
        public override TransportChannelTypes ChannelType
        {
            get { return TransportChannelTypes.TCP; }
        }

        /// <summary>
        ///     获取或设置延迟设置
        /// </summary>
        /// <exception cref="System.Exception">无效的Socket</exception>
        public override LingerOption LingerState
        {
            get
            {
                if (!_socket.Connected)
                    throw new System.Exception("You cannot GET linger option at a disconnected socket.");
                return _lingerState = _socket.LingerState;
            }
            set
            {
                if (!_socket.Connected)
                    throw new System.Exception("You cannot SET linger option at a disconnected socket.");
                _socket.LingerState = value;
                base.LingerState = value;
            }
        }

        /// <summary>
        ///     获取内部核心套接字
        /// </summary>
        /// <returns>返回内部核心套接字</returns>
        public Socket GetStream()
        {
            return _socket;
        }

        /// <summary>
        ///     获取当前TCP协议传输通道的唯一键值
        /// </summary>
        public int ChannelKey
        {
            get { return _channelKey; }
        }

        /// <summary>
        ///     连接到远程终结点地址
        /// </summary>
        public override void Connect()
        {
            _communicationState = CommunicationStates.Opening;
            if (_socket != null)
            {
                if (_socket.Connected)
                {
                    try
                    {
                        _socket.Shutdown(SocketShutdown.Both);
                        _socket.Close();
                    }
                    catch { }
                }
                _socket = null;
            }
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                //这里连接可能会出现异常，在这个场景下不做捕获，抛出就是为了让用户知道。
                _socket.Connect(_address);
                _connected = _socket.Connected;
                if (!_connected)
                {
                    _communicationState = CommunicationStates.Closed;
                    return;
                }
                _communicationState = CommunicationStates.Opened;
                _channelKey = _socket.GetHashCode();
                InitializeReceiver();
            }
            catch
            {
                _connected = false;
                _communicationState = CommunicationStates.Closed;
            }
        }

        /// <summary>
        ///     断开
        /// </summary>
        public override void Disconnect()
        {
            try
            {
                _communicationState = CommunicationStates.Closing;
                if (_receiver != null)
                {
                    _receiver.ReceivedData -= RecvData;
                    _receiver.Disconnected -= ReceiverDisconnected;
                    _receiver.Stop();
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
            }
            finally
            {
                _receiver = null;
                _connected = false;
                _communicationState = CommunicationStates.Closed;
                DisconnectedHandler(null);
            }
        }

        /// <summary>
        ///     重新尝试建立连接
        /// </summary>
        /// <returns>返回尝试后的状态</returns>
        public bool Reconnect()
        {
            Connect();
            return _connected;
        }

        /// <summary>
        ///     发送数据
        ///     <para>* 如果此方法进行发送的元数据，可能是自动分包后的数据。</para>
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <returns>返回发送的字节数</returns>
        protected override int InnerSend(byte[] data)
        {
            int sendCount = 1;
            try
            {
                if (_socket == null || !_socket.Connected) return -1;
                if (data.Length > ChannelConst.MaxMessageDataLength)
                {
                    _tracing.Warn(string.Format("#Illegal data size: {0}, current allow size: {1}", data.Length, ChannelConst.MaxMessageDataLength));
                    return -1;
                }
                //get fixed cache.
                IFixedCacheStub<NoBuffSocketStub> stub = ChannelConst.NoBuffAsyncStubPool.Rent();
                if (stub == null)
                {
                    _tracing.Warn("Cannot rent a socket async event args cache.");
                    return -2;
                }
                stub.Tag = this;
                SocketAsyncEventArgs args = stub.Cache.Target;
                args.SetBuffer(data, 0, data.Length);
                args.UserToken = stub;
                bool result;
                using (ExecutionContext.SuppressFlow())
                    result = _socket.SendAsync(args);
                //同步发送完成
                if (!result)
                {
                    //注意，在这里如果是同步完成的，根据MSDN的解释，将不会触发Completed事件
                    sendCount = args.BytesTransferred;
                    ChannelConst.NoBuffAsyncStubPool.Giveback(stub);
                }
                return sendCount;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return sendCount;
            }
        }

        #endregion

        #region 事件

        /// <summary>
        ///   接收数据
        ///   <para>* IOCP接收数据线程重入函数</para>  
        /// </summary>
        protected virtual void RecvData(object sender, SegmentReceiveEventArgs e)
        {
            ReceivedDataSegmentHandler(e);
        }

        void ReceiverDisconnected(object sender, RecevierDisconnectedEventArgs e)
        {
            try
            {
                if (_receiver != null)
                {
                    _receiver.ReceivedData -= RecvData;
                    _receiver.Disconnected -= ReceiverDisconnected;
                    _receiver = null;
                }
            }
            catch(System.Exception ex) { _tracing.Error(ex, null); }
            finally
            {
                _connected = false;
                _communicationState = CommunicationStates.Closed;
                DisconnectedHandler(null);
            }
        }

        #endregion
    }
}