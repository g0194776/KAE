using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     停止服务回馈消息
    /// </summary>
    public class DSNStopServiceResponseMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     停止服务回馈消息
        /// </summary>
        public DSNStopServiceResponseMessage()
        {
            Header.ProtocolId = 14;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值表示了当前是否开启服务成功
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsSuccess { get; set; }
        /// <summary>
        ///     获取或设置错误信息
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string LastError { get; set; }
        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(14, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}