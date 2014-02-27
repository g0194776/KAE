using KJFramework.Messages.Attributes;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     获取关键字数据配置请求消息
    /// </summary>
    public class CSNGetKeyDataRequestMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     获取关键字数据配置请求消息
        /// </summary>
        public CSNGetKeyDataRequestMessage()
        {
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 3, DetailsId = 0 };
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置数据库名称
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string DatabaseName { get; set; }
        /// <summary>
        ///     获取或设置数据表名称
        ///     <para>* 此字段支持多表同时查询，字段值按照分号分隔。</para>
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string TableName { get; set; }
        /// <summary>
        ///     获取或设置要查询的关键数据集合
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string ServiceName { get; set; }

        //bool hasWatch;

        #endregion
    }
}