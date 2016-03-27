namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     获取所有已注册的部署节点信息请求消息
    /// </summary>
    public class ClientGetDeployNodesRequestMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     获取所有已注册的部署节点信息请求消息
        /// </summary>
        public ClientGetDeployNodesRequestMessage()
        {
            Header.ProtocolId = 14;
        }

        #endregion
    }
}