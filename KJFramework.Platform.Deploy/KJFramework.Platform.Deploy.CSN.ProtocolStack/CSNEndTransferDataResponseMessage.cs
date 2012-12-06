using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     结束传输数据回馈包
    /// </summary>
    public class CSNEndTransferDataResponseMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     结束传输数据回馈包
        /// </summary>
        public CSNEndTransferDataResponseMessage()
        {
            Header.ProtocolId = 12;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置本次分包所涉及的事物序列号
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int SerialNumber { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值表示了当前传输是否已经完成
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsComplated { get; set; }
        /// <summary>
        ///     获取或设置当前丢包的编号集合
        ///     <para>* 此值仅当IsComplated = false时有效。</para>
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public int[] PackageIds { get; set; }

        #endregion
    }
}