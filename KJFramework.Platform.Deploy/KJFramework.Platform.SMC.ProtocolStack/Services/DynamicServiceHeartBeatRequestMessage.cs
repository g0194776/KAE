namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��̬��������������Ϣ
    /// </summary>
    public class DynamicServiceHeartBeatRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��̬��������������Ϣ
        /// </summary>
        public DynamicServiceHeartBeatRequestMessage()
        {
            Header.ProtocolId = 2;
        }

        #endregion
    }
}