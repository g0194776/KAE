using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.CSN.CP.DBMatcher.Caches;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.CP.DBMatcher.Contracts
{
    internal class ComponentTunnelService
    {
        #region Implementation of IComponentTunnelContract

        public void Transport(object obj)
        {
            CSNMessage message = (CSNMessage)obj;
            if (message is CSNGetDataTableRequestMessage)
            {
                CSNGetDataTableRequestMessage csnGetDataTableRequestMessage = (CSNGetDataTableRequestMessage) message;
                CSNGetDataTableResponseMessage csnGetDataTableResponseMessage = new CSNGetDataTableResponseMessage();
                csnGetDataTableResponseMessage.Header.SessionId = csnGetDataTableRequestMessage.Header.SessionId;
                csnGetDataTableResponseMessage.Header.ServiceKey = csnGetDataTableRequestMessage.Header.ServiceKey;
                csnGetDataTableResponseMessage.Header.ClientTag = csnGetDataTableRequestMessage.Header.ClientTag;

                #region Get data table request message [DB data]

                string[] tables = csnGetDataTableRequestMessage.TableName.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                csnGetDataTableResponseMessage.Tables = new DataTable[tables.Length];
                for (int i = 0; i < tables.Length; i++)
                {
                    string key = string.Intern(string.Format("__{0}__{1}", csnGetDataTableRequestMessage.DatabaseName, tables[i]));
                    try
                    {
                        //try to get a cache from cache manager.
                        DBDataCache dataCache = (DBDataCache)Global.DBCacheManager.GetCache(key);
                        if (dataCache == null)
                        {
                            //create new cache
                            dataCache = (DBDataCache)Global.DBCacheFactory.Create(csnGetDataTableRequestMessage.DatabaseName, tables[i]);
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
                //callback result.
                IComponentTunnelContract componentTunnelContract = GetContract("CSN.Components.ConnectorComponent");
                if (componentTunnelContract != null)
                {
                    //use tunnel to assembled data.
                    componentTunnelContract.Transport(csnGetDataTableResponseMessage);
                }

                #endregion
            }
        }

        #endregion

        #region Methods

        private IComponentTunnelContract GetContract(string name)
        {
            IComponentTunnelContract componentTunnelContract = Global.ComponentInstance.GetTunnel(name);
            if (componentTunnelContract == null)
            {
                string info = "Can not transport a object to component tunnel, beacuse this tunnel is null. #name: " + name + ".";
                Logs.Logger.Log(info);
                ConsoleHelper.PrintLine(info, ConsoleColor.DarkRed);
                return null;
            }
            return componentTunnelContract;
        }

        #endregion
    }
}