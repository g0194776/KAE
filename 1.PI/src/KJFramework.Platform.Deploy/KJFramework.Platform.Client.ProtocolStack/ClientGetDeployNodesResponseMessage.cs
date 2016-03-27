using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     获取所有已注册的部署节点信息回馈消息
    /// </summary>
    public class ClientGetDeployNodesResponseMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     获取所有已注册的部署节点信息回馈消息
        /// </summary>
        public ClientGetDeployNodesResponseMessage()
        {
            Header.ProtocolId = 15;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置部署节点信息集合
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public OwnDeployNodeItem[] Items { get; set; }

        #endregion
    }
}