namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ��ȡ������ע��Ĳ���ڵ���Ϣ������Ϣ
    /// </summary>
    public class DSCGetDeployNodesRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ������ע��Ĳ���ڵ���Ϣ������Ϣ
        /// </summary>
        public DSCGetDeployNodesRequestMessage()
        {
            Header.ProtocolId = 19;
        }

        #endregion
    }
}