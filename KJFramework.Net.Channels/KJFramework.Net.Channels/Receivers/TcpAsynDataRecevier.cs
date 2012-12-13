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
    ///     �����Ļ���TCPЭ�����Ϣ���������ṩ����صĻ���������
    ///     <para>* �첽���ƻ��� .NET FRAMEWORK 3.5�е���Socket�첽ģ�͡�</para>
    /// </summary>
    public class TcpAsynDataRecevier
    {
        #region Constructor

        /// <summary>
        ///     �����Ļ���TCPЭ�����Ϣ���������ṩ����صĻ���������
        ///             * �첽���ƻ��� .NET FRAMEWORK 3.5�е���Socket�첽ģ�͡�
        /// </summary>
        public TcpAsynDataRecevier()
        { }

        /// <summary>
        ///     �����Ļ���TCPЭ�����Ϣ���������ṩ����صĻ���������
        ///             * �첽���ƻ��� .NET FRAMEWORK 3.5�е���Socket�첽ģ�͡�
        /// </summary>
        /// <param name="socket">�׽���</param>
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
        ///     ��ʼ��������
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
        ///     ������յ�����
        /// </summary>
        /// <param name="e">�첽�����Ķ���</param>
        /// <param name="segment">�ڴ�Ƭ��</param>
        internal void ProcessReceive(SocketAsyncEventArgs e, IMemorySegment segment)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                //���ճɹ�
                try { ProcessData(segment, e.BytesTransferred); }
                catch (System.Exception ex) { Logs.Logger.Log(ex, DebugGrade.Standard); }
                finally { StartReceive(); }
            }
            else Stop();
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="segment">�ڴ�Ƭ��</param>
        /// <param name="bytesTransferred">���յ������ݳ���</param>
        private void ProcessData(IMemorySegment segment, int bytesTransferred)
        {
            ReceivedDataHandler(new SegmentReceiveEventArgs(segment, bytesTransferred));
        }

        #endregion

        #region ITcpMessageRecevier Members

        private Socket _socket;

        /// <summary>
        ///     �û��׽���
        /// </summary>
        public Socket Socket
        {
            get { return _socket; }
            set { _socket = value; }
        }

        /// <summary>
        ///     ���յ������¼�
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
        ///     �������Ͽ������¼�
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

        #region IMetadata<string> Members

        private int _key;

        /// <summary>
        ///     ��ȡ����������Լ�����ж����Ψһ��ʾ
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
        ///     ִֹͣ��
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