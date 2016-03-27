using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     注销请求消息
    /// </summary>
    public class DSCUnRegistRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     注销请求消息
        /// </summary>
        public DSCUnRegistRequestMessage()
        {
            Header.ProtocolId = 2;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     获取或设置注销原因
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string Reason { get; set; }

        #endregion
    }
}