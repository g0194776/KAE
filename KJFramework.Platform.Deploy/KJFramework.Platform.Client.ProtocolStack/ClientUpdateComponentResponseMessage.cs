using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     更新组件回馈消息
    /// </summary>
    public class ClientUpdateComponentResponseMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     更新组件回馈消息
        /// </summary>
        public ClientUpdateComponentResponseMessage()
        {
            Header.ProtocolId = 8;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置服务名
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     获取或设置错误信息
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string ErrorTrace { get; set; }
        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置组件更新结果
        /// </summary>
        [IntellectProperty(14, IsRequire = false)]
        public ComponentUpdateResultItem[] Items { get; set; }
        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(15, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}