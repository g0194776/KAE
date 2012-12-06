using System;
using System.Configuration;
using KJFramework.Basic.Enum;
using KJFramework.Datas;
using KJFramework.Dynamic.Components;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Plugin;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector
{
    /// <summary>
    ///     数据库数据匹配器组件
    /// </summary>
    public class DBMatcherComponent : DynamicDomainComponent
    {
        #region Constructor

        /// <summary>
        ///     CSN数据库数据匹配器组件，用于缓存和匹配数据库中的数据
        /// </summary>
        public DBMatcherComponent()
        {
            _name = "CSN数据库数据匹配器组件";
            _pluginInfo = new PluginInfomation();
            _pluginInfo.CatalogName = "Plugins";
            _pluginInfo.Version = "0.0.0.1";
            _pluginInfo.ServiceName = "CSN.Components.DBMatcherComponent";
            _pluginInfo.Description = "CSN数据库数据匹配器组件，用于缓存和匹配数据库中的数据";
            Global.DBComponentInstance = this;
        }

        #endregion

        #region Overrides of DynamicDomainComponent

        protected override void InnerStart()
        {
            //start to regist db
            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                try
                {
                    ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[i];
                    Database database = new Database(connectionStringSettings.ConnectionString);
                    database.Open();
                    Global.DBCacheFactory.RegistDatabase(connectionStringSettings.Name, database);
                    ConsoleHelper.PrintLine("#Database registed. #name: " + connectionStringSettings.Name, ConsoleColor.DarkGray);
                }
                catch (System.Exception ex)
                {
                    Logs.Logger.Log(ex);
                    ConsoleHelper.PrintLine(ex.Message, ConsoleColor.DarkRed);
                }
            }
        }

        protected override void InnerStop()
        {
            throw new NotImplementedException();
        }

        protected override void InnerOnLoading()
        {
            Console.WriteLine("Component : #DBMatcherComponent loading......!");
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return HealthStatus.Good;
        }

        #endregion
    }
}