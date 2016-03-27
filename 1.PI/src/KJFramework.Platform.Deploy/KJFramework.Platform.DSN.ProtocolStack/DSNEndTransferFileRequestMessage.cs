using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     结束传输文件请求消息
    /// </summary>
    public class DSNEndTransferFileRequestMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     结束传输文件请求消息
        /// </summary>
        public DSNEndTransferFileRequestMessage()
        {
            Header.ProtocolId = 3;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置请求令牌
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }

        #endregion
    }
}