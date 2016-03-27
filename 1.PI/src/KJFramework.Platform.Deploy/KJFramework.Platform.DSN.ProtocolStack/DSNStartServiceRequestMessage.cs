using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     开启服务请求消息
    /// </summary>
    public class DSNStartServiceRequestMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     开启服务请求消息
        /// </summary>
        public DSNStartServiceRequestMessage()
        {
            Header.ProtocolId = 11;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }

        #endregion
    }
}