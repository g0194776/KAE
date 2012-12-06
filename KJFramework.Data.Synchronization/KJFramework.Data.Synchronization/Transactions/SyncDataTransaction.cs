using System;
using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Data.Synchronization.Transactions
{
    /// <summary>
    ///     ͬ����������
    /// </summary>
    internal class SyncDataTransaction : BusinessMessageTransaction
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
        public SyncDataTransaction(IMessageTransportChannel<BaseMessage> channel)
            : base(channel)
        {
        }

        /// <summary>
        ///     ����ҵ����Ϣ�����ṩ����صĻ�������
        /// </summary>
        /// <param name="lease">��������������Լ</param>
        /// <param name="channel">��Ϣͨ���ŵ�</param>
        public SyncDataTransaction(ILease lease, IMessageTransportChannel<BaseMessage> channel)
            : base(lease, channel)
        {
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ���������Դ���
        /// </summary>
        public int RetryCount { get; set; }

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
        public new void SendRequest(BaseMessage message)
        {
            if (message == null) return;
            message.TransactionIdentity = Identity;
            message.TransactionIdentity.IsRequest = true;
            _request = message;
            if (!_channel.IsConnected)
            {
                Logs.Logger.Log(string.Format("Cannot send a response message to {0}, because target msg channel has been disconnected.", _channel.RemoteEndPoint));
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
                //30s
                GetLease().Change(DateTime.Now.AddSeconds(30));
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
        public new void SendResponse(BaseMessage message)
        {
            SendResponse(message, true);
        }

        /// <summary>
        ///     ����һ����Ӧ��Ϣ
        /// </summary>
        /// <param name="message">��Ӧ��Ϣ</param>
        /// <param name="autoTransactionIdentity">һ����ʾ��ָʾ�˵�ǰ�Ƿ���Ӧ����Ϣ�м���������Ϣ������Ψһ��ʾ </param>
        public new void SendResponse(BaseMessage message, bool autoTransactionIdentity)
        {
            _response = message;
            if (message == null || Identity.IsOneway) return;
            if (_request.TransactionIdentity != null)
            {
                message.TransactionIdentity = Identity;
                message.TransactionIdentity.IsRequest = false;
            }
            //the same tid for client.
            if (Request != null && message.MessageIdentity != null) message.MessageIdentity.Tid = Request.MessageIdentity.Tid;
            if (!_channel.IsConnected)
            {
                Logs.Logger.Log(string.Format("Cannot send a response message to {0}, because target msg channel has been disconnected.", _channel.RemoteEndPoint));
                return;
            }
            try
            {
                int sendCount = _channel.Send(message);
                if (sendCount == 0)
                {
                    _tracing.Warn(
                        "#Cannot send binary data to remote endpoint, serialized data maybe is null. \r\n#Ref protocol P: {0}, S: {1}, D: {2}\r\n#Ref Instant Message: \r\n{3}",
                        message.MessageIdentity.ProtocolId,
                        message.MessageIdentity.ServiceId,
                        message.MessageIdentity.DetailsId,
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