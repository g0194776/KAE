using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.CSN.ProtocolStack.Enums;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     开始分包传输请求消息
    /// </summary>
    public class CSNBeginTransferDataRequestMessage :   CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     开始分包传输请求消息
        /// </summary>
        public CSNBeginTransferDataRequestMessage()
        {
            Header.ProtocolId = 8;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置上一次配置信息请求包的会话编号
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int PreviousSessionId { get; set; }
        /// <summary>
        ///     获取或设置本次分包所涉及的事物序列号
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public int SerialNumber { get; set; }
        /// <summary>
        ///     获取或设置总共的分包数目
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public int TotalPackageCount { get; set; }
        /// <summary>
        ///     获取或设置总共的数据大小
        /// </summary>
        [IntellectProperty(14, IsRequire = true)]
        public int TotalDataLength { get; set; }
        /// <summary>
        ///     获取或设置相关联的配置类型
        /// </summary>
        [IntellectProperty(15, IsRequire = true)]
        public ConfigTypes ConfigType { get; set; }

        #endregion
    }
}