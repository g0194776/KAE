using KJFramework.Net.EventArgs;
using KJFramework.Net.Helper;
using KJFramework.Tracing;
using System;
using System.Net;
using System.Net.Sockets;

namespace KJFramework.Net.Listener.Asynchronous
{
    /// <summary>
    ///     �����Ļ���TCPЭ����첽�˿ڼ����� - �汾2
    ///             * �첽���ƻ��� .NET FRAMEWORK 3.5�е���Socket�첽ģ�͡�
    /// </summary>
    /// <typeparam name="TListenerInfo">����������</typeparam>
    public class BasicTcpAsynListenerV2<TListenerInfo> : ITcpIocpListener<TListenerInfo>
        where TListenerInfo : IPortListenerInfomation
    {
        #region ���캯��

        /// <summary>
        ///     �����Ļ���TCPЭ����첽�˿ڼ����� - �汾2
        ///             * �첽���ƻ��� .NET FRAMEWORK 3.5�е���Socket�첽ģ��
        /// </summary>
        /// <param name="port">�˿ں�</param>
        public BasicTcpAsynListenerV2(int port)
            : this(new IPEndPoint(IPAddress.Any, port))
        { }

        /// <summary>
        ///     �����Ļ���TCPЭ����첽�˿ڼ����� - �汾2
        ///             * �첽���ƻ��� .NET FRAMEWORK 3.5�е���Socket�첽ģ��
        /// </summary>
        /// <param name="ipEndPoint">�󶨵ı����ն˵�ַ</param>
        public BasicTcpAsynListenerV2(IPEndPoint ipEndPoint)
        {
            if (ipEndPoint.Port <= 0)
            {
                throw new System.Exception("�������󣬼����Ķ˿���Ҫ����0��");
            }
            _port = ipEndPoint.Port;
            _ipEndPoint = ipEndPoint;
        }

        #endregion

        #region ��Ա

        private IPEndPoint _ipEndPoint;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(BasicTcpAsynListenerV2<TListenerInfo>));

        #endregion

        #region ����

        /// <summary>
        ///     ��ʼ������
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
        ///     ��ʼ��������
        /// </summary>
        /// <param name="e">�첽�����Ķ���</param>
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
            catch(System.Exception ex) { _tracing.Error(ex, null); }
        }

        /// <summary>
        ///     �����������¼�
        /// </summary>
        /// <param name="e">�첽�����Ķ���</param>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.AcceptSocket != null)
                {
                    //��������ʱ��
                    e.AcceptSocket.IOControl(IOControlCode.KeepAliveValues, NetHelper.KeepAliveValue, null);
                    //business exceptions way.
                    try { ConnectedHandler(new IocpPortListenerConnectedEventArgs<TListenerInfo>(e.AcceptSocket, _listenerInfomation)); }
                    catch (System.Exception innerEx) { _tracing.Error(innerEx, null); }
                }
                StartAccept(e);
            }
            catch (System.Exception ex)
            {
                e.Completed -= SocketAcceptCompleted;
                e.Dispose();
                Stop();
                _tracing.Error(ex, null); 
            }
        }

        #endregion

        #region �¼�

        //Socket��ɽ����������¼�
        void SocketAcceptCompleted(Object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        #endregion

        #region ITcpIocpListener<TListenerInfo> Members

        private Socket _listener;

        /// <summary>
        ///     ��ȡTCP�˿ڼ�������
        /// </summary>
        public Socket Listener
        {
            get { return _listener; }
        }

        private TListenerInfo _listenerInfomation;

        /// <summary>
        ///     ��ȡ�����ö˿ڼ�������ϸ��ϢԪ�ӿ�
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

        [Obsolete("�ڴ��첽�˿ڼ������汾�У���֧�ִ˲�����", true)]
        public void GetPedding()
        { }

        /// <summary>
        ///     ���û������¼�
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
        ///     �����Ķ˿�
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

        [Obsolete("�ڴ��첽�˿ڼ������汾�У���֧�ִ˲�����", true)]
        public void Listen()
        {
            
        }

        #endregion

        #region IListener<int> Members

        private bool _state;

        /// <summary>
        ///     ��ȡ�����õ�ǰ��״̬
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
        /// ��ȡ����������Լ�����ж����Ψһ��ʾ
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
        ///     ��ʼִ��
        /// </summary>
        public void Start()
        {
            if(!_state)
            {
                //��ʼ��
                Initialize();
                StartAccept(null);
                _state = true;
            }
        }

        /// <summary>
        ///     ִֹͣ��
        /// </summary>
        public void Stop()
        {
            if (_state)
            {
                if (_listener != null)
                {
                    try { _listener.Shutdown(SocketShutdown.Both); }
                    catch (System.Exception e) { _tracing.Error(e, null); }
                    _listener.Close();
                    _listener = null;
                }
                _state = false;
            }
        }

        #endregion
    }
}