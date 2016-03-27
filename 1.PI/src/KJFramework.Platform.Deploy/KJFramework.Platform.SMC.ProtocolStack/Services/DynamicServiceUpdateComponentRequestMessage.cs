using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     动态服务更新组件请求消息
    /// </summary>
    public class DynamicServiceUpdateComponentRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     动态服务更新组件请求消息
        /// </summary>
        public DynamicServiceUpdateComponentRequestMessage()
        {
            Header.ProtocolId = 6;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置组件名称
        ///     <para>* 根据协议规定，如果此值为：*ALL*, 则表示所有的组件都要更新</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     获取或设置文件名称
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string FileName { get; set; }

        #endregion
    }
}