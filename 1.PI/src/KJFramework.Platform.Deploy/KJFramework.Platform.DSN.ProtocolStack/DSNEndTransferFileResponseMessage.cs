using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     结束传输文件回馈消息
    /// </summary>
    public class DSNEndTransferFileResponseMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     结束传输文件回馈消息
        /// </summary>
        public DSNEndTransferFileResponseMessage()
        {
            Header.ProtocolId = 4;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置请求令牌
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值表示了当前是否已经传递成功
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsDone { get; set; }
        /// <summary>
        ///     获取或设置当前数据包编号
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public int[] LosePackets { get; set; }

        #endregion
    }
}