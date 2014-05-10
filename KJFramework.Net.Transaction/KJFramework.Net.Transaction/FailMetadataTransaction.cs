using System.Net;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///   失败的消息事务
    /// </summary>
    public class FailMetadataTransaction : MetadataMessageTransaction
    {
        #region Constrcutor

        /// <summary>
        ///     失败的消息事务，提供了相关的基本操作
        /// </summary>
        public FailMetadataTransaction(string iep)
        {
            _iep = iep;
            Identity = new ErrorTransactionIdentity { EndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 1000) };
        }

        /// <summary>
        ///     失败的消息事务，提供了相关的基本操作
        /// </summary>
        /// <param name="channel">消息通信信道</param>
        public FailMetadataTransaction(IMessageTransportChannel<MetadataContainer> channel)
            : base(channel)
        {
            Identity = new ErrorTransactionIdentity { EndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 1000) };
        }

        /// <summary>
        ///     失败的消息事务，提供了相关的基本操作
        /// </summary>
        /// <param name="lease">事务生命租期租约</param>
        /// <param name="channel">消息通信信道</param>
        public FailMetadataTransaction(ILease lease, IMessageTransportChannel<MetadataContainer> channel)
            : base(lease, channel)
        {
            Identity = new ErrorTransactionIdentity { EndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 1000) };
        }

        #endregion

        #region Members

        private readonly string _iep;
        private static ITracing _tracing = TracingManager.GetTracing(typeof(FailMessageTransaction));

        #endregion

        #region Methods

        /// <summary>
        ///     发送一个请求消息
        /// </summary>
        /// <param name="message">请求消息</param>
        public override void SendRequest(MetadataContainer message)
        {
            _tracing.Error("Cannot call method FailWXMessageTransaction.SendRequest! #addr: " + _iep);
            FailedHandler(null);
        }

        /// <summary>
        ///     发送一个响应消息
        /// </summary>
        /// <param name="message">响应消息</param>
        public override void SendResponse(MetadataContainer message)
        {
            _tracing.Error("Cannot call method FailWXMessageTransaction.SendResponse! #addr: " + _iep);
            FailedHandler(null);
        }

        #endregion
    }
}