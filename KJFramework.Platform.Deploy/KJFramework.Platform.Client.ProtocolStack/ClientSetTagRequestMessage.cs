namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     �ͻ������ñ�ʶ������Ϣ
    /// </summary>
    public class ClientSetTagRequestMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     �ͻ������ñ�ʶ������Ϣ
        /// </summary>
        public ClientSetTagRequestMessage()
        {
            Header.ProtocolId = 0;
        }

        #endregion
    }
}