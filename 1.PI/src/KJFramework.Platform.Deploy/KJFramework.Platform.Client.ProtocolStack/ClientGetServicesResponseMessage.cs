using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     客户端获取所有服务相关信息回馈消息
    /// </summary>
    public class ClientGetServicesResponseMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     客户端获取所有服务相关信息回馈消息
        /// </summary>
        public ClientGetServicesResponseMessage()
        {
            Header.ProtocolId = 3;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置服务详细信息
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public OwnServiceItem[] Items { get; set; }
        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}