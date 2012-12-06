namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     获取所有已注册的部署节点信息请求消息
    /// </summary>
    public class DSCGetDeployNodesRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     获取所有已注册的部署节点信息请求消息
        /// </summary>
        public DSCGetDeployNodesRequestMessage()
        {
            Header.ProtocolId = 19;
        }

        #endregion
    }
}