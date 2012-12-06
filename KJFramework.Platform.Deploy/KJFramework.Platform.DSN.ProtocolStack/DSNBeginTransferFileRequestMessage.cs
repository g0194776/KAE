using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     开始传送文件的请求消息
    /// </summary>
    public class DSNBeginTransferFileRequestMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     开始传送文件的请求消息
        /// </summary>
        public DSNBeginTransferFileRequestMessage()
        {
            Header.ProtocolId = 0;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置请求令牌
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     获取或设置总共分包数目
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public int TotalPacketCount { get; set; }
        /// <summary>
        ///     获取或设置服务名
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置服务别名
        /// </summary>
        [IntellectProperty(14, IsRequire = false)]
        public string Name { get; set; }
        /// <summary>
        ///     获取或设置服务版本
        /// </summary>
        [IntellectProperty(15, IsRequire = true)]
        public string Version { get; set; }
        /// <summary>
        ///     获取或设置服务描述
        /// </summary>
        [IntellectProperty(16, IsRequire = false)]
        public string Description { get; set; }

        #endregion
    }
}