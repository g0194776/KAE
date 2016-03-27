using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     开始部署请求消息
    /// </summary>
    public class DSNBeginDeployMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     开始部署请求消息
        /// </summary>
        public DSNBeginDeployMessage()
        {
            Header.ProtocolId = 6;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置请求令牌
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值表示了是否需要汇报部署细节
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool ReportDetail { get; set; }

        #endregion
    }
}