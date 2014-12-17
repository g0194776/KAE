using System.Net;
using KJFramework.Net.Channels.Identities;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///   失败的消息事务
    /// </summary>
    public class FailMessageTransaction<TMessage> : MessageTransaction<TMessage> 
    {
        #region Constrcutor

        /// <summary>
        ///     失败的消息事务，提供了相关的基本操作
        /// </summary>
        public FailMessageTransaction(string iep)
        {
            _iep = iep;
            Identity = new ErrorTransactionIdentity { EndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 1000) };
        }

        #endregion

        #region Members

        private readonly string _iep;
        private static ITracing _tracing = TracingManager.GetTracing(typeof(FailMessageTransaction<TMessage>));

        #endregion

        #region Methods

        /// <summary>
        ///     发送一个请求消息
        /// </summary>
        /// <param name="message">请求消息</param>
        public override void SendRequest(TMessage message)
        {
            _tracing.Error("Cannot call method FailMessageTransaction.SendRequest! #addr: " + _iep);
            FailedHandler(null);
        }

        /// <summary>
        ///     发送一个响应消息
        /// </summary>
        /// <param name="message">响应消息</param>
        public override void SendResponse(TMessage message)
        {
            _tracing.Error("Cannot call method FailMessageTransaction.SendResponse! #addr: " + _iep);
            FailedHandler(null);
        }

        #endregion
    }
}