using System;
using System.Net;
using KJFramework.EventArgs;
using KJFramework.Logger;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     基于HTTP协议的宿主信道，提供了相关的基本操作
    /// </summary>
    public class HttpHostTransportChannel : HostTransportChannel, IHttpHostTransportChannel
    {
        #region Constructor

        /// <summary>
        ///     基于HTTP协议的宿主信道，提供了相关的基本操作
        /// </summary>
        public HttpHostTransportChannel()
        {
            _listener = new HttpListener();
        }

        #endregion

        #region Members

        private readonly HttpListener _listener;

        #endregion

        #region Overrides of HostTransportChannel

        /// <summary>
        ///     注册网络
        /// </summary>
        /// <returns>返回注册的状态</returns>
        public override bool Regist()
        {
            try
            {
                _listener.Start();
                if (_listener.IsListening)
                {
                    _listener.BeginGetContext(Callback, _listener);
                    return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return false;
            }
        }

        /// <summary>
        ///     注销网络
        /// </summary>
        /// <returns>返回注册的状态</returns>
        public override bool UnRegist()
        {
            _listener.Stop();
            return !_listener.IsListening;
        }

        #endregion

        #region Implementation of IHttpHostTransportChannel

        /// <summary>
        ///     获取由此 HttpListener 对象处理的统一资源标识符 (URI) 前缀
        /// </summary>
        public HttpListenerPrefixCollection Prefixes
        {
            get { return _listener.Prefixes; }
        }

        /// <summary>
        ///     获取或设置与此 HttpListener 对象关联的领域或资源分区
        /// </summary>
        public string Realm
        {
            get { return _listener.Realm; }
            set { _listener.Realm = value; }
        }

        /// <summary>
        ///     获取或设置 Boolean 值，该值控制当使用 NTLM 时是否需要对使用同一传输控制协议 (TCP) 连接的其他请求进行身份验证
        /// </summary>
        public bool UnsafeConnectionNtlmAuthentication
        {
            get { return _listener.UnsafeConnectionNtlmAuthentication; }
            set { _listener.UnsafeConnectionNtlmAuthentication = value; }
        }

        /// <summary>
        ///     获取或设置 Boolean 值，该值指定应用程序是否接收 HttpListener 向客户端发送响应时发生的异常
        /// </summary>
        public bool IgnoreWriteExceptions
        {
            get { return _listener.IgnoreWriteExceptions; }
            set { _listener.IgnoreWriteExceptions = value; }
        }

        #endregion

        #region Methods

        //http listener callback proc.
        protected virtual void Callback(IAsyncResult result)
        {
            try
            {
                HttpListenerContext context = _listener.EndGetContext(result);
                ITransportChannel transportChannel = new HttpTransportChannel(context);
                transportChannel.Disconnected += TransportChannelDisconnected;
                //active this event.
                ChannelCreatedHandler(new LightSingleArgEventArgs<ITransportChannel>(transportChannel));
            }
            catch (System.Exception ex) { Logs.Logger.Log(ex); }
            finally
            {
                if (_listener.IsListening)
                {
                    _listener.BeginGetContext(Callback, _listener);
                }
            }
        }

        #endregion

        #region Events

        //http channel disconencted.
        void TransportChannelDisconnected(object sender, System.EventArgs e)
        {
            ITransportChannel transportChannel = (ITransportChannel)sender;
            transportChannel.Disconnected -= TransportChannelDisconnected;
            ChannelDisconnectedHandler(new LightSingleArgEventArgs<ITransportChannel>(transportChannel));
        }

        #endregion
    }
}