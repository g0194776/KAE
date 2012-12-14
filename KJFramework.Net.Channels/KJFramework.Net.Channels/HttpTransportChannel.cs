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
    ///     ����HTTPЭ���ͨѶ�ŵ����ṩ����صĻ�������
    /// </summary>
    public class HttpTransportChannel : TransportChannel, IHttpTransportChannel
    {
        #region Constructor

        /// <summary>
        ///      ����HTTPЭ���ͨѶ�ŵ����ṩ����صĻ�������
        ///     <para>* ʹ�ô˹��콫����ʹ�ŵ���Ϊ����״̬</para>
        /// </summary>
        /// <param name="context">������������</param>
        public HttpTransportChannel(HttpListenerContext context)
        {
            _channelType = HttpChannelTypes.Accepted;
            _listenerRequest = context.Request;
            _listenerResponse = context.Response;
            _connected = true;
            _communicationState = CommunicationStates.Opened;
        }

        /// <summary>
        ///      ����HTTPЭ���ͨѶ�ŵ����ṩ����صĻ�������
        ///     <para>* ʹ�ô˹��콫����ʹ�ŵ���Ϊ����״̬</para>
        /// </summary>
        /// <param name="requestUriString">��ʶ Internet ��Դ�� URI</param>
        /// <param name="timeout">��ʱʱ��</param>
        /// <exception cref="ArgumentNullException">��������</exception>
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
        ///      ����HTTPЭ���ͨѶ�ŵ����ṩ����صĻ�������
        ///     <para>* ʹ�ô˹��콫����ʹ�ŵ���Ϊ����״̬</para>
        /// </summary>
        /// <param name="requestUri">�����������Դ�� URI �� Uri</param>
        /// <param name="timeout">��ʱʱ��</param>
        /// <exception cref="ArgumentNullException">��������</exception>
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
        ///     ֹͣ
        /// </summary>
        protected override void InnerAbort()
        {
            Disconnect();
        }

        /// <summary>
        ///     ��
        /// </summary>
        protected override void InnerOpen()
        {
            Connect();
        }

        /// <summary>
        ///     �ر�
        /// </summary>
        protected override void InnerClose()
        {
            Disconnect();
        }

        #endregion

        #region Overrides of TransportChannel

        /// <summary>
        ///     ����
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
        ///     �Ͽ�
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
        ///     ��������
        ///     <para>* ����˷������з��͵�Ԫ���ݣ��������Զ��ְ�������ݡ�</para>
        /// </summary>
        /// <param name="data">Ҫ���͵�����</param>
        /// <returns>���ط��͵��ֽ���</returns>
        /// <exception cref="System.Exception">��������</exception>
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
        ///     ��ȡ�ŵ�����
        /// </summary>
        public HttpChannelTypes ChannelType
        {
            get { return _channelType; }
        }

        /// <summary>
        ///     ��ȡ�����������е��������ݵĳ���
        /// </summary>
        public long ContentLength64
        {
            get { return _listenerRequest.ContentLength64; }
        }

        /// <summary>
        ///     ��ȡ�����÷��ظ��ͻ��˵� HTTP ״̬����
        /// </summary>
        public int StatusCode
        {
            get { return _listenerResponse.StatusCode; }
            set { _listenerResponse.StatusCode = value; }
        }

        /// <summary>
        ///     ��ȡ�ͻ�������� URL ��Ϣ�������������Ͷ˿ڣ�
        /// </summary>
        public string RawUrl
        {
            get { return _listenerRequest.RawUrl; }
        }

        /// <summary>
        ///     ��ȡ�����ս���ַ
        /// </summary>
        public override IPEndPoint LocalEndPoint
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        ///     ��ȡ��������Ŀͻ��� IP ��ַ�Ͷ˿ں�
        /// </summary>
        public override IPEndPoint RemoteEndPoint
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        ///     ��ȡ�ͻ�������� Uri ����
        /// </summary>
        public System.Uri Url
        {
            get { return _listenerRequest.Url; }
        }

        /// <summary>
        ///     ��ȡ���󱻶��򵽵ķ����� IP ��ַ�Ͷ˿ں�
        /// </summary>
        public string UserHostAddress
        {
            get { return _listenerRequest.UserHostAddress; }
        }

        /// <summary>
        ///     ��ȡ�ɿͻ���ָ���� HTTP ����
        /// </summary>
        public string HttpMethod
        {
            get { return _listenerRequest.HttpMethod; }
        }

        /// <summary>
        ///     ��ȡ�ͻ��˽��ܵ� MIME ����
        /// </summary>
        public string[] AcceptTypes
        {
            get { return _listenerRequest.AcceptTypes; }
        }

        /// <summary>
        ///     ��ȡ�����������е��������ݵ� MIME ����
        /// </summary>
        public string ContentType
        {
            get { return _listenerRequest.ContentType; }
        }

        /// <summary>
        ///     ��ȡһ�� Boolean ֵ����ֵָʾ�ͻ����Ƿ��������������
        /// </summary>
        public bool KeepAlive
        {
            get { return _listenerRequest.KeepAlive; }
        }

        /// <summary>
        ///     ��ȡ Boolean ֵ����ֵָʾ�������Ƿ����Ա��ؼ����
        /// </summary>
        public bool IsLocal
        {
            get { return _listenerRequest.IsLocal; }
        }

        /// <summary>
        ///     ��ȡһ�� Boolean ֵ����ֵָʾ�����Ƿ��й�������������
        /// </summary>
        public bool HasEntityBody
        {
            get { return _listenerRequest.HasEntityBody; }
        }

        /// <summary>
        ///     ��ȡ�������з��͵ı�ͷ����/ֵ�Եļ���
        /// </summary>
        public NameValueCollection Headers
        {
            get { return _listenerRequest.Headers; }
        }

        /// <summary>
        ///     ��ȡ�����������еĲ�ѯ�ַ���
        /// </summary>
        public NameValueCollection QueryString
        {
            get { return _listenerRequest.QueryString; }
        }

        /// <summary>
        ///     ��ȡ�������͵� Cookie
        /// </summary>
        public CookieCollection Cookies
        {
            get { return _listenerRequest.Cookies; }
        }

        /// <summary>
        ///     ��ȡ�������������͵����ݵ����ݱ���
        /// </summary>
        public Encoding ContentEncoding
        {
            get { return _listenerRequest.ContentEncoding; }
        }

        /// <summary>
        ///     ����HTTP����
        /// </summary>
        public void Send()
        {
            InnerSend(null);
        }

        /// <summary>
        ///     ��ȡ�ڲ������������
        /// </summary>
        /// <returns>�����������</returns>
        public HttpListenerRequest GetRequest()
        {
            return _listenerRequest;
        }

        /// <summary>
        ///     ��ȡ�ڲ����Ļ�������
        /// </summary>
        /// <returns>���ػ�������</returns>
        public HttpListenerResponse GetResponse()
        {
            return _listenerResponse;
        }

        #endregion
    }
}