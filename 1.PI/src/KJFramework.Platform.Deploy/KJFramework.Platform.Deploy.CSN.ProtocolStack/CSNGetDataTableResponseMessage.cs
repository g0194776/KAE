using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     获取配置表回馈消息
    /// </summary>
    public class CSNGetDataTableResponseMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     获取配置表回馈消息
        /// </summary>
        public CSNGetDataTableResponseMessage()
        {
            Header.ProtocolId = 3;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置数据库表集合
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public DataTable[] Tables { get; set; }
        /// <summary>
        ///     获取或设置最后错误信息
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}