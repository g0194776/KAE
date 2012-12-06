using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     获取组件健康状态请求消息
    /// </summary>
    public class ClientGetComponentHealthRequestMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     获取组件健康状态请求消息
        /// </summary>
        public ClientGetComponentHealthRequestMessage()
        {
            Header.ProtocolId = 9;
        }

        #endregion

        #region Members

        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置要获取组件健康状态的名称集合
        ///     <para>根据协议要求，如果此值为: *ALL*, 则汇报所有组件的健康状态</para>
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string[] Components { get; set; }
        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}