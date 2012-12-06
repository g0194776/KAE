using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     结束传输数据请求消息
    /// </summary>
    public class CSNEndTransferDataRequestMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     结束传输数据请求消息
        /// </summary>
        public CSNEndTransferDataRequestMessage()
        {
            Header.ProtocolId = 11;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置本次分包所涉及的事物序列号
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int SerialNumber { get; set; }

        #endregion
    }
}