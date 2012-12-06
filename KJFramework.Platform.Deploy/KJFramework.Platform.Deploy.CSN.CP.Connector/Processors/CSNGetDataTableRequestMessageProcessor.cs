using System;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Caches;
using KJFramework.Platform.Deploy.CSN.CP.Connector.SubscribeObjs;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Platform.Deploy.CSN.ProtocolStack.Enums;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Processors
{
    /// <summary>
    ///     获取数据表信息请求消息处理器
    /// </summary>
    public class CSNGetDataTableRequestMessageProcessor : FunctionProcessor<CSNMessage>
    {
        #region Overrides of FunctionProcessor<CSNMessage>

        public override CSNMessage Process(Guid id, CSNMessage message)
        {
            CSNGetDataTableResponseMessage responseMessage;
            CSNGetDataTableRequestMessage msg = (CSNGetDataTableRequestMessage)message;
            Console.WriteLine("#New get table data request message received, details below: ");
            Console.WriteLine("#Subscriber: " + msg.Header.ServiceKey);
            Console.WriteLine("#Session id: " + msg.Header.SessionId);
            Console.WriteLine("#Database: " + msg.DatabaseName ?? "*None*");
            Console.WriteLine("#Tables: " + msg.TableName ?? "*None*");
            try
            {
                IConfigSubscriber subscriber = ConfigSubscriberManager.GetSubscriber(msg.Header.ServiceKey);
                if (subscriber == null)
                {
                    responseMessage = new CSNGetDataTableResponseMessage { LastError = "Can not get data table config infomation, because not subscribed." };
                    responseMessage.Header.ServiceKey = msg.Header.ServiceKey;
                    return responseMessage;
                }
                IDBSubscribeObject dbSubscribeObject = subscriber.GetSubscribeObject<IDBSubscribeObject>();
                //try to subscribe.
                dbSubscribeObject.AddSubscribe(msg.DatabaseName, msg.TableName);
                //deal
                CSNGetDataTableResponseMessage csnGetDataTableResponseMessage = new CSNGetDataTableResponseMessage();
                csnGetDataTableResponseMessage.Header.SessionId = msg.Header.SessionId;
                csnGetDataTableResponseMessage.Header.ServiceKey = msg.Header.ServiceKey;
                csnGetDataTableResponseMessage.Header.ClientTag = msg.Header.ClientTag;

                #region Get data table request message [DB data]

                string[] tables = msg.TableName.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                csnGetDataTableResponseMessage.Tables = new DataTable[tables.Length];
                for (int i = 0; i < tables.Length; i++)
                {
                    string key = string.Intern(string.Format("__{0}__{1}", msg.DatabaseName, tables[i]));
                    try
                    {
                        //try to get a cache from cache manager.
                        DBDataCache dataCache = (DBDataCache)Global.DBCacheManager.GetCache(key);
                        if (dataCache == null)
                        {
                            //create new cache
                            dataCache = (DBDataCache)Global.DBCacheFactory.Create(msg.DatabaseName, tables[i]);
                            dataCache.Key = key;
                            Global.DBCacheManager.Add(dataCache);
                        }
                        csnGetDataTableResponseMessage.Tables[i] = dataCache.Item;
                    }
                    catch (System.Exception ex)
                    {
                        Logs.Logger.Log(ex);
                        csnGetDataTableResponseMessage.LastError = ex.Message;
                    }
                }

                #endregion

                #region configuration transmitter assembled.

                ITransmitterContext transmitterContext = new TransmitterContext();
                transmitterContext.PreviousSessionId = message.Header.SessionId;
                transmitterContext.ResponseMessage = message;
                transmitterContext.ConfigType = ConfigTypes.Table;
                transmitterContext.Subscriber = subscriber;
                IConfigTransmitter configTransmitter = new ConfigTransmitter(subscriber, transmitterContext);
                configTransmitter.Next();

                #endregion
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                responseMessage = new CSNGetDataTableResponseMessage { LastError = "Can not get data table config infomation, because there has some errors happened." };
                responseMessage.Header.ServiceKey = msg.Header.ServiceKey;
                return responseMessage;
            }
            return null;
        }

        #endregion
    }
}