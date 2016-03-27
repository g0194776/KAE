using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
        /// <summary>
        ///     卸载部署回馈消息
        /// </summary>
    public class DSNUnDeployServiceResponseMessage : DSNMessage
    {
        #region Constrcutor

        /// <summary>
        ///     卸载部署回馈消息
        /// </summary>
        public DSNUnDeployServiceResponseMessage()
        {
            Header.ProtocolId = 10;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值表示了当前是否已经完成卸载服务的任务
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsSuccess { get; set; }
        /// <summary>
        ///     获取或设置最后的错误信息
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}