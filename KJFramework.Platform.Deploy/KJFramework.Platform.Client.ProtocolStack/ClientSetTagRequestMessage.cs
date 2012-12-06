namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     客户端设置标识请求消息
    /// </summary>
    public class ClientSetTagRequestMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     客户端设置标识请求消息
        /// </summary>
        public ClientSetTagRequestMessage()
        {
            Header.ProtocolId = 0;
        }

        #endregion
    }
}