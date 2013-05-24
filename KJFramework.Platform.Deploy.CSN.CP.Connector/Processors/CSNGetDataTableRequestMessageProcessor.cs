using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Tracing;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Processors
{
    /// <summary>
    ///     获取数据表信息请求消息处理器
    /// </summary>
    public class CSNGetDataTableRequestMessageProcessor : IMessageProcessor
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(CSNGetDataTableRequestMessageProcessor));

        #endregion

        public void Process(BusinessMessageTransaction transaction)
        {
            CSNGetDataTableRequestMessage msg = (CSNGetDataTableRequestMessage)transaction.Request;
            try
            {
                CSNGetDataTableResponseMessage rspMsg = new CSNGetDataTableResponseMessage();
                #region Get data table request message [DB data]

                try
                {
                    rspMsg.Tables = Global.DBCacheFactory.Create(msg.DatabaseName, msg.TableName).Item;
                }
                catch (System.Exception ex)
                {
                    _tracing.Error(ex, null);
                    rspMsg.LastError = ex.Message;
                }

                #endregion
                transaction.SendResponse(rspMsg);
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                transaction.SendResponse(new CSNGetDataTableResponseMessage { LastError = "Can not get data table config infomation, because there has some errors happened." });
            }
        }
    }
}