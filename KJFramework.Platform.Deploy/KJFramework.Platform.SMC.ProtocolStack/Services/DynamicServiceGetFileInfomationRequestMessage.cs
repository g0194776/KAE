using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     获取文件详细信息请求消息
    /// </summary>
    public class DynamicServiceGetFileInfomationRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     获取文件详细信息请求消息
        /// </summary>
        public DynamicServiceGetFileInfomationRequestMessage()
        {
            Header.ProtocolId = 11;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置要获取详细信息的文件列表
        ///     <para>* 协议规定，如果此值为：*ALL*则返回所有文件的详细信息</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string Files { get; set; }

        #endregion
    }
}