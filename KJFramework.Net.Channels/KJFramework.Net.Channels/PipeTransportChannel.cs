using System;
using System.IO.Pipes;
using System.Net;
using System.Threading;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;
using KJFramework.Logger;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.Transactions;
using KJFramework.Net.Channels.Uri;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///    ����IPCͨ���Ĵ���ͨ�����ṩ����صĻ���������
    /// </summary>
    public class PipeTransportChannel : TransportChannel, IReconnectionTransportChannel
    {
        #region ��Ա

        protected PipeStream _stream;
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
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰͨ���Ƿ�������״̬
        /// </summary>
        public override bool IsConnected
        {
            get { return (_connected = _client.IsConnected); }
        }

        private readonly Action<byte[]> _callback;
        protected NamedPipeClientStream _client;
        protected Thread _thread;
        protected ServerPipeStreamTransaction _servTransaction;
        protected ClientPipeStreamTransaction _clientTransaction;

        #endregion

        #region ���캯��

        /// <summary>
        ///    ����IPCͨ���Ĵ���ͨ�����ṩ����صĻ���������
        /// </summary>
        /// <param name="logicalUri">ͨ����ַ</param>
        /// <exception cref="ArgumentNullException">��������</exception>
        public PipeTransportChannel(Uri.Uri logicalUri)
        {
            if (logicalUri == null)
            {
                throw new ArgumentNullException("logicalUri");
            }
            _logicalAddress = logicalUri;
            _callback = DefaultCallback;
        }

        /// <summary>
        ///    ����IPCͨ���Ĵ���ͨ�����ṩ����صĻ���������
        /// </summary>
        /// <param name="stream" type="System.IO.Pipes.PipeStream">PIPE��</param>
        /// <exception cref="ArgumentNullException">��������</exception>
        public PipeTransportChannel(PipeStream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            _stream = stream;
            _callback = DefaultCallback;
            _connected = stream.IsConnected;
            if (_stream is NamedPipeServerStream)
            {
                InitializeServerTransaction();
            }
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
            try
            {
                if (_logicalAddress == null)
                {
                    throw new System.Exception("δ�ṩԶ���ն˵�ַ��");
                }
                if (_client == null)
                {
                    PipeUri uri = new PipeUri(_logicalAddress.ToString());
                    _client = new NamedPipeClientStream(uri.MachineName, uri.PipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
                }
                _communicationState = CommunicationStates.Opening;
                _client.Connect(100);
                _stream = _client;
                _connected = _client.IsConnected;
                if (!_connected)
                {
                    _communicationState = CommunicationStates.Closed;
                    return;
                }
                _communicationState = CommunicationStates.Opened;
                InitializeClientTransaction();
                ConnectedHandler(null);
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
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
                if (_servTransaction != null)
                {
                    _servTransaction.Dispose();
                    _servTransaction = null;
                }
                if (_clientTransaction != null)
                {
                    _clientTransaction.Dispose();
                    _clientTransaction = null;
                }
                if (_stream != null)
                {
                    try
                    {
                        if (_stream is NamedPipeServerStream)
                        {
                            (_stream as NamedPipeServerStream).Disconnect();
                        }
                        _stream.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Logs.Logger.Log(ex, DebugGrade.Standard, Logs.Name);
                    }
                    _stream = null;
                }
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
            }
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
        public bool Reconnect()
        {
            try
            {
                Connect();
                return _connected;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return false;
            }
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
                    Logs.Logger.Log(string.Format("#Illegal data size: {0}, current allow size: {1}", data.Length, ChannelConst.MaxMessageDataLength));
                    return -1;
                }
                //�ж���״̬
                if (_stream != null && _stream.CanWrite)
                {
                    _stream.BeginWrite(data, 0, data.Length, SendCallback, null);
                    return data.Length;
                }
                return -1;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex, DebugGrade.Standard, Logs.Name);
                return -1;
            }
        }

        #endregion

        #region �¼�

        //����Ͽ��¼�
        void TransactionDisconnected(object sender, System.EventArgs e)
        {
            if (_servTransaction != null)
            {
                _servTransaction.Disconnected -= TransactionDisconnected;
                _servTransaction = null;
            }
            if (_clientTransaction != null)
            {
                _clientTransaction.Disconnected -= TransactionDisconnected;
                _clientTransaction = null;
            }
            _stream = null;
            _connected = false;
            _communicationState = CommunicationStates.Closed;
            DisconnectedHandler(null);
        }

        #endregion

        #region ����

        /// <summary>
        ///     ��ʼ������
        /// </summary>
        protected void InitializeServerTransaction()
        {
            _servTransaction = new ServerPipeStreamTransaction((NamedPipeServerStream)_stream, true, _callback);
            _servTransaction.Disconnected += TransactionDisconnected;
        }

        /// <summary>
        ///     ��ʼ������
        /// </summary>
        protected virtual void InitializeClientTransaction()
        {
            _clientTransaction = new ClientPipeStreamTransaction((NamedPipeClientStream)_stream, true, _callback);
            _clientTransaction.Disconnected += TransactionDisconnected;
        }

        /// <summary>
        ///     Ĭ�ϵ���Ϣ�ص�����
        /// </summary>
        /// <param name="data" type="byte[]">���յ�������</param>
        protected void DefaultCallback(byte[] data)
        {
            ReceivedDataHandler(new LightSingleArgEventArgs<byte[]>(data));
        }

        //send async callback.
        private void SendCallback(IAsyncResult result)
        {
            try
            {
                _stream.EndWrite(result);
                _stream.Flush();
            }
            catch (System.Exception ex) { Logs.Logger.Log(ex); }
        }

        #endregion
    }
}