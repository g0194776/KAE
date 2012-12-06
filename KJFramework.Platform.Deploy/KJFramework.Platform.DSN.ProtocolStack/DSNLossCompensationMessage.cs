using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     丢包补偿消息
    /// </summary>
    public class DSNLossCompensationMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     丢包补偿消息
        /// </summary>
        public DSNLossCompensationMessage()
        {
            Header.ProtocolId = 5;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置请求令牌
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     获取或设置当前数据包编号
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public int CurrentPackageId { get; set; }
        /// <summary>
        ///     获取或设置当前数据包数据
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public byte[] Data { get; set; }

        #endregion
    }
}