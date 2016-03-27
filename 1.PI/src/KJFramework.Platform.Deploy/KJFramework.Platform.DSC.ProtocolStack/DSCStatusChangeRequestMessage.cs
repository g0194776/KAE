using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     状态变更请求消息
    /// </summary>
    public class DSCStatusChangeRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     状态变更请求消息
        /// </summary>
        public DSCStatusChangeRequestMessage()
        {
            Header.ProtocolId = 8;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置状态变更的类型
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public StatusChangeTypes StatusChangeType { get; set; }
        /// <summary>
        ///     获取或设置状态变更项
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public OwnServiceItem[] Items { get; set; }
        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}