using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     服务状态变更消息
    /// </summary>
    public class ClientStatusChangeMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     服务状态变更消息
        /// </summary>
        public ClientStatusChangeMessage()
        {
            Header.ProtocolId = 4;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置状态变更的类型
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public StatusChangeTypes StatusChangeType { get; set; }
        /// <summary>
        ///     获取或设置状态变更项
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public OwnServiceItem[] Items { get; set; }
        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}