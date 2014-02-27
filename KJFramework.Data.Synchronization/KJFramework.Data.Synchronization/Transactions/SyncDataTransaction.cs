using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;
using System;

namespace KJFramework.Data.Synchronization.Transactions
{
    /// <summary>
    ///     ͬ����������
    /// </summary>
    internal class SyncDataTransaction : MetadataMessageTransaction
    {
        #region Constructor

        /// <summary>
        ///     ����ҵ����Ϣ�����ṩ����صĻ�������
        /// </summary>
        public SyncDataTransaction()
        {
        }

        /// <summary>
        ///     ����ҵ����Ϣ�����ṩ����صĻ�������
        /// </summary>
        /// <param name="channel">��Ϣͨ���ŵ�</param>
        public SyncDataTransaction(IMessageTransportChannel<MetadataContainer> channel)
            : base(channel)
        {
        }

        /// <summary>
        ///     ����ҵ����Ϣ�����ṩ����صĻ�������
        /// </summary>
        /// <param name="lease">��������������Լ</param>
        /// <param name="channel">��Ϣͨ���ŵ�</param>
        public SyncDataTransaction(ILease lease, IMessageTransportChannel<MetadataContainer> channel)
            : base(lease, channel)
        {
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ���������Դ���
        /// </summary>
        public int RetryCount { get; set; }
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (SyncDataTransaction));

        #endregion

        #region Methods

        /// <summary>
        ///     ���õ�ǰ����Ϊ�Ѿ���ʱ��״̬
        /// </summary>
        public void SetTimeout()
        {
            InnerSetTimeout();
        }

        #endregion

        #region Overrides of MessageTransaction<BaseMessage>

        /// <summary>
        ///     ����һ��������Ϣ
        /// </summary>
        /// <param name="message">������Ϣ</param>
        public override void SendRequest(MetadataContainer message)
        {
            if (message == null) return;
            Identity.IsRequest = true;
            message.SetAttribute(0x01, new TransactionIdentityValueStored(Identity));
            _request = message;
            if (!_channel.IsConnected)
            {
                _tracing.Warn("Cannot send a response message to {0}, because target msg channel has been disconnected.", _channel.RemoteEndPoint);
                SyncDataTransactionManager.Instance.Remove(Identity);
                FailedHandler(null);
                return;
            }
            try
            {
                int sendCount;
                //send failed.
                if ((sendCount = _channel.Send(message)) < 0)
                {
                    SyncDataTransactionManager.Instance.Remove(Identity);
                    _channel.Disconnect();
                    FailedHandler(null);
                    return;
                }
                //calc REQ time.
                RequestTime = DateTime.Now;
                _tracing.Info("SendCount: {0}\r\nL: {1}\r\nR: {2}\r\n{3}", sendCount, _channel.LocalEndPoint, _channel.RemoteEndPoint, message.ToString());
            }
            catch
            {
                SyncDataTransactionManager.Instance.Remove(Identity);
                _channel.Disconnect();
                FailedHandler(null);
            }
        }

        /// <summary>
        ///     ����һ����Ӧ��Ϣ
        /// </summary>
        /// <param name="message">��Ӧ��Ϣ</param>
        public override void SendResponse(MetadataContainer message)
        {
            SendResponse(message, true);
        }

        /// <summary>
        ///     ����һ����Ӧ��Ϣ
        /// </summary>
        /// <param name="message">��Ӧ��Ϣ</param>
        /// <param name="autoTransactionIdentity">һ����ʾ��ָʾ�˵�ǰ�Ƿ���Ӧ����Ϣ�м���������Ϣ������Ψһ��ʾ </param>
        public void SendResponse(MetadataContainer message, bool autoTransactionIdentity)
        {
            _response = message;
            if (message == null || Identity.IsOneway) return;
            if (!message.IsAttibuteExsits(0x00)) throw new ArgumentException("#Current RSP message dose not contain any Message-Identity infomation.");
            MessageIdentity mIdentity = message.GetAttributeAsType<MessageIdentity>(0x00);
            TransactionIdentityValueStored valueStored = (TransactionIdentityValueStored)_request.GetAttribute(0x01);
            TransactionIdentity tIdentity;
            if (valueStored != null)
            {
                tIdentity = valueStored.GetValue<TransactionIdentity>();
                tIdentity.IsRequest = false;
                message.SetAttribute(0x01, new TransactionIdentityValueStored(tIdentity));
            }
            if (!_channel.IsConnected)
            {
                _tracing.Warn("Cannot send a response message to {0}, because target msg channel has been disconnected.", _channel.RemoteEndPoint);
                return;
            }
            try
            {
                int sendCount = _channel.Send(message);
                if (sendCount == 0)
                {
                    _tracing.Warn(
                        "#Cannot send binary data to remote endpoint, serialized data maybe is null. \r\n#Ref protocol P: {0}, S: {1}, D: {2}\r\n#Ref Instant Message: \r\n{3}",
                        mIdentity.ProtocolId,
                        mIdentity.ServiceId,
                        mIdentity.DetailsId,
                        message);
                    return;
                }
                //send failed.
                if (sendCount < 0)
                {
                    _channel.Disconnect();
                    FailedHandler(null);
                    return;
                }
                //calc RSP time.
                ResponseTime = DateTime.Now;
                _tracing.Info("SendCount: {0}\r\nL: {1}\r\nR: {2}\r\n{3}", sendCount, _channel.LocalEndPoint, _channel.RemoteEndPoint, message.ToString());
            }
            catch
            {
                _channel.Disconnect();
                FailedHandler(null);
            }
        }


        #endregion
    }
}