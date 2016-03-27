using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     更新组件请求消息
    /// </summary>
    public class DSCUpdateComponentRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     更新组件请求消息
        /// </summary>
        public DSCUpdateComponentRequestMessage()
        {
            Header.ProtocolId = 12;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置组件名称
        ///     <para>* 根据协议规定，如果此值为：*ALL*, 则表示所有的组件都要更新</para>
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     获取或设置文件名称
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string FileName { get; set; }
        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(14, IsRequire = false)]
        public string MachineName { get; set; }

        #endregion
    }
}