using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     客户端设置标识回馈消息
    /// </summary>
    public class ClientSetTagResponseMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     客户端设置标识回馈消息
        /// </summary>
        public ClientSetTagResponseMessage()
        {
            Header.ProtocolId = 1;
        }

        #endregion

        #region Members
        
        /// <summary>
        ///     获取或设置结果
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }

        #endregion
    }
}