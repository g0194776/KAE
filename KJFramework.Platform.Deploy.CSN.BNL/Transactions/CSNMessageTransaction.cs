using System;
using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction;
using KJFramework.Platform.Deploy.CSN.Common.Objects;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Tracing;

namespace KJFramework.Platform.Deploy.CSN.BNL.Transactions
{
    /// <summary>
    ///     CSN事务， 内部专用
    /// </summary>
    public class CSNMessageTransaction : MessageTransaction<CSNMessage>
    {
        #region Constructor

        /// <summary>
        ///     CSN事务， 内部专用
        /// </summary>
        public CSNMessageTransaction()
        {
            
        }

        /// <summary>
        ///     CSN事务， 内部专用
        /// </summary>
        /// <param name="channel">消息通信信道</param>
        public CSNMessageTransaction(IMessageTransportChannel<CSNMessage> channel)
            : base(channel)
        {
        }

        /// <summary>
        ///     CSN事务， 内部专用
        /// </summary>
        /// <param name="lease">事务生命租期租约</param>
        /// <param name="channel">消息通信信道</param>
        public CSNMessageTransaction(ILease lease, IMessageTransportChannel<CSNMessage> channel)
            : base(lease, channel)
        {
        }

        #endregion

        #region Members

        private static ITracing _tracing = TracingManager.GetTracing(typeof (CSNMessageTransaction));
        public CSNTransactionManager TransactionManager { get; set; }
        /// <summary>
        ///     获取或设置当前事务的唯一标示
        /// </summary>
        public CSNTransactionIdentity Identity { get; set; }

        #endregion

        #region Methods

        internal void SetTimeout()
        {
            TimeoutHandler(null);
        }

        #endregion

        #region Overrides of MessageTransaction<CSNMessage>

        /// <summary>
        /// 发送一个请求消息
        /// </summary>
        /// <param name="message">请求消息</param>
        public override void SendRequest(CSNMessage message)
        {
            if (message == null) return;
            message.TransactionIdentity = Identity;
            if (!_channel.IsConnected)
            {
                Logs.Logger.Log(string.Format("Cannot send a response message to {0}, because target msg channel has been disconnected.", _channel.RemoteEndPoint));
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
                _tracing.Info("SendCount: {0}\r\nL: {1}\r\nR: {2}\r\n{3}", sendCount, _channel.LocalEndPoint, _channel.RemoteEndPoint, message.ToString());
                //30s
                GetLease().Change(DateTime.Now.AddSeconds(30));
            }
            catch
            {
                TransactionManager.Remove(Identity);
                _channel.Disconnect();
                FailedHandler(null);
            }
        }

        /// <summary>
        /// 发送一个响应消息
        /// </summary>
        /// <param name="message">响应消息</param>
        public override void SendResponse(CSNMessage message)
        {
            message.TransactionIdentity = Identity;
            if (!_channel.IsConnected)
            {
                Logs.Logger.Log(string.Format("Cannot send a response message to {0}, because target msg channel has been disconnected.", _channel.RemoteEndPoint));
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
                _tracing.Info("SendCount: {0}\r\nL: {1}\r\nR: {2}\r\n{3}", sendCount, _channel.LocalEndPoint, _channel.RemoteEndPoint, message.ToString());
            }
            catch
            {
                TransactionManager.Remove(Identity);
                _channel.Disconnect();
                FailedHandler(null);
            }
            finally { TransactionManager.Remove(Identity); }
        }

        #endregion
    }
}