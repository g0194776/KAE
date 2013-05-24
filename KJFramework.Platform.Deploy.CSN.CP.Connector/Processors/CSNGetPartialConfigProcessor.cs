using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Tracing;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Processors
{
    /// <summary>
    ///     获取部分配置信息请求消息处理器
    /// </summary>
    public class CSNGetPartialConfigProcessor : IMessageProcessor
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (CSNGetPartialConfigProcessor));

        #endregion

        #region Implementation of IMessageProcessor

        /// <summary>
        ///     处理一个事务
        /// </summary>
        /// <param name="transaction">消息事务</param>
        public void Process(BusinessMessageTransaction transaction)
        {
            CSNGetPartialConfigRequestMessage msg = (CSNGetPartialConfigRequestMessage)transaction.Request;
            CSNGetPartialConfigResponseMessage rsp = new CSNGetPartialConfigResponseMessage();
            try
            {
                rsp.Config = Global.DBCacheFactory.Create(msg.Key);
                transaction.SendResponse(rsp);
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                transaction.SendResponse(new CSNGetPartialConfigResponseMessage {ErrorId = 255, LastError = ex.Message});
            }
        }

        #endregion
    }
}