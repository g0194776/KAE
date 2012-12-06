using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     获取本地服务详细信息回馈消息
    /// </summary>
    public class DSNGetLocalServiceInfomationResponseMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     获取本地服务详细信息回馈消息
        /// </summary>
        public DSNGetLocalServiceInfomationResponseMessage()
        {
            Header.ProtocolId = 16;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     获取或设置服务详细信息集合
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public ServiceInfoItem[] Services { get; set; }
        /// <summary>
        ///     获取或设置最后错误信息
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}