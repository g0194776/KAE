using System;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///   基于元数据网络通信协议的消息事务
    /// </summary>
    public class MetadataMessageTransaction : MessageTransaction<MetadataContainer>
    {
        #region Constructor

        /// <summary>
        ///     基于元数据网络通信协议的消息事务，提供了相关的基本操作
        /// </summary>
        public MetadataMessageTransaction()
        {
        }

        /// <summary>
        ///     基于元数据网络通信协议的消息事务，提供了相关的基本操作
        /// </summary>
        /// <param name="channel">消息通信信道</param>
        public MetadataMessageTransaction(IMessageTransportChannel<MetadataContainer> channel)
            : base(channel)
        {
        }

        /// <summary>
        ///     基于元数据网络通信协议的消息事务，提供了相关的基本操作
        /// </summary>
        /// <param name="lease">事务生命租期租约</param>
        /// <param name="channel">消息通信信道</param>
        public MetadataMessageTransaction(ILease lease, IMessageTransportChannel<MetadataContainer> channel)
            : base(lease, channel)
        {
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置事务管理器
        /// </summary>
        public MetadataTransactionManager TransactionManager { get; set; }
        protected static readonly ITracing _tracing = TracingManager.GetTracing(typeof(MetadataMessageTransaction));
        /// <summary>
        ///     获取或设置请求消息
        /// </summary>
        public override MetadataContainer Request
        {
            get
            {
                return base.Request;
            }
            set
            {
                //calc REQ time.
                RequestTime = DateTime.Now;
                base.Request = value;
            }
        }

        #endregion

        #region Overrides of MessageTransaction<BaseMessage>

        /// <summary>
        ///     发送一个应答消息
        /// </summary>
        /// <param name="response">要发送的应答消息</param>
        public override void SetResponse(MetadataContainer response)
        {
            //calc RSP time.
            ResponseTime = DateTime.Now;
            base.SetResponse(response);
        }

        /// <summary>
        ///     发送一个请求消息
        /// </summary>
        /// <param name="message">请求消息</param>
        public override void SendRequest(MetadataContainer message)
        {
            if (message == null) return;
            Identity.IsRequest = true;
            message.SetAttribute(0x01, new TransactionIdentityValueStored(Identity));
            message.SetAttribute(0x02, new DateTimeValueStored(_lease.ExpireTime));
            _request = message;
            if (!_channel.IsConnected)
            {
                _tracing.Warn("Cannot send a response message to {0}, because target msg channel has been disconnected.", _channel.RemoteEndPoint);
                TransactionManager.Remove(Identity);
                FailedHandler(null);
                return;
            }
            try
            {
                int sendCount;
                //send failed.
                if ((sendCount = _channel.Send(message)) < 0)
                {
                    TransactionManager.Remove(Identity);
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
                TransactionManager.Remove(Identity);
                _channel.Disconnect();
                FailedHandler(null);
            }
        }

        /// <summary>
        ///     发送一个响应消息
        /// </summary>
        /// <param name="message">响应消息</param>
        public override void SendResponse(MetadataContainer message)
        {
            SendResponse(message, true);
        }

        /// <summary>
        ///     发送一个响应消息
        /// </summary>
        /// <param name="message">响应消息</param>
        /// <param name="autoTransactionIdentity">一个标示，指示了当前是否在应答消息中加入请求消息的事务唯一标示 </param>
        public void SendResponse(MetadataContainer message, bool autoTransactionIdentity)
        {
            _response = message;
            if (message == null || Identity.IsOneway) return;
            //Automatically calculate the RSP message's protocol.
            MessageIdentity mIdentity = _request.GetAttributeAsType<MessageIdentity>(0x00);
            mIdentity.DetailsId += 1;
            message.SetAttribute(0x00, new MessageIdentityValueStored(mIdentity));
            TransactionIdentityValueStored valueStored = (TransactionIdentityValueStored) _request.GetAttribute(0x01);
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