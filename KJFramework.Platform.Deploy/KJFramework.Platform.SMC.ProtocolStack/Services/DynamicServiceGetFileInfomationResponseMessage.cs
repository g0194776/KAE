using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     获取文件详细信息回馈消息
    /// </summary>
    public class DynamicServiceGetFileInfomationResponseMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     获取文件详细信息回馈消息
        /// </summary>
        public DynamicServiceGetFileInfomationResponseMessage()
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
        ///     获取或设置要获取详细信息的列表
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public FileInfo[] Files { get; set; }
        
        #endregion
    }
}