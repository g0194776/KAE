using System;
using System.IO.Pipes;
using System.Net;
using KJFramework.Cores;
using KJFramework.Net.Caches;
using KJFramework.Net.EndPoints;
using KJFramework.Net.Enums;
using KJFramework.Net.Events;
using KJFramework.Net.Transactions;
using KJFramework.Net.Uri;
using KJFramework.Tracing;

namespace KJFramework.Net
{
    /// <summary>
    ///    ����IPCͨ���Ĵ���ͨ�����ṩ����صĻ���������
    /// </summary>
    public class PipeTransportChannel : TransportChannel, IReconnectionTransportChannel
    {
        #region Members.

        private readonly NamedPipeEndPoint _endPoint;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(PipeTransportChannel));

        /// <summary>
        ///    ��ȡ�ڲ�������
        /// </summary>
        public PipeStream Stream
        {
            get { return _stream; }
        }

        /// <summary>
        ///     ��ȡ�����ս���ַ
        /// </summary>
        public override EndPoint LocalEndPoint { get { return _endPoint; } }

        /// <summary>
        ///     ��ȡ��������Ŀͻ��� IP ��ַ�Ͷ˿ں�
        /// </summary>
        public override EndPoint RemoteEndPoint { get { return _endPoint; } }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰͨ���Ƿ�������״̬
        /// </summary>
        public override bool IsConnected
        {
            get { return (_connected = _stream != null && _stream.IsConnected); }
        }

        protected PipeStream _stream;
        protected PipeStreamTransaction _streamAgent;
        private readonly Action<IFixedCacheStub<BuffStub>, int> _callback;

        #endregion

        #region Constructor.

        /// <summary>
        ///    ����IPCͨ���Ĵ���ͨ�����ṩ����صĻ���������
        ///     <para>* �˹��캯�����ڳ�ʼ��һ����Ҫ���ӵ�Զ�̵������ܵ�����</para>
        /// </summary>
        /// <param name="logicalUri">ͨ����ַ</param>
        /// <param name="numInstance">
        ///     ��ǰ�����ܵ�ʵ������index
        ///     <para>* index��0��ʼ����</para>
        /// </param>
        /// <exception cref="ArgumentNullException">��������</exception>
        public PipeTransportChannel(PipeUri logicalUri, byte numInstance = 0x00)
        {
            if (logicalUri == null) throw new ArgumentNullException("logicalUri");
            _logicalAddress = logicalUri;
            _callback = DefaultCallback;
            _supportSegment = true;
            _endPoint = new NamedPipeEndPoint(logicalUri, numInstance);
        }

        /// <summary>
        ///    ����IPCͨ���Ĵ���ͨ�����ṩ����صĻ���������
        ///     <para>* �˹��캯�����ڳ�ʼ��һ���Ѿ����ӵ������ܵ�������</para>
        /// </summary>
        /// <param name="uri">����ͨ����ַ</param>
        /// <param name="stream" type="System.IO.Pipes.PipeStream">PIPE��</param>
        /// <exception cref="ArgumentNullException">��������</exception>
        public PipeTransportChannel(PipeUri uri, PipeStream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            _stream = stream;
            _supportSegment = true;
            _callback = DefaultCallback;
            _connected = stream.IsConnected;
            _streamAgent = new PipeStreamTransaction(_stream, stream.IsAsync, _callback);
            _streamAgent.Disconnected += TransactionDisconnected;
            _endPoint = new NamedPipeEndPoint(uri, stream.GetHashCode());
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
            if(!_connected) Connect();
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
            get { return TransportChannelTypes.NamedPipe; }
        }

        /// <summary>
        ///     ���ӵ�Զ�������ܵ�
        ///     <para>* �˷�������ʹ��˫���������ķ�ʽ����Զ�������ܵ�</para>
        /// </summary>
        public override void Connect()
        {
            Connect(PipeDirection.InOut);
        }

        /// <summary>
        ///     ���ӵ�Զ�������ܵ�
        /// </summary>
        /// <param name="direction">����������</param>
        /// <param name="milliseconds">��ʱʱ��</param>
        /// <exception cref="ArgumentException">��Ч�ĳ�ʱʱ��</exception>
        /// <exception cref="InvalidOperationException">�޷��ٴε���һ���Ѿ���ʼ�����Channel��Connect����</exception>
        public virtual void Connect(PipeDirection direction, int milliseconds = 1000)
        {
            try
            {
                if (milliseconds == 0)throw new ArgumentException("#You cannot specified ZERO to timeout value.");
                if (_logicalAddress == null) throw new System.Exception("#You didn't offer any address which currently went to connect.");
                if (_stream != null && _stream.IsConnected) throw new InvalidOperationException("#You cannot did this operation again, because of current channel had been done with the conenction since it created.");
                PipeUri uri = new PipeUri(_logicalAddress.ToString());
                NamedPipeClientStream stream = new NamedPipeClientStream(uri.MachineName, uri.PipeName, direction, PipeOptions.Asynchronous);
                _communicationState = CommunicationStates.Opening;
                stream.Connect(milliseconds);
                _stream = stream;
                _connected = _stream.IsConnected;
                _streamAgent = new PipeStreamTransaction(_stream, stream.IsAsync, _callback);
                _streamAgent.Disconnected += TransactionDisconnected;
                _communicationState = CommunicationStates.Opened;
                ConnectedHandler(null);
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
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
                if (_streamAgent != null)
                {
                    _streamAgent.EndWork();
                    _streamAgent = null;
                }
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
            finally
            {
                _communicationState = CommunicationStates.Closed;
                DisconnectedHandler(null);
            }
        }

        /// <summary>
        ///     ���³��Խ�������
        /// </summary>
        /// <returns>���س��Ժ��״̬</returns>
        [Obsolete("#You cannot did this operation on current type of Transport Channel.", true)]
        public bool Reconnect()
        {
            throw new NotImplementedException("#You cannot did this operation on current type of Transport Channel.");
        }

        /// <summary>
        ///     ��������
        ///     <para>* ����˷������з��͵�Ԫ���ݣ��������Զ��ְ�������ݡ�</para>
        /// </summary>
        /// <param name="data">Ҫ���͵�����</param>
        /// <returns>���ط��͵��ֽ���</returns>
        protected override int InnerSend(byte[] data)
        {
            try
            {
                if (data.Length > ChannelConst.MaxMessageDataLength)
                {
                    _tracing.Warn(string.Format("#Illegal data size: {0}, current allow size: {1}", data.Length, ChannelConst.MaxMessageDataLength));
                    return -1;
                }
                //�ж���״̬
                if (_stream == null || !_stream.IsConnected)
                {
                    InnerClose();
                    return -2;
                }
                if (!_stream.CanWrite)
                {
                    _tracing.Warn("#You were trying to write some binary data into a read-only Named Pipe channel.", null);
                    return -3;
                }
                _stream.BeginWrite(data, 0, data.Length, SendCallback, null);
                return data.Length;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                InnerClose();
                return -2;
            }
        }

        #endregion

        #region Events.

        //����Ͽ��¼�
        void TransactionDisconnected(object sender, System.EventArgs e)
        {
            if (_streamAgent != null)
            {
                _streamAgent.Disconnected -= TransactionDisconnected;
                _streamAgent = null;
            }
            _connected = false;
            _communicationState = CommunicationStates.Closed;
            DisconnectedHandler(null);
        }

        #endregion

        #region Members.

        //callback function.
        protected virtual void DefaultCallback(IFixedCacheStub<BuffStub> stub, int bytesTransferred)
        {
            ReceivedDataSegmentHandler(new NamedPipeSegmentReceiveEventArgs(stub, bytesTransferred));
        }

        //send async callback.
        private void SendCallback(IAsyncResult result)
        {
            try
            {
                _stream.EndWrite(result);
                _stream.Flush();
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                InnerClose();
            }
        }

        #endregion
    }
}