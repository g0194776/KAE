using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     更新通知消息
    /// </summary>
    public class ClientUpdateProcessingMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     更新通知消息
        /// </summary>
        public ClientUpdateProcessingMessage()
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
        /// <summary>
        ///     获取或设置组件名称
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     获取或设置更新进度
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string Content { get; set; }

        #endregion
    }
}