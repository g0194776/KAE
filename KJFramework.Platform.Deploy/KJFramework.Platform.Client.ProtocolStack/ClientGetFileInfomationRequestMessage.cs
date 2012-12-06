using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     获取文件详细信息请求消息
    /// </summary>
    public class ClientGetFileInfomationRequestMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     获取文件详细信息请求消息
        /// </summary>
        public ClientGetFileInfomationRequestMessage()
        {
            Header.ProtocolId = 12;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置要获取详细信息的文件列表
        ///     <para>* 协议规定，如果此值为：*ALL*则返回所有文件的详细信息</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string Files { get; set; }
        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string MachineName { get; set; }

        #endregion
    }
}