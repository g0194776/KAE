using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     获取服务详情回馈消息
    /// </summary>
    public class DSCGetServicesResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     获取服务详情回馈消息
        /// </summary>
        public DSCGetServicesResponseMessage()
        {
            Header.ProtocolId = 7;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置服务详细信息
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public OwnServiceItem[] Items { get; set; }
        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}