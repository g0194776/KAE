using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     分包传输数据的网络包
    /// </summary>
    public class CSNTransferDataMessage :  CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     分包传输数据的网络包
        /// </summary>
        public CSNTransferDataMessage()
        {
            Header.ProtocolId = 10;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置本次分包所涉及的事物序列号
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int SerialNumber { get; set; }
        /// <summary>
        ///     获取或设置本次数据包的编号
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public int PackageId { get; set; }
        /// <summary>
        ///     获取或设置本次传输的数据
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public byte[] Data { get; set; }

        #endregion
    }
}