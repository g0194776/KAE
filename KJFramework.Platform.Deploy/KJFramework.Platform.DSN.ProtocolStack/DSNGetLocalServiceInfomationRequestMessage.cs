namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     获取本地服务详细信息请求消息
    /// </summary>
    public class DSNGetLocalServiceInfomationRequestMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     获取本地服务详细信息请求消息
        /// </summary>
        public DSNGetLocalServiceInfomationRequestMessage()
        {
            Header.ProtocolId = 15;
        }

        #endregion
    }
}