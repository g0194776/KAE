using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     部署状态汇报信息
    /// </summary>
    public class DSNDeployStatusReportMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     部署状态汇报信息
        /// </summary>
        public DSNDeployStatusReportMessage()
        {
            Header.ProtocolId = 8;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置请求令牌
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     获取或设置当前状态信息
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string CurrentStatus { get; set; }

        #endregion
    }
}