using System;
using System.Collections.Generic;
using System.Data;
using KJFramework.ApplicationEngine.Entities;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Datas;
using KJFramework.Net.Channels.Identities;
using KJFramework.Results;

namespace KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent.Helpers
{
    internal static class ApplicationHelper
    {
        #region Members.

        private static Database _database;

        #endregion

        #region Methods.

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="database">数据库对象</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public static void Initialize(Database database)
        {
            if (database == null) throw new ArgumentNullException("database");
            _database = database;
        }

        /// <summary>
        ///     根据一个CRC值异步获取指定应用的详细信息
        /// </summary>
        /// <param name="crc">应用的CRC值</param>
        public static IExecuteResult<ApplicationInformation> GetApplicationInformationByCRCAsync(long crc)
        {
            DataTable table = _database.SpExecuteTable("spGetMessageIdentitiesByAppCRC", new[] { "CRC" }, new object[] { crc });
            if (table == null || table.Rows.Count == 0) return ExecuteResult<ApplicationInformation>.Fail((byte)KAEErrorCodes.NullResultWithTargetedAppCRC, string.Empty);
            ApplicationInformation information = new ApplicationInformation();
            DataRow row = table.Rows[0];
            information.PackageName = Convert.IsDBNull(row["PackageName"]) ? string.Empty : row["PackageName"].ToString();
            information.Description = Convert.IsDBNull(row["Description"]) ? string.Empty:row["Description"].ToString();
            information.Version = Convert.IsDBNull(row["Version"]) ? string.Empty : row["Version"].ToString();
            information.Level = (ApplicationLevel) Enum.Parse(typeof (ApplicationLevel), row["Level"].ToString());
            information.MessageIdentities = new Dictionary<ProtocolTypes, IList<MessageIdentity>>();
            foreach (DataRow dataRow in table.Rows)
            {
                ProtocolTypes protocol = (ProtocolTypes) byte.Parse(dataRow["ProtocolTypeId"].ToString());
                MessageIdentity identity = new MessageIdentity
                {
                    ProtocolId = byte.Parse(dataRow["ProtocolId"].ToString()),
                    ServiceId = byte.Parse(dataRow["ServiceId"].ToString()),
                    DetailsId = byte.Parse(dataRow["DetailsId"].ToString())
                };
                IList<MessageIdentity> tempValue;
                if (!information.MessageIdentities.TryGetValue(protocol, out tempValue))
                    information.MessageIdentities.Add(protocol, (tempValue = new List<MessageIdentity>()));
                tempValue.Add(identity);
            }
            return ExecuteResult<ApplicationInformation>.Succeed(information);
        }

        #endregion
    }
}