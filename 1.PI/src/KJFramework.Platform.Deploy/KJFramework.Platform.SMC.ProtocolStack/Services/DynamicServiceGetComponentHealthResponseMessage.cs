using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Performances;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     获取组件健康状态回馈消息
    /// </summary>
    public class DynamicServiceGetComponentHealthResponseMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     获取组件健康状态回馈消息
        /// </summary>
        public DynamicServiceGetComponentHealthResponseMessage()
        {
            Header.ProtocolId = 10;
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

        #endregion
    }
}