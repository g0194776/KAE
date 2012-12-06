using System;
using System.Net;
using KJFramework.EventArgs;
using KJFramework.Logger;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     ����HTTPЭ��������ŵ����ṩ����صĻ�������
    /// </summary>
    public class HttpHostTransportChannel : HostTransportChannel, IHttpHostTransportChannel
    {
        #region Constructor

        /// <summary>
        ///     ����HTTPЭ��������ŵ����ṩ����صĻ�������
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
        ///     ע������
        /// </summary>
        /// <returns>����ע���״̬</returns>
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
        ///     ע������
        /// </summary>
        /// <returns>����ע���״̬</returns>
        public override bool UnRegist()
        {
            _listener.Stop();
            return !_listener.IsListening;
        }

        #endregion

        #region Implementation of IHttpHostTransportChannel

        /// <summary>
        ///     ��ȡ�ɴ� HttpListener �������ͳһ��Դ��ʶ�� (URI) ǰ׺
        /// </summary>
        public HttpListenerPrefixCollection Prefixes
        {
            get { return _listener.Prefixes; }
        }

        /// <summary>
        ///     ��ȡ��������� HttpListener ����������������Դ����
        /// </summary>
        public string Realm
        {
            get { return _listener.Realm; }
            set { _listener.Realm = value; }
        }

        /// <summary>
        ///     ��ȡ������ Boolean ֵ����ֵ���Ƶ�ʹ�� NTLM ʱ�Ƿ���Ҫ��ʹ��ͬһ�������Э�� (TCP) ���ӵ�����������������֤
        /// </summary>
        public bool UnsafeConnectionNtlmAuthentication
        {
            get { return _listener.UnsafeConnectionNtlmAuthentication; }
            set { _listener.UnsafeConnectionNtlmAuthentication = value; }
        }

        /// <summary>
        ///     ��ȡ������ Boolean ֵ����ֵָ��Ӧ�ó����Ƿ���� HttpListener ��ͻ��˷�����Ӧʱ�������쳣
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