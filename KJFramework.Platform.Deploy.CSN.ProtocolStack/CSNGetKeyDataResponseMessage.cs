using KJFramework.Messages.Attributes;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     获取关键字数据配置回馈消息
    /// </summary>
    public class CSNGetKeyDataResponseMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     获取关键字数据配置回馈消息
        /// </summary>
        public CSNGetKeyDataResponseMessage()
        {
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 3, DetailsId = 1 };
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置查询后的数据集合
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public KeyValueItem[] Items { get; set; }
        /// <summary>
        ///     获取或设置最后的错误信息
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}