using System;
using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction;
using KJFramework.ServiceModel.Bussiness.Default.Counters;
using KJFramework.ServiceModel.Bussiness.Default.Messages;
using KJFramework.Tracing;

namespace KJFramework.ServiceModel.Bussiness.Default.Transactions
{
    /// <summary>
    ///     远程调用事务
    /// </summary>
    internal class RPCTransaction : Transaction
    {
        #region Constructor

        /// <summary>
        ///     远程调用事务
        /// </summary>
        public RPCTransaction(TransactionIdentity identity, IMessageTransportChannel<Message> channel)
        {
            if (identity == null) throw new ArgumentNullException("identity");
            if (channel == null) throw new ArgumentNullException("channel");
            if (!channel.IsConnected) throw new ArgumentException("#Cannot initialize transaction, because target channel has been disconnected.");
            Identity = identity;
            _channel = channel;
        }

        #endregion

        #region Members

        protected ITracing _tracing = TracingManager.GetTracing(typeof (RPCTransaction));
        private readonly IMessageTransportChannel<Message> _channel;
        internal TransactionManager<RPCTransaction> TransactionManager;

        /// <summary>
        ///     获取事务唯一标示
        /// </summary>
        public TransactionIdentity Identity { get; private set; }
        /// <summary>
        ///     获取请求消息
        /// </summary>
        public Message Request { get; set; }
        /// <summary>
        ///     获取应答消息
        /// </summary>
        public Message Response { get; set; }

        #endregion

        #region Methods

        internal void SetResponse(Message message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Response = message;
            ResponseRecvHandler(new LightSingleArgEventArgs<Message>(message));
        }

        internal void SetTimeout()
        {
            TimeoutHandler(null);
        }

        /// <summary>
        ///     发送一个请求消息
        /// </summary>
        /// <param name="reqMsg">请求消息</param>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        public void SendRequest(Message reqMsg)
        {
            if (reqMsg == null) throw new ArgumentNullException("reqMsg");
            reqMsg.TransactionIdentity = (TransactionIdentity)Identity;
            Request = reqMsg;
            if(!_channel.IsConnected)
            {
                _tracing.Error(
                    "Cannot send request to remote end point ,because inner channel has been disconencted! #P: {0}, #S: {1}, #D: {2}",
                    reqMsg.MessageIdentity.ProtocolId, 
                    reqMsg.MessageIdentity.ServiceId,
                    reqMsg.MessageIdentity.DetailsId);
                TransactionManager.Remove(Identity);
                FailHandler(null);
                return;
            }
            //send failed.
            if (_channel.Send(reqMsg) < 0)
            {
                TransactionManager.Remove(Identity);
                _channel.Disconnect();
                FailHandler(null);
                return;
            }
            ServiceModelPerformanceCounter.Instance.RateOfRequest.Increment();
            _tracing.Info("L: {0}\r\nR: {1}\r\n{2}", _channel.LocalEndPoint, _channel.RemoteEndPoint, reqMsg.ToString());
        }

        /// <summary>
        ///     发送一个应答消息
        /// </summary>
        /// <param name="rspMsg">应答消息</param>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        public void SendResponse(Message rspMsg)
        {
            if (rspMsg == null) throw new ArgumentNullException("rspMsg");
            rspMsg.TransactionIdentity = (TransactionIdentity)Identity;
            Response = rspMsg;
            if (!_channel.IsConnected)
            {
                _tracing.Error(
                    "Cannot send response to remote end point ,because inner channel has been disconencted! #P: {0}, #S: {1}, #D: {2}",
                    rspMsg.MessageIdentity.ProtocolId,
                    rspMsg.MessageIdentity.ServiceId,
                    rspMsg.MessageIdentity.DetailsId);
                FailHandler(null);
                return;
            }
            //send failed.
            if (_channel.Send(rspMsg) < 0)
            {
                _channel.Disconnect();
                FailHandler(null);
                return;
            }
            ServiceModelPerformanceCounter.Instance.RateOfResponse.Increment();
            _tracing.Info("L: {0}\r\nR: {1}\r\n{2}", _channel.LocalEndPoint, _channel.RemoteEndPoint, rspMsg.ToString());
        }

        #endregion

        #region Events

        /// <summary>
        ///     接收到应答消息事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<Message>> ResponseRecv;
        protected void ResponseRecvHandler(LightSingleArgEventArgs<Message> e)
        {
            EventHandler<LightSingleArgEventArgs<Message>> handler = ResponseRecv;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     超时 
        /// </summary>
        public event EventHandler Timeout;
        protected void TimeoutHandler(System.EventArgs e)
        {
            EventHandler handler = Timeout;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     失败
        /// </summary>
        public event EventHandler Fail;
        protected void FailHandler(System.EventArgs e)
        {
            EventHandler handler = Fail;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}