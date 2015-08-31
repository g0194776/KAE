using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using KJFramework.Net.Disconvery.Protocols;
using KJFramework.Tracing;
using Newtonsoft.Json;

namespace KJFramework.Net.Disconvery
{
    /// <summary>
    ///   探索模式的输入节点
    /// </summary>
    public sealed class DiscoveryInputPin
    {
        #region Constructors

        /// <summary>
        ///   探索模式的输入节点
        /// </summary>
        /// <param name="port">需要监听的UDP端口</param>
        /// <param name="bufferSize">
        ///   缓冲区大小
        ///   <para>*默认为: 2K</para>
        /// </param>
        /// <exception cref="ArgumentException">参数错误</exception>
        public DiscoveryInputPin(int port, int bufferSize = 2048)
        {
            if(port > IPEndPoint.MaxPort || port < IPEndPoint.MinPort) throw new ArgumentException("#Incorrect input UDP port.");
            if (bufferSize <= 0) throw new ArgumentException("#Incorrect buffer size.");
            _port = port;
            _bufferSize = bufferSize;
            _broadcastIep = new IPEndPoint(IPAddress.Broadcast, _port);
        }

        #endregion

        #region Members

        private bool _enable;
        private Socket _socket;
        private readonly int _port;
        private readonly int _bufferSize;
        private SocketAsyncEventArgs _args;
        private readonly IPEndPoint _broadcastIep;
        private readonly byte[] _buffer = new byte[2048];
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DiscoveryInputPin));
        private readonly ConcurrentDictionary<string, Action<CommonBoradcastProtocol>> _callbacks = new ConcurrentDictionary<string, Action<CommonBoradcastProtocol>>(); 

        /// <summary>
        ///   获取或设置当前的启用状态
        /// </summary>
        public bool Enable 
        {
            get
            {
                return _enable;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   开始监听
        /// </summary>
        public void Start()
        {
            Stop();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Any, _port));
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += EventCompleted;
            args.AcceptSocket = _socket;
            args.RemoteEndPoint = _broadcastIep;
            args.SetBuffer(_buffer, 0, _buffer.Length);
            StartRecv(args);
            _enable = true;
        }

        /// <summary>
        ///   停止监听
        /// </summary>
        public void Stop()
        {
            if (!_enable) return;
            if (_socket != null)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Disconnect(false);
                }
                catch { /*Do nothing even throw an exception.*/ }
            }
            if (_args != null)
            {
                _args.Completed -= EventCompleted;
                _args.Dispose();
                _args = null;
            }
            _enable = false;
        }

        /// <summary>
        ///   开始异步接收数据
        /// </summary>
        /// <param name="args">异步SOCKET事件</param>
        private void StartRecv(SocketAsyncEventArgs args)
        {
            args.SetBuffer(0, _buffer.Length);
            if (!_socket.ReceiveFromAsync(args)) ProcessRecv(args);
        }

        /// <summary>
        ///   处理接收到的数据
        /// </summary>
        /// <param name="args">异步SOCKET事件</param>
        private void ProcessRecv(SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred == 0 || args.SocketError != SocketError.Success)
            {
                Stop();
                return;
            }
            try
            {
                int count = args.BytesTransferred;
                string content = Encoding.UTF8.GetString(args.Buffer, 0, count);
                CommonBoradcastProtocol obj = JsonConvert.DeserializeObject<CommonBoradcastProtocol>(content);
                //discard current boradcast object directly when it was an illegal object.
                if (string.IsNullOrEmpty(obj.Key)) return;
                //dispatch it.
                Action<CommonBoradcastProtocol> callback;
                if (!_callbacks.TryGetValue(obj.Key, out callback))
                {
                    _tracing.Warn("#We had to discared current boradcast object, because of it hasn't any dispatcher in there.");
                    return;
                }
                callback(obj);
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
            finally { StartRecv(args); }

        }

        /// <summary>
        ///   添加一个关注指定KEY的事件
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="callback">回调函数</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public void AddNotificationEvent(string key, Action<CommonBoradcastProtocol> callback)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            if (callback == null) throw new ArgumentNullException("callback");
            _callbacks[key] = callback;
        }

        /// <summary>
        ///   移除一个关注指定KEY的事件
        /// </summary>
        /// <param name="key">关键字</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public void RemoveNotificationEvent(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            Action<CommonBoradcastProtocol> callback;
            _callbacks.TryRemove(key, out callback);
        }

        /// <summary>
        ///   清空所有关注的事件
        /// </summary>
        public void ClearNotificationEvent()
        {
            _callbacks.Clear();
        }

        #endregion

        #region Events

        //currently UDP socket async event.
        void EventCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0 || e.SocketError != SocketError.Success)
            {
                Stop();
                return;
            }
            ProcessRecv(e);
        }

        #endregion
    }
}