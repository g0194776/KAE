using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     获取配置表请求消息
    /// </summary>
    public class CSNGetDataTableRequestMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     获取配置表请求消息
        /// </summary>
        public CSNGetDataTableRequestMessage()
        {
            Header.ProtocolId = 2;
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

        #endregion
    }
}