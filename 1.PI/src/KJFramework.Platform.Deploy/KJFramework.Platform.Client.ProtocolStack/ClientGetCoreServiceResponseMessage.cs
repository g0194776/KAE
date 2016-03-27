using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     获取核心服务信息回馈消息
    /// </summary>
    public class ClientGetCoreServiceResponseMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     获取核心服务信息回馈消息
        /// </summary>
        public ClientGetCoreServiceResponseMessage()
        {
            Header.ProtocolId = 17;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置核心服务信息集合
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public CoreServiceItem[] Items { get; set; }

        #endregion
    }
}