using System;
using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Data.Synchronization.Transactions
{
    /// <summary>
    ///     同步数据事务
    /// </summary>
    internal class SyncDataTransaction : BusinessMessageTransaction
    {
        #region Constructor

        /// <summary>
        ///     基础业务消息事务，提供了相关的基本操作
        /// </summary>
        public SyncDataTransaction()
        {
        }

        /// <summary>
        ///     基础业务消息事务，提供了相关的基本操作
        /// </summary>
        /// <param name="channel">消息通信信道</param>
        public SyncDataTransaction(IMessageTransportChannel<BaseMessage> channel)
            : base(channel)
        {
        }

        /// <summary>
        ///     基础业务消息事务，提供了相关的基本操作
        /// </summary>
        /// <param name="lease">事务生命租期租约</param>
        /// <param name="channel">消息通信信道</param>
        public SyncDataTransaction(ILease lease, IMessageTransportChannel<BaseMessage> channel)
            : base(lease, channel)
        {
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置重试次数
        /// </summary>
        public int RetryCount { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     设置当前事物为已经超时的状态
        /// </summary>
        public void SetTimeout()
        {
            InnerSetTimeout();
        }

        #endregion

        #region Overrides of MessageTransaction<BaseMessage>

        /// <summary>
        ///     发送一个请求消息
        /// </summary>
        /// <param name="message">请求消息</param>
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
        ///     发送一个响应消息
        /// </summary>
        /// <param name="message">响应消息</param>
        public new void SendResponse(BaseMessage message)
        {
            SendResponse(message, true);
        }

        /// <summary>
        ///     发送一个响应消息
        /// </summary>
        /// <param name="message">响应消息</param>
        /// <param name="autoTransactionIdentity">一个标示，指示了当前是否在应答消息中加入请求消息的事务唯一标示 </param>
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