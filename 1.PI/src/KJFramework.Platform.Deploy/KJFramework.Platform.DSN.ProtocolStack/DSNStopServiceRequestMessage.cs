using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     停止服务请求消息
    /// </summary>
    public class DSNStopServiceRequestMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     停止服务请求消息
        /// </summary>
        public DSNStopServiceRequestMessage()
        {
            Header.ProtocolId = 13;
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