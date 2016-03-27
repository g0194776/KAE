using System;
using System.Net;
using System.Net.Sockets;
using KJFramework.Buffers;
using KJFramework.EventArgs;
using KJFramework.Net.Enums;
using KJFramework.Net.Events;

namespace KJFramework.Net
{
    /// <summary>
    ///     ����ͨ�������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class TransportChannel : ServiceChannel, IRawTransportChannel
    {
        #region Constructor
        
        /// <summary>
        ///     ����ͨ�������࣬�ṩ����صĻ���������
        /// </summary>
        protected TransportChannel()
        {
        }

        #endregion

        #region Implementation of ITransportChannel

        protected IPEndPoint _address;
        protected Uri.Uri _logicalAddress;
        protected bool _connected;
        protected bool _supportSegment;
        protected IByteArrayBuffer _buffer;
        protected LingerOption _lingerState;

        /// <summary>
        ///   ��ȡͨ���ŵ�������
        /// </summary>
        public abstract TransportChannelTypes ChannelType { get; }

        /// <summary>
        ///     ��ȡ�����ս���ַ
        /// </summary>
        public abstract EndPoint LocalEndPoint { get; }
        /// <summary>
        ///     ��ȡԶ���ս���ַ
        /// </summary>
        public abstract EndPoint RemoteEndPoint { get; }

        /// <summary>
        ///   ��ȡ������
        /// </summary>
        public IByteArrayBuffer Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        /// <summary>
        ///     ��ȡ�������ӳ�����
        /// </summary>
        public virtual LingerOption LingerState
        {
            get { return _lingerState; }
            set { _lingerState = value; }
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰͨ���Ƿ�������״̬
        /// </summary>
        public virtual bool IsConnected
        {
            get { return _connected; }
        }
        /// <summary>
        ///     ����
        /// </summary>
        public abstract void Connect();
        /// <summary>
        ///     �Ͽ�
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="data">Ҫ���͵�����</param>
        /// <returns>���ط��͵��ֽ���</returns>
        /// <exception cref="ArgumentNullException">��������</exception>
        public int Send(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            return InnerSend(data);
        }

        /// <summary>
        ///     ��������
        ///     <para>* ����˷������з��͵�Ԫ���ݣ��������Զ��ְ�������ݡ�</para>
        /// </summary>
        /// <param name="data">Ҫ���͵�����</param>
        /// <returns>���ط��͵��ֽ���</returns>
        protected abstract int InnerSend(byte[] data);

        /// <summary>
        ///     ͨ���������¼�
        /// </summary>
        public event EventHandler Connected;
        protected void ConnectedHandler(System.EventArgs e)
        {
            EventHandler connected = Connected;
            if (connected != null) connected(this, e);
        }
        /// <summary>
        ///     ͨ���ѶϿ��¼�
        /// </summary>
        public event EventHandler Disconnected;
        protected void DisconnectedHandler(System.EventArgs e)
        {
            EventHandler disconnected = Disconnected;
            if (disconnected != null) disconnected(this, e);
        }

        #endregion

        #region Implementation of ICommunicationChannelAddress

        /// <summary>
        ///     ��ȡ�����������ַ
        /// </summary>
        public IPEndPoint Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        ///     ��ȡ�������߼���ַ
        /// </summary>
        public Uri.Uri LogicalAddress
        {
            get { return _logicalAddress; }
            set { _logicalAddress = value; }
        }

        #endregion

        #region Implementation of IRawTransportChannel

        /// <summary>
        ///     ��ȡ�����õ�ǰԪ�����ŵ��Ƿ�֧����Ƭ�εķ�ʽ��������������
        /// </summary>
        public bool SupportSegment
        {
            get { return _supportSegment; }
            set { _supportSegment = value; }
        }

        /// <summary>
        ///     ���յ������¼�
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<byte[]>> ReceivedData;

        /// <summary>
        ///     ���յ�����Ƭ���¼�
        /// </summary>
        public event EventHandler<SegmentReceiveEventArgs> ReceivedDataSegment;
        protected void ReceivedDataSegmentHandler(SegmentReceiveEventArgs e)
        {
            EventHandler<SegmentReceiveEventArgs> handler = ReceivedDataSegment;
            if (handler != null) handler(this, e);
        }

        protected void ReceivedDataHandler(LightSingleArgEventArgs<byte[]> e)
        {
            EventHandler<LightSingleArgEventArgs<byte[]>> handler = ReceivedData;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}