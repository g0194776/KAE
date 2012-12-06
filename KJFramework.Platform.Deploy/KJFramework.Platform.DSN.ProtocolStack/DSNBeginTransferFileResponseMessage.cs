using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     开始传送文件的回馈消息
    /// </summary>
    public class DSNBeginTransferFileResponseMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     开始传送文件的回馈消息
        /// </summary>
        public DSNBeginTransferFileResponseMessage()
        {
            Header.ProtocolId = 1;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置请求令牌
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值标示了当前部署节点是否同意开始传送文件
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsAccept { get; set; }
        /// <summary>
        ///     获取或设置拒绝传送文件的原因
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string Reason { get; set; }

        #endregion
    }
}