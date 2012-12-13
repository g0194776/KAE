using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Tracing;
using System;

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

                string[] tables = msg.TableName.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                rspMsg.Tables = new DataTable[tables.Length];
                for (int i = 0; i < tables.Length; i++)
                {
                    try
                    {
                        rspMsg.Tables[i] = Global.DBCacheFactory.Create(msg.DatabaseName, tables[i]).Item;
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, null);
                        rspMsg.LastError = ex.Message;
                    }
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