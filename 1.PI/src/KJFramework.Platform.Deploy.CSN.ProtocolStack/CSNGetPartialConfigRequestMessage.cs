using KJFramework.Messages.Attributes;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     获取部分配置信息请求消息
    /// </summary>
    public class CSNGetPartialConfigRequestMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     获取部分配置信息请求消息
        /// </summary>
        public CSNGetPartialConfigRequestMessage()
        {
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 4, DetailsId = 0 };
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置配置关键字名
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string Key { get; set; }

        #endregion
    }
}