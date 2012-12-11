using KJFramework.Messages.Attributes;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     获取配置表回馈消息
    /// </summary>
    public class CSNGetDataTableResponseMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     获取配置表回馈消息
        /// </summary>
        public CSNGetDataTableResponseMessage()
        {
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 2, DetailsId = 1 };
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