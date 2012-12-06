using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     开始分包传输回馈消息
    /// </summary>
    public class CSNBeginTransferDataResponseMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     开始分包传输回馈消息
        /// </summary>
        public CSNBeginTransferDataResponseMessage()
        {
            Header.ProtocolId = 9;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置本次分包所涉及的事物序列号
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int SerialNumber { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值标示了对方是否接受此次分包传输的会话邀请
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsAccept { get; set; }

        #endregion
    }
}