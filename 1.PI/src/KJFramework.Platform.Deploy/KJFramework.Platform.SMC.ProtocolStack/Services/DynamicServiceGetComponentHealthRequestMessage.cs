using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     获取组件健康状态请求消息
    /// </summary>
    public class DynamicServiceGetComponentHealthRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     获取组件健康状态请求消息
        /// </summary>
        public DynamicServiceGetComponentHealthRequestMessage()
        {
            Header.ProtocolId = 9;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置要获取组件健康状态的名称集合
        ///     <para>根据协议要求，如果此值为: *ALL*, 则汇报所有组件的健康状态</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string[] Components { get; set; }

        #endregion
    }
}