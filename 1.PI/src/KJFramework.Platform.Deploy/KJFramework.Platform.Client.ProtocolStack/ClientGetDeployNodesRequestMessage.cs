namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     ��ȡ������ע��Ĳ���ڵ���Ϣ������Ϣ
    /// </summary>
    public class ClientGetDeployNodesRequestMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ������ע��Ĳ���ڵ���Ϣ������Ϣ
        /// </summary>
        public ClientGetDeployNodesRequestMessage()
        {
            Header.ProtocolId = 14;
        }

        #endregion
    }
}