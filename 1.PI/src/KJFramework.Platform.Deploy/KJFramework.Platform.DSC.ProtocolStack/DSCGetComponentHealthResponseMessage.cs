using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Performances;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     获取组件健康状态回馈消息
    /// </summary>
    public class DSCGetComponentHealthResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     获取组件健康状态回馈消息
        /// </summary>
        public DSCGetComponentHealthResponseMessage()
        {
            Header.ProtocolId = 15;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置要获取组件健康状态的名称集合
        ///     <para>根据协议要求，如果此值为: *ALL*, 则汇报所有组件的健康状态</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public ComponentHealthItem[] Items { get; set; }
        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置最后错误信息
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