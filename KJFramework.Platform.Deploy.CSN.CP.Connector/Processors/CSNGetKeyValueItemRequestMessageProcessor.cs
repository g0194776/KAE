using System;
using KJFramework.Logger;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Processors
{
    public class CSNGetKeyValueItemRequestMessageProcessor : IMessageProcessor
    {
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
                        Logs.Logger.Log(ex);
                        rspMsg.LastError = ex.Message;
                    }
                }

                #endregion
                transaction.SendResponse(rspMsg);
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                transaction.SendResponse(new CSNGetDataTableResponseMessage { LastError = "Can not get data table config infomation, because there has some errors happened." });
            }
        }
    }
}