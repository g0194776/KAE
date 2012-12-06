using System;
using System.Net;
using System.Net.Sockets;
using KJFramework.Basic.Enum;
using KJFramework.Logger;
using KJFramework.Logger.LogObject;
using KJFramework.Net.EventArgs;
using KJFramework.Net.Helper;

namespace KJFramework.Net.Listener.Asynchronous
{
    /// <summary>
    ///     基础的基于TCP协议的异步端口监听器 - 版本2
    ///             * 异步机制基于 .NET FRAMEWORK 3.5中的新Socket异步模型。
    /// </summary>
    /// <typeparam name="TListenerInfo">监听器类型</typeparam>
    public class BasicTcpAsynListenerV2<TListenerInfo> : ITcpIocpListener<TListenerInfo>
        where TListenerInfo : IPortListenerInfomation
    {
        #region 构造函数

        /// <summary>
        ///     基础的基于TCP协议的异步端口监听器 - 版本2
        ///             * 异步机制基于 .NET FRAMEWORK 3.5中的新Socket异步模型
        /// </summary>
        /// <param name="port">端口号</param>
        public BasicTcpAsynListenerV2(int port)
            : this(new IPEndPoint(IPAddress.Any, port))
        { }

        /// <summary>
        ///     基础的基于TCP协议的异步端口监听器 - 版本2
        ///             * 异步机制基于 .NET FRAMEWORK 3.5中的新Socket异步模型
        /// </summary>
        /// <param name="ipEndPoint">绑定的本地终端地址</param>
        public BasicTcpAsynListenerV2(IPEndPoint ipEndPoint)
        {
            if (ipEndPoint.Port <= 0)
            {
                throw new System.Exception("参数错误，监听的端口需要大于0。");
            }
            _port = ipEndPoint.Port;
            _ipEndPoint = ipEndPoint;
        }

        #endregion

        #region 成员

        private IPEndPoint _ipEndPoint;

        #endregion

        #region 方法

        /// <summary>
        ///     初始化操作
        /// </summary>
        private void Initialize()
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress , true);
            _listener.Bind(_ipEndPoint);
            _listener.Listen(60000);
            _key = _listener.GetHashCode();
        }

        /// <summary>
        ///     开始接受连接
        /// </summary>
        /// <param name="e">异步上下文对象</param>
        private void StartAccept(SocketAsyncEventArgs e)
        {
            if (e == null)
            {
                e= new SocketAsyncEventArgs();
                e.Completed += SocketAcceptCompleted;
            }
            else { e.AcceptSocket = null; }
            try
            {
                if (_listener != null)
                    if (!_listener.AcceptAsync(e)) ProcessAccept(e);
            }
            catch(System.Exception ex) { Logs.Logger.Log(ex); }
        }

        /// <summary>
        ///     处理新连接事件
        /// </summary>
        /// <param name="e">异步上下文对象</param>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.AcceptSocket != null)
                {
                    //设置心跳时间
                    e.AcceptSocket.IOControl(IOControlCode.KeepAliveValues, NetHelper.KeepAliveValue, null);
                    //business exceptions way.
                    try { ConnectedHandler(new IocpPortListenerConnectedEventArgs<TListenerInfo>(e.AcceptSocket, _listenerInfomation)); }
                    catch (System.Exception innerEx) { Logs.Logger.Log(innerEx, DebugGrade.Standard); }
                }
                StartAccept(e);
            }
            catch (System.Exception ex)
            {
                e.Completed -= SocketAcceptCompleted;
                e.Dispose();
                Stop();
                Logs.Logger.Log(ex, DebugGrade.High);
            }
        }

        /// <summary>
        ///     记录异常
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <param name="debugGrade">异常等级</param>
        private void Log(System.Exception e, DebugGrade debugGrade)
        {
            if (_debugLogger != null)
            {
                _debugLogger.Log(e, debugGrade);
            }
        }

        #endregion

        #region 事件

        //Socket完成接收新连接事件
        void SocketAcceptCompleted(Object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        #endregion

        #region ITcpIocpListener<TListenerInfo> Members

        private Socket _listener;

        /// <summary>
        ///     获取TCP端口监听对象
        /// </summary>
        public Socket Listener
        {
            get { return _listener; }
        }

        private TListenerInfo _listenerInfomation;

        /// <summary>
        ///     获取或设置端口监听器详细信息元接口
        /// </summary>
        public TListenerInfo ListenerInfomation
        {
            get
            {
                return _listenerInfomation;
            }
            set
            {
                _listenerInfomation = value;
            }
        }

        [Obsolete("在此异步端口监听器版本中，不支持此操作。", true)]
        public void GetPedding()
        { }

        /// <summary>
        ///     新用户连接事件
        /// </summary>
        public event DELEGATE_IOCP_PORTLISTENER_CONNECTED<TListenerInfo> Connected;
        private void ConnectedHandler(IocpPortListenerConnectedEventArgs<TListenerInfo> e)
        {
            DELEGATE_IOCP_PORTLISTENER_CONNECTED<TListenerInfo> connected = Connected;
            if (connected != null) connected(this, e);
        }

        #endregion

        #region IPortListener Members

        private int _port;

        /// <summary>
        ///     监听的端口
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        [Obsolete("在此异步端口监听器版本中，不支持此操作。", true)]
        public void Listen()
        {
            
        }

        #endregion

        #region IListener<int> Members

        private IDebugLogger<IDebugLog> _debugLogger;

        /// <summary>
        ///     获取或设置异常记录器
        /// </summary>
        public IDebugLogger<IDebugLog> DebugLogger
        {
            get
            {
                return _debugLogger;
            }
            set
            {
                _debugLogger = value;
            }
        }

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

        #region IMetadata<int> Members

        private int _key;

        /// <summary>
        /// 获取或设置用来约束所有对象的唯一标示
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
            if(!_state)
            {
                //初始化
                Initialize();
                StartAccept(null);
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
                if (_listener != null)
                {
                    try { _listener.Shutdown(SocketShutdown.Both); }
                    catch (System.Exception e) { Log(e, DebugGrade.Fatal); }
                    _listener.Close();
                    _listener = null;
                }
                _state = false;
            }
        }

        #endregion
    }
}