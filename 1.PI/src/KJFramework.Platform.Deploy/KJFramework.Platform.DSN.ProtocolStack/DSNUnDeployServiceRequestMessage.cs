using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     卸载部署请求消息
    /// </summary>
    public class DSNUnDeployServiceRequestMessage : DSNMessage
    {
        #region Constrcutor

        /// <summary>
        ///     卸载部署请求消息
        /// </summary>
        public DSNUnDeployServiceRequestMessage()
        {
            Header.ProtocolId = 9;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置卸载的原因
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string Reason { get; set; }
        /// <summary>
        ///     获取或设置是否需要卸载报告
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public bool IsDetailReport { get; set; }

        #endregion
    }
}