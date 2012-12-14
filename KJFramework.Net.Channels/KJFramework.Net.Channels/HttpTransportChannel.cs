using KJFramework.EventArgs;
using KJFramework.Net.Channels.Enums;
using KJFramework.Tracing;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     基于HTTP协议的通讯信道，提供了相关的基本操作
    /// </summary>
    public class HttpTransportChannel : TransportChannel, IHttpTransportChannel
    {
        #region Constructor

        /// <summary>
        ///      基于HTTP协议的通讯信道，提供了相关的基本操作
        ///     <para>* 使用此构造将会迫使信道变为被动状态</para>
        /// </summary>
        /// <param name="context">监听器上下文</param>
        public HttpTransportChannel(HttpListenerContext context)
        {
            _channelType = HttpChannelTypes.Accepted;
            _listenerRequest = context.Request;
            _listenerResponse = context.Response;
            _connected = true;
            _communicationState = CommunicationStates.Opened;
        }

        /// <summary>
        ///      基于HTTP协议的通讯信道，提供了相关的基本操作
        ///     <para>* 使用此构造将会迫使信道变为主动状态</para>
        /// </summary>
        /// <param name="requestUriString">标识 Internet 资源的 URI</param>
        /// <param name="timeout">超时时间</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public HttpTransportChannel(string requestUriString, int timeout)
        {
            if (string.IsNullOrEmpty(requestUriString))
            {
                throw new ArgumentNullException("requestUriString");
            }
            _communicationState = CommunicationStates.Closed;
            _channelType = HttpChannelTypes.Connected;
            _request = (HttpWebRequest) WebRequest.Create(requestUriString);
            _request.Timeout = timeout;
        }

        /// <summary>
        ///      基于HTTP协议的通讯信道，提供了相关的基本操作
        ///     <para>* 使用此构造将会迫使信道变为主动状态</para>
        /// </summary>
        /// <param name="requestUri">包含请求的资源的 URI 的 Uri</param>
        /// <param name="timeout">超时时间</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public HttpTransportChannel(System.Uri requestUri, int timeout)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException("requestUri");
            }
            _communicationState = CommunicationStates.Closed;
            _channelType = HttpChannelTypes.Connected;
            _request = (HttpWebRequest)WebRequest.Create(requestUri);
            _request.Timeout = timeout;
        }

        #endregion

        #region Members

        protected readonly HttpListenerRequest _listenerRequest;
        protected readonly HttpListenerResponse _listenerResponse;
        protected readonly HttpChannelTypes _channelType;
        protected readonly HttpWebRequest _request;
        private HttpWebResponse _response;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(HttpTransportChannel));

        #endregion

        #region Overrides of ServiceChannel

        /// <summary>
        ///     停止
        /// </summary>
        protected override void InnerAbort()
        {
            Disconnect();
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
            if (_channelType == HttpChannelTypes.Accepted)
                throw new System.Exception("Cannot execute connect func for this channel, because current channel state is accepted.");
            //set true value at this time!
            _connected = true;
            _communicationState = CommunicationStates.Opened;
            ConnectedHandler(null);
        }

        /// <summary>
        ///     断开
        /// </summary>
        public override void Disconnect()
        {
            _communicationState = CommunicationStates.Closing;
            try
            {
                if (_channelType == HttpChannelTypes.Accepted)
                {
                    _listenerResponse.Close();
                }
                else
                {
                    if (_response != null)
                    {
                        _response.Close();
                        _response = null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
            }
            finally
            {
                _connected = false;
                _communicationState = CommunicationStates.Closed;
                DisconnectedHandler(null);
            }
        }

        /// <summary>
        ///     发送数据
        ///     <para>* 如果此方法进行发送的元数据，可能是自动分包后的数据。</para>
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <returns>返回发送的字节数</returns>
        /// <exception cref="System.Exception">发生错误</exception>
        protected override int InnerSend(byte[] data)
        {
            if (!_connected) throw new System.Exception("Cannot send data from this channel, because current channel has been disconencted.");
            try
            {
                if (_channelType == HttpChannelTypes.Accepted)
                {
                    if (data != null)
                    {
                        _listenerResponse.ContentLength64 = data.Length;
                        _listenerResponse.OutputStream.Write(data, 0, data.Length);
                        _listenerResponse.OutputStream.Flush();
                        return data.Length;
                    }
                    return 0;
                }
                if (data != null)
                {
                    if (data.Length > ChannelConst.MaxMessageDataLength)
                    {
                        _tracing.Warn(string.Format("#Illegal data size: {0}, current allow size: {1}", data.Length, ChannelConst.MaxMessageDataLength));
                        return -1;
                    }
                    Stream requestStream = _request.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Close();
                }
                _response = (HttpWebResponse)_request.GetResponse();
                if (_response.ContentLength > 0)
                {
                    Stream responseStream = _response.GetResponseStream();
                    byte[] resData = new byte[_response.ContentLength];
                    responseStream.Read(resData, 0, resData.Length);
                    ReceivedDataHandler(new LightSingleArgEventArgs<byte[]>(resData));
                }
                return data == null ? 0 : data.Length;
            }
            catch (System.Exception ex)
            {
                _communicationState = CommunicationStates.Faulte;
                _tracing.Error(ex, null);
                return -1;
            } finally { Disconnect(); }
        }

        #endregion

        #region Implementation of IHttpTransportChannel

        /// <summary>
        ///     获取信道类型
        /// </summary>
        public HttpChannelTypes ChannelType
        {
            get { return _channelType; }
        }

        /// <summary>
        ///     获取包含在请求中的正文数据的长度
        /// </summary>
        public long ContentLength64
        {
            get { return _listenerRequest.ContentLength64; }
        }

        /// <summary>
        ///     获取或设置返回给客户端的 HTTP 状态代码
        /// </summary>
        public int StatusCode
        {
            get { return _listenerResponse.StatusCode; }
            set { _listenerResponse.StatusCode = value; }
        }

        /// <summary>
        ///     获取客户端请求的 URL 信息（不包括主机和端口）
        /// </summary>
        public string RawUrl
        {
            get { return _listenerRequest.RawUrl; }
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
        ///     获取客户端请求的 Uri 对象
        /// </summary>
        public System.Uri Url
        {
            get { return _listenerRequest.Url; }
        }

        /// <summary>
        ///     获取请求被定向到的服务器 IP 地址和端口号
        /// </summary>
        public string UserHostAddress
        {
            get { return _listenerRequest.UserHostAddress; }
        }

        /// <summary>
        ///     获取由客户端指定的 HTTP 方法
        /// </summary>
        public string HttpMethod
        {
            get { return _listenerRequest.HttpMethod; }
        }

        /// <summary>
        ///     获取客户端接受的 MIME 类型
        /// </summary>
        public string[] AcceptTypes
        {
            get { return _listenerRequest.AcceptTypes; }
        }

        /// <summary>
        ///     获取包含在请求中的正文数据的 MIME 类型
        /// </summary>
        public string ContentType
        {
            get { return _listenerRequest.ContentType; }
        }

        /// <summary>
        ///     获取一个 Boolean 值，该值指示客户端是否请求持续型连接
        /// </summary>
        public bool KeepAlive
        {
            get { return _listenerRequest.KeepAlive; }
        }

        /// <summary>
        ///     获取 Boolean 值，该值指示该请求是否来自本地计算机
        /// </summary>
        public bool IsLocal
        {
            get { return _listenerRequest.IsLocal; }
        }

        /// <summary>
        ///     获取一个 Boolean 值，该值指示请求是否有关联的正文数据
        /// </summary>
        public bool HasEntityBody
        {
            get { return _listenerRequest.HasEntityBody; }
        }

        /// <summary>
        ///     获取在请求中发送的标头名称/值对的集合
        /// </summary>
        public NameValueCollection Headers
        {
            get { return _listenerRequest.Headers; }
        }

        /// <summary>
        ///     获取包含在请求中的查询字符串
        /// </summary>
        public NameValueCollection QueryString
        {
            get { return _listenerRequest.QueryString; }
        }

        /// <summary>
        ///     获取随请求发送的 Cookie
        /// </summary>
        public CookieCollection Cookies
        {
            get { return _listenerRequest.Cookies; }
        }

        /// <summary>
        ///     获取可用于随请求发送的数据的内容编码
        /// </summary>
        public Encoding ContentEncoding
        {
            get { return _listenerRequest.ContentEncoding; }
        }

        /// <summary>
        ///     发送HTTP请求
        /// </summary>
        public void Send()
        {
            InnerSend(null);
        }

        /// <summary>
        ///     获取内部核心请求对象
        /// </summary>
        /// <returns>返回请求对象</returns>
        public HttpListenerRequest GetRequest()
        {
            return _listenerRequest;
        }

        /// <summary>
        ///     获取内部核心回馈对象
        /// </summary>
        /// <returns>返回回馈对象</returns>
        public HttpListenerResponse GetResponse()
        {
            return _listenerResponse;
        }

        #endregion
    }
}