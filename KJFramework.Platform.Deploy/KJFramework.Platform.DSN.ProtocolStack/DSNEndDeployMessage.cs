using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     结束部署请求消息
    /// </summary>
    public class DSNEndDeployMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///    结束部署请求消息
        /// </summary>
        public DSNEndDeployMessage()
        {
            Header.ProtocolId = 7;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置请求令牌
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值表示了是否已经部署完毕
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsDeployed { get; set; }
        /// <summary>
        ///     获取或设置最后的错误信息
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}