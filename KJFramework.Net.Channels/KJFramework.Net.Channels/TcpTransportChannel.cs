using KJFramework.Basic.Enum;
using KJFramework.Cache.Cores;
using KJFramework.Net.Channels.Caches;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.Events;
using KJFramework.Net.Channels.Receivers;
using KJFramework.Net.Channels.Statistics;
using KJFramework.Net.EventArgs;
using KJFramework.Statistics;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     ����TCPЭ��Ĵ���ͨ�����ṩ����صĻ���������
    ///     <para>* ���ŵ�֧������Ԫ�����¼���</para>
    /// </summary>
    public class TcpTransportChannel : TransportChannel, ITcpTransportChannel, IReconnectionTransportChannel
    {
        #region ��Ա

        protected Socket _socket;
        protected int _channelKey;
        protected TcpAsynDataRecevier _receiver;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(TcpTransportChannel));

        /// <summary>
        ///     ��ȡ�����ս���ַ
        /// </summary>
        public override EndPoint LocalEndPoint
        {
            get { return _socket.LocalEndPoint; }
        }

        /// <summary>
        ///     ��ȡԶ���ս���ַ
        /// </summary>
        public override EndPoint RemoteEndPoint
        {
            get { return _socket.RemoteEndPoint; }
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰͨ���Ƿ�������״̬
        /// </summary>
        public override bool IsConnected
        {
            get { return (_connected = _socket.Connected); }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     ����TCPЭ��Ĵ���ͨ�����ṩ����صĻ�������
        /// </summary>
        /// <param name="ip">Զ���ս��IP��ַ</param>
        /// <param name="port">Զ���ս��˿�</param>
        /// <exception cref="ArgumentNullException">��������</exception>
        public TcpTransportChannel(string ip, int port)
            : this(new IPEndPoint(IPAddress.Parse(ip), port))
        { }

        /// <summary>
        ///     ����TCPЭ��Ĵ���ͨ�����ṩ����صĻ�������
        /// </summary>
        /// <param name="iep">Զ���ս���ַ</param>
        /// <exception cref="ArgumentNullException">��������</exception>
        public TcpTransportChannel(IPEndPoint iep)
        {
            if (iep == null) throw new ArgumentNullException("iep");
            _supportSegment = true;
            _address = iep;
            InitializeStatistics();
        }

        /// <summary>
        ///     ����TCPЭ��Ĵ���ͨ�����ṩ����صĻ�������
        /// </summary>
        /// <param name="socket" type="System.Net.Sockets.Socket">�����׽���</param>
        /// <exception cref="ArgumentNullException">��������</exception>
        public TcpTransportChannel(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            _supportSegment = true;
            _socket = socket;
            _connected = _socket.Connected;
            _channelKey = _socket.GetHashCode();
            InitializeStatistics();
        }

        #endregion

        #region ����

        /// <summary>
        ///     ��ʼ��ͳ����
        /// </summary>
        protected void InitializeStatistics()
        {
            #if (DEBUG)
            {
                _statistics = new Dictionary<StatisticTypes, IStatistic>();
                TcpTransportChannelStatistic statistic = new TcpTransportChannelStatistic();
                statistic.Initialize(this);
                _statistics.Add(StatisticTypes.Network, statistic);
            }
            #endif          
        }

        /// <summary>
        ///     ��ʼ����Ϣ������
        /// </summary>
        protected void InitializeReceiver()
        {
            if (_receiver != null) return;
            _receiver = new TcpAsynDataRecevier(_socket);
            _receiver.ReceivedData += RecvData;
            _receiver.Disconnected += ReceiverDisconnected;
            _receiver.Start();
        }

        #endregion

        #region Overrides of ServiceChannel

        /// <summary>
        ///     ֹͣ
        /// </summary>
        protected override void InnerAbort()
        {
            InnerClose();
        }

        /// <summary>
        ///     ��
        /// </summary>
        protected override void InnerOpen()
        {
            if (!_connected) throw new System.Exception("Cannot open a tcp transport channel, because current channel has been disconnected.");
            InitializeReceiver();
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
        ///   ��ȡͨ���ŵ�������
        /// </summary>
        public override TransportChannelTypes ChannelType
        {
            get { return TransportChannelTypes.TCP; }
        }

        /// <summary>
        ///     ��ȡ�������ӳ�����
        /// </summary>
        /// <exception cref="System.Exception">��Ч��Socket</exception>
        public override LingerOption LingerState
        {
            get
            {
                if (!_socket.Connected)
                    throw new System.Exception("You cannot GET linger option at a disconnected socket.");
                return _lingerState = _socket.LingerState;
            }
            set
            {
                if (!_socket.Connected)
                    throw new System.Exception("You cannot SET linger option at a disconnected socket.");
                _socket.LingerState = value;
                base.LingerState = value;
            }
        }

        /// <summary>
        ///     ��ȡ�ڲ������׽���
        /// </summary>
        /// <returns>�����ڲ������׽���</returns>
        public Socket GetStream()
        {
            return _socket;
        }

        /// <summary>
        ///     ��ȡ��ǰTCPЭ�鴫��ͨ����Ψһ��ֵ
        /// </summary>
        public int ChannelKey
        {
            get { return _channelKey; }
        }

        /// <summary>
        ///     ���ӵ�Զ���ս���ַ
        /// </summary>
        public override void Connect()
        {
            _communicationState = CommunicationStates.Opening;
            if (_socket != null)
            {
                if (_socket.Connected)
                {
                    try
                    {
                        _socket.Shutdown(SocketShutdown.Both);
                        _socket.Close();
                    }
                    catch { }
                }
                _socket = null;
            }
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                //�������ӿ��ܻ�����쳣������������²��������׳�����Ϊ�����û�֪����
                _socket.Connect(_address);
                _connected = _socket.Connected;
                if (!_connected)
                {
                    _communicationState = CommunicationStates.Closed;
                    return;
                }
                _communicationState = CommunicationStates.Opened;
                _channelKey = _socket.GetHashCode();
                InitializeReceiver();
            }
            catch
            {
                _connected = false;
                _communicationState = CommunicationStates.Closed;
            }
        }

        /// <summary>
        ///     �Ͽ�
        /// </summary>
        public override void Disconnect()
        {
            try
            {
                _communicationState = CommunicationStates.Closing;
                if (_receiver != null)
                {
                    _receiver.ReceivedData -= RecvData;
                    _receiver.Disconnected -= ReceiverDisconnected;
                    _receiver.Stop();
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
            }
            finally
            {
                _receiver = null;
                _connected = false;
                _communicationState = CommunicationStates.Closed;
                DisconnectedHandler(null);
            }
        }

        /// <summary>
        ///     ���³��Խ�������
        /// </summary>
        /// <returns>���س��Ժ��״̬</returns>
        public bool Reconnect()
        {
            Connect();
            return _connected;
        }

        /// <summary>
        ///     ��������
        ///     <para>* ����˷������з��͵�Ԫ���ݣ��������Զ��ְ�������ݡ�</para>
        /// </summary>
        /// <param name="data">Ҫ���͵�����</param>
        /// <returns>���ط��͵��ֽ���</returns>
        protected override int InnerSend(byte[] data)
        {
            int sendCount = 1;
            try
            {
                if (_socket == null || !_socket.Connected) return -1;
                if (data.Length > ChannelConst.MaxMessageDataLength)
                {
                    _tracing.Warn(string.Format("#Illegal data size: {0}, current allow size: {1}", data.Length, ChannelConst.MaxMessageDataLength));
                    return -1;
                }
                //get fixed cache.
                IFixedCacheStub<NoBuffSocketStub> stub = ChannelConst.NoBuffAsyncStubPool.Rent();
                if (stub == null)
                {
                    _tracing.Warn("Cannot rent a socket async event args cache.");
                    return -2;
                }
                stub.Tag = this;
                SocketAsyncEventArgs args = stub.Cache.Target;
                args.SetBuffer(data, 0, data.Length);
                args.UserToken = stub;
                bool result;
                using (ExecutionContext.SuppressFlow())
                    result = _socket.SendAsync(args);
                //ͬ���������
                if (!result)
                {
                    //ע�⣬�����������ͬ����ɵģ�����MSDN�Ľ��ͣ������ᴥ��Completed�¼�
                    sendCount = args.BytesTransferred;
                    ChannelConst.NoBuffAsyncStubPool.Giveback(stub);
                }
                return sendCount;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return sendCount;
            }
        }

        #endregion

        #region �¼�

        /// <summary>
        ///   ��������
        ///   <para>* IOCP���������߳����뺯��</para>  
        /// </summary>
        protected virtual void RecvData(object sender, SegmentReceiveEventArgs e)
        {
            ReceivedDataSegmentHandler(e);
        }

        void ReceiverDisconnected(object sender, RecevierDisconnectedEventArgs e)
        {
            try
            {
                if (_receiver != null)
                {
                    _receiver.ReceivedData -= RecvData;
                    _receiver.Disconnected -= ReceiverDisconnected;
                    _receiver = null;
                }
            }
            catch(System.Exception ex) { _tracing.Error(ex, null); }
            finally
            {
                _connected = false;
                _communicationState = CommunicationStates.Closed;
                DisconnectedHandler(null);
            }
        }

        #endregion
    }
}