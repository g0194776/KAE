using KJFramework.Cache.Cores;
using KJFramework.Net.Channels.Caches;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.Events;
using KJFramework.Net.Channels.Transactions;
using KJFramework.Net.Channels.Uri;
using KJFramework.Tracing;
using System;
using System.IO.Pipes;
using System.Net;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///    基于IPC通道的传输通道，提供了相关的基本操作。
    /// </summary>
    public class PipeTransportChannel : TransportChannel, IReconnectionTransportChannel
    {
        #region Members.

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(PipeTransportChannel));

        /// <summary>
        ///    获取内部流对象
        /// </summary>
        public PipeStream Stream
        {
            get { return _stream; }
        }

        /// <summary>
        ///     获取本地终结点地址
        /// </summary>
        public override EndPoint LocalEndPoint
        {
            get { return new DnsEndPoint("http://www.126.com", 0); }
        }

        /// <summary>
        ///     获取发出请求的客户端 IP 地址和端口号
        /// </summary>
        public override EndPoint RemoteEndPoint
        {
            get { return new DnsEndPoint("http://www.126.com", 0); }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前通道是否处于连接状态
        /// </summary>
        public override bool IsConnected
        {
            get { return (_connected = _stream != null && _stream.IsConnected); }
        }

        protected PipeStream _stream;
        protected PipeStreamTransaction _streamAgent;
        private readonly Action<IFixedCacheStub<BuffStub>, int> _callback;

        #endregion

        #region Constructor.

        /// <summary>
        ///    基于IPC通道的传输通道，提供了相关的基本操作。
        ///     <para>* 此构造函数用于初始化一个需要连接到远程的命名管道对象</para>
        /// </summary>
        /// <param name="logicalUri">通道地址</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public PipeTransportChannel(PipeUri logicalUri)
        {
            if (logicalUri == null) throw new ArgumentNullException("logicalUri");
            _logicalAddress = logicalUri;
            _callback = DefaultCallback;
            _supportSegment = true;
        }

        /// <summary>
        ///    基于IPC通道的传输通道，提供了相关的基本操作。
        ///     <para>* 此构造函数用于初始化一个已经连接的命名管道数据流</para>
        /// </summary>
        /// <param name="stream" type="System.IO.Pipes.PipeStream">PIPE流</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public PipeTransportChannel(PipeStream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            _stream = stream;
            _supportSegment = true;
            _callback = DefaultCallback;
            _connected = stream.IsConnected;
            _streamAgent = new PipeStreamTransaction(_stream, stream.IsAsync, _callback);
            _streamAgent.Disconnected += TransactionDisconnected;
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
            if(!_connected) Connect();
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
            get { return TransportChannelTypes.NamedPipe; }
        }

        /// <summary>
        ///     连接到远程命名管道
        ///     <para>* 此方法将会使用双向数据流的方式连接远程命名管道</para>
        /// </summary>
        public override void Connect()
        {
            Connect(PipeDirection.InOut);
        }

        /// <summary>
        ///     连接到远程命名管道
        /// </summary>
        /// <param name="direction">数据流方向</param>
        /// <param name="milliseconds">超时时间</param>
        /// <exception cref="ArgumentException">无效的超时时间</exception>
        /// <exception cref="InvalidOperationException">无法再次调用一个已经初始化后的Channel的Connect方法</exception>
        public virtual void Connect(PipeDirection direction, int milliseconds = 1000)
        {
            try
            {
                if (milliseconds == 0)throw new ArgumentException("#You cannot specified ZERO to timeout value.");
                if (_logicalAddress == null) throw new System.Exception("#You didn't offer any address which currently went to connect.");
                if (_stream != null && _stream.IsConnected) throw new InvalidOperationException("#You cannot did this operation again, because of current channel had been done with the conenction since it created.");
                PipeUri uri = new PipeUri(_logicalAddress.ToString());
                NamedPipeClientStream stream = new NamedPipeClientStream(uri.MachineName, uri.PipeName, direction, PipeOptions.Asynchronous);
                _communicationState = CommunicationStates.Opening;
                stream.Connect(milliseconds);
                _stream = stream;
                _connected = _stream.IsConnected;
                _streamAgent = new PipeStreamTransaction(_stream, stream.IsAsync, _callback);
                _streamAgent.Disconnected += TransactionDisconnected;
                _communicationState = CommunicationStates.Opened;
                ConnectedHandler(null);
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
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
                if (_streamAgent != null)
                {
                    _streamAgent.EndWork();
                    _streamAgent = null;
                }
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
            finally
            {
                _communicationState = CommunicationStates.Closed;
                DisconnectedHandler(null);
            }
        }

        /// <summary>
        ///     重新尝试建立连接
        /// </summary>
        /// <returns>返回尝试后的状态</returns>
        [Obsolete("#You cannot did this operation on current type of Transport Channel.", true)]
        public bool Reconnect()
        {
            throw new NotImplementedException("#You cannot did this operation on current type of Transport Channel.");
        }

        /// <summary>
        ///     发送数据
        ///     <para>* 如果此方法进行发送的元数据，可能是自动分包后的数据。</para>
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <returns>返回发送的字节数</returns>
        protected override int InnerSend(byte[] data)
        {
            try
            {
                if (data.Length > ChannelConst.MaxMessageDataLength)
                {
                    _tracing.Warn(string.Format("#Illegal data size: {0}, current allow size: {1}", data.Length, ChannelConst.MaxMessageDataLength));
                    return -1;
                }
                //判断流状态
                if (_stream == null || !_stream.IsConnected)
                {
                    InnerClose();
                    return -2;
                }
                if (!_stream.CanWrite)
                {
                    _tracing.Warn("#You were trying to write some binary data into a read-only Named Pipe channel.", null);
                    return -3;
                }
                _stream.BeginWrite(data, 0, data.Length, SendCallback, null);
                return data.Length;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                InnerClose();
                return -2;
            }
        }

        #endregion

        #region Events.

        //事物断开事件
        void TransactionDisconnected(object sender, System.EventArgs e)
        {
            if (_streamAgent != null)
            {
                _streamAgent.Disconnected -= TransactionDisconnected;
                _streamAgent = null;
            }
            _connected = false;
            _communicationState = CommunicationStates.Closed;
            DisconnectedHandler(null);
        }

        #endregion

        #region Members.

        //callback function.
        protected virtual void DefaultCallback(IFixedCacheStub<BuffStub> stub, int bytesTransferred)
        {
            ReceivedDataSegmentHandler(new NamedPipeSegmentReceiveEventArgs(stub, bytesTransferred));
        }

        //send async callback.
        private void SendCallback(IAsyncResult result)
        {
            try
            {
                _stream.EndWrite(result);
                _stream.Flush();
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                InnerClose();
            }
        }

        #endregion
    }
}