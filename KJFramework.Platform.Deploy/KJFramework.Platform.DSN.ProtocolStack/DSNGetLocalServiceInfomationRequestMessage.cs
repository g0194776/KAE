namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ���ط�����ϸ��Ϣ������Ϣ
    /// </summary>
    public class DSNGetLocalServiceInfomationRequestMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ���ط�����ϸ��Ϣ������Ϣ
        /// </summary>
        public DSNGetLocalServiceInfomationRequestMessage()
        {
            Header.ProtocolId = 15;
        }

        #endregion
    }
}