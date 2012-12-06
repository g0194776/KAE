using System;
using System.IO.Pipes;
using System.Net;
using System.Threading;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;
using KJFramework.Logger;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.Transactions;
using KJFramework.Net.Channels.Uri;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///    基于IPC通道的传输通道，提供了相关的基本操作。
    /// </summary>
    public class PipeTransportChannel : TransportChannel, IReconnectionTransportChannel
    {
        #region 成员

        protected PipeStream _stream;
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
        public override IPEndPoint LocalEndPoint
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        ///     获取发出请求的客户端 IP 地址和端口号
        /// </summary>
        public override IPEndPoint RemoteEndPoint
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前通道是否处于连接状态
        /// </summary>
        public override bool IsConnected
        {
            get { return (_connected = _client.IsConnected); }
        }

        private readonly Action<byte[]> _callback;
        protected NamedPipeClientStream _client;
        protected Thread _thread;
        protected ServerPipeStreamTransaction _servTransaction;
        protected ClientPipeStreamTransaction _clientTransaction;

        #endregion

        #region 构造函数

        /// <summary>
        ///    基于IPC通道的传输通道，提供了相关的基本操作。
        /// </summary>
        /// <param name="logicalUri">通道地址</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public PipeTransportChannel(Uri.Uri logicalUri)
        {
            if (logicalUri == null)
            {
                throw new ArgumentNullException("logicalUri");
            }
            _logicalAddress = logicalUri;
            _callback = DefaultCallback;
        }

        /// <summary>
        ///    基于IPC通道的传输通道，提供了相关的基本操作。
        /// </summary>
        /// <param name="stream" type="System.IO.Pipes.PipeStream">PIPE流</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public PipeTransportChannel(PipeStream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            _stream = stream;
            _callback = DefaultCallback;
            _connected = stream.IsConnected;
            if (_stream is NamedPipeServerStream)
            {
                InitializeServerTransaction();
            }
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
            Connect();
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
        ///     连接
        /// </summary>
        public override void Connect()
        {
            try
            {
                if (_logicalAddress == null)
                {
                    throw new System.Exception("未提供远程终端地址。");
                }
                if (_client == null)
                {
                    PipeUri uri = new PipeUri(_logicalAddress.ToString());
                    _client = new NamedPipeClientStream(uri.MachineName, uri.PipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
                }
                _communicationState = CommunicationStates.Opening;
                _client.Connect(100);
                _stream = _client;
                _connected = _client.IsConnected;
                if (!_connected)
                {
                    _communicationState = CommunicationStates.Closed;
                    return;
                }
                _communicationState = CommunicationStates.Opened;
                InitializeClientTransaction();
                ConnectedHandler(null);
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
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
                if (_servTransaction != null)
                {
                    _servTransaction.Dispose();
                    _servTransaction = null;
                }
                if (_clientTransaction != null)
                {
                    _clientTransaction.Dispose();
                    _clientTransaction = null;
                }
                if (_stream != null)
                {
                    try
                    {
                        if (_stream is NamedPipeServerStream)
                        {
                            (_stream as NamedPipeServerStream).Disconnect();
                        }
                        _stream.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Logs.Logger.Log(ex, DebugGrade.Standard, Logs.Name);
                    }
                    _stream = null;
                }
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
            }
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
        public bool Reconnect()
        {
            try
            {
                Connect();
                return _connected;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return false;
            }
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
                    Logs.Logger.Log(string.Format("#Illegal data size: {0}, current allow size: {1}", data.Length, ChannelConst.MaxMessageDataLength));
                    return -1;
                }
                //判断流状态
                if (_stream != null && _stream.CanWrite)
                {
                    _stream.BeginWrite(data, 0, data.Length, SendCallback, null);
                    return data.Length;
                }
                return -1;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex, DebugGrade.Standard, Logs.Name);
                return -1;
            }
        }

        #endregion

        #region 事件

        //事物断开事件
        void TransactionDisconnected(object sender, System.EventArgs e)
        {
            if (_servTransaction != null)
            {
                _servTransaction.Disconnected -= TransactionDisconnected;
                _servTransaction = null;
            }
            if (_clientTransaction != null)
            {
                _clientTransaction.Disconnected -= TransactionDisconnected;
                _clientTransaction = null;
            }
            _stream = null;
            _connected = false;
            _communicationState = CommunicationStates.Closed;
            DisconnectedHandler(null);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     初始化事物
        /// </summary>
        protected void InitializeServerTransaction()
        {
            _servTransaction = new ServerPipeStreamTransaction((NamedPipeServerStream)_stream, true, _callback);
            _servTransaction.Disconnected += TransactionDisconnected;
        }

        /// <summary>
        ///     初始化事物
        /// </summary>
        protected virtual void InitializeClientTransaction()
        {
            _clientTransaction = new ClientPipeStreamTransaction((NamedPipeClientStream)_stream, true, _callback);
            _clientTransaction.Disconnected += TransactionDisconnected;
        }

        /// <summary>
        ///     默认的消息回调函数
        /// </summary>
        /// <param name="data" type="byte[]">接收到的数据</param>
        protected void DefaultCallback(byte[] data)
        {
            ReceivedDataHandler(new LightSingleArgEventArgs<byte[]>(data));
        }

        //send async callback.
        private void SendCallback(IAsyncResult result)
        {
            try
            {
                _stream.EndWrite(result);
                _stream.Flush();
            }
            catch (System.Exception ex) { Logs.Logger.Log(ex); }
        }

        #endregion
    }
}