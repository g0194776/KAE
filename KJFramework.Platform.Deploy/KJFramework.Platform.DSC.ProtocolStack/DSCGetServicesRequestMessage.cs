using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     获取服务详情请求消息
    /// </summary>
    public class DSCGetServicesRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     获取服务详情请求消息
        /// </summary>
        public DSCGetServicesRequestMessage()
        {
            Header.ProtocolId = 6;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置服务名称
        ///     <para>* 根据协议规定，如果此值为：*ALL*, 则表示返回所有服务的状态</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置机器名
        ///     <para>* 将通过机器名来过滤需要获取服务信息的SMC， 如果此值设置为: *ALL*, 则代表全部机器的所有被控服务都要获取详细信息</para>
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}