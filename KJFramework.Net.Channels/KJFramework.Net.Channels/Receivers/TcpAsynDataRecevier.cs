using KJFramework.Basic.Enum;
using KJFramework.Cache.Cores;
using KJFramework.Cache.Objects;
using KJFramework.Logger;
using KJFramework.Net.Channels.Caches;
using KJFramework.Net.Channels.Events;
using KJFramework.Net.EventArgs;
using KJFramework.Tracing;
using System;
using System.Net.Sockets;
using System.Threading;

namespace KJFramework.Net.Channels.Receivers
{
    /// <summary>
    ///     基础的基于TCP协议的消息接收器，提供了相关的基本操作。
    ///     <para>* 异步机制基于 .NET FRAMEWORK 3.5中的新Socket异步模型。</para>
    /// </summary>
    public class TcpAsynDataRecevier
    {
        #region Constructor

        /// <summary>
        ///     基础的基于TCP协议的消息接收器，提供了相关的基本操作。
        ///             * 异步机制基于 .NET FRAMEWORK 3.5中的新Socket异步模型。
        /// </summary>
        public TcpAsynDataRecevier()
        { }

        /// <summary>
        ///     基础的基于TCP协议的消息接收器，提供了相关的基本操作。
        ///             * 异步机制基于 .NET FRAMEWORK 3.5中的新Socket异步模型。
        /// </summary>
        /// <param name="socket">套接字</param>
        public TcpAsynDataRecevier(Socket socket)
        {
            _socket = socket;
        }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(TcpAsynDataRecevier));

        #endregion

        #region Methods

        /// <summary>
        ///     开始接收数据
        /// </summary>
        private void StartReceive()
        {
            try
            {
                IFixedCacheStub<BuffSocketStub> stub = ChannelConst.BuffAsyncStubPool.Rent();
                if (stub == null) throw new System.Exception("#Cannot rent an async recv io-stub for socket recv async action.");
                SocketAsyncEventArgs e = stub.Cache.Target;
                //set callback var = receiver
                stub.Tag = this;
                e.AcceptSocket = _socket;
                e.UserToken = stub;
                bool result;
                using (ExecutionContext.SuppressFlow())
                    result = e.AcceptSocket.ReceiveAsync(e);
                if (!result)
                {
                    ChannelConst.BuffAsyncStubPool.Giveback(stub);
                    ProcessReceive(e, stub.Cache.Segment);
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                Stop();
            }
        }

        /// <summary>
        ///     处理接收的数据
        /// </summary>
        /// <param name="e">异步上下文对象</param>
        /// <param name="segment">内存片段</param>
        internal void ProcessReceive(SocketAsyncEventArgs e, IMemorySegment segment)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                //接收成功
                try { ProcessData(segment, e.BytesTransferred); }
                catch (System.Exception ex) { Logs.Logger.Log(ex, DebugGrade.Standard); }
                finally { StartReceive(); }
            }
            else Stop();
        }

        /// <summary>
        ///     处理数据
        /// </summary>
        /// <param name="segment">内存片段</param>
        /// <param name="bytesTransferred">接收到的数据长度</param>
        private void ProcessData(IMemorySegment segment, int bytesTransferred)
        {
            ReceivedDataHandler(new SegmentReceiveEventArgs(segment, bytesTransferred));
        }

        #endregion

        #region ITcpMessageRecevier Members

        private Socket _socket;

        /// <summary>
        ///     用户套接字
        /// </summary>
        public Socket Socket
        {
            get { return _socket; }
            set { _socket = value; }
        }

        /// <summary>
        ///     接收到数据事件
        /// </summary>
        public event EventHandler<SegmentReceiveEventArgs> ReceivedData;
        protected void ReceivedDataHandler(SegmentReceiveEventArgs e)
        {
            EventHandler<SegmentReceiveEventArgs> handler = ReceivedData;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region IMessageRecevier<string,NetMessage> Members

        /// <summary>
        ///     接收器断开连接事件
        /// </summary>
        public event EventHandler<RecevierDisconnectedEventArgs> Disconnected;
        private void DisconnectedHandler(RecevierDisconnectedEventArgs e)
        {
            EventHandler<RecevierDisconnectedEventArgs> disconnected = Disconnected;
            if (disconnected != null) disconnected(this, e);
        }

        #endregion

        #region IListener<string> Members

        private bool _state;

        /// <summary>
        ///     获取或设置当前的状态
        /// </summary>
        public bool State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        #endregion

        #region IMetadata<string> Members

        private int _key;

        /// <summary>
        ///     获取或设置用来约束所有对象的唯一标示
        /// </summary>
        public int Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        #endregion

        #region IControlable Members

        /// <summary>
        ///     开始执行
        /// </summary>
        public void Start()
        {
            if (!_state)
            {
                if (_socket == null || !_socket.Connected)
                {
                    DisconnectedHandler(null);
                    return;
                }
                _key = _socket.GetHashCode();
                StartReceive();
                _state = true;
            }
        }

        /// <summary>
        ///     停止执行
        /// </summary>
        public void Stop()
        {
            if (_state)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                }
                catch (System.Exception e) { Logs.Logger.Log(e); }
                _socket = null;
                _state = false;
            }
            DisconnectedHandler(null);
        }

        #endregion
    }
}