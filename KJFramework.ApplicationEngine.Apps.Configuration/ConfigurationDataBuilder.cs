using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Data;

namespace KJFramework.ApplicationEngine.Apps.Configuration
{
    /// <summary>
    ///    内部配置信息数据工厂
    /// </summary>
    internal static class ConfigurationDataBuilder
    {
        #region Constructor.

        /// <summary>
        ///    内部配置信息数据工厂
        /// </summary>
        static ConfigurationDataBuilder()
        {
            _dtProcMapping = new Dictionary<string, string>();
            _dtProcMapping["ha_configinfo"] = "USP_GetConfigInfo";
            _dtProcMapping["ha_serviceinfo"] = "USP_GetServiceInfo";
            _dtProcMapping["ha_serviceroutetable"] = "USP_GetServiceRouteTable";
        }
        
        #endregion

        #region Members.

        private static readonly Dictionary<string, string> _dtProcMapping;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ConfigurationDataBuilder));

        #endregion

        #region Methods.

        /// <summary>
        ///    根据数据库、表名、服务名信息获取一整张基于KEY/VALUE形式的表信息
        /// </summary>
        /// <param name="dbName">数据库名</param>
        /// <param name="table">表名</param>
        /// <param name="servicename">服务名称</param>
        /// <returns></returns>
        public static ResourceBlock[] GetKeyValueConfigurations(string dbName, string table, string servicename)
        {
            DataTable dataTable = Global.Database.SpExecuteTable(_dtProcMapping[table.ToLower()], new[] { "ServiceName" }, new object[] { servicename });
            if (dataTable == null || dataTable.Rows.Count == 0) return null;
            ResourceBlock[] data = new ResourceBlock[dataTable.Rows.Count];
            for (int i = 0; i < data.Length; i++)
            {
                ResourceBlock block = new ResourceBlock();
                DataRow row = dataTable.Rows[i];
                block.SetAttribute(0x00, new StringValueStored(row["ConfigKey"].ToString()));
                block.SetAttribute(0x01, new StringValueStored(row["ConfigValue"].ToString()));
                block.SetAttribute(0x02, new DateTimeValueStored(Convert.ToDateTime(row["CreateTime"])));
                block.SetAttribute(0x03, new DateTimeValueStored(Convert.ToDateTime(row["LastOprTime"])));
                data[i] = block;
            }
            return data;
        }

        #endregion
    }
}