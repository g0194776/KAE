using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Tracing;
using System;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Processors
{
    public class CSNGetKeyValueItemRequestMessageProcessor : IMessageProcessor
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(CSNGetKeyValueItemRequestMessageProcessor));

        #endregion

        public void Process(BusinessMessageTransaction transaction)
        {
            CSNGetKeyDataRequestMessage msg = (CSNGetKeyDataRequestMessage)transaction.Request;
            try
            {
                CSNGetKeyDataResponseMessage rspMsg = new CSNGetKeyDataResponseMessage();
                #region Get data table request message [DB data]

                string[] tables = msg.TableName.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < tables.Length; i++)
                {
                    try
                    {
                        rspMsg.Items = Global.DBCacheFactory.Create(msg.DatabaseName, msg.TableName, msg.ServiceName).Item;
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
                transaction.SendResponse(new CSNGetKeyDataResponseMessage { LastError = "Can not get data table config infomation, because there has some errors happened." });
            }
        }
    }
}