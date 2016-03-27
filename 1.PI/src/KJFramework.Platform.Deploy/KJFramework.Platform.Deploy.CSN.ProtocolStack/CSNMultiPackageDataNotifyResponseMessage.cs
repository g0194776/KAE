using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     多包数据通知回馈消息
    ///     <para>* 此包通常在要分包传送大数据之前的场景出现。</para>
    /// </summary>
    public class CSNMultiPackageDataNotifyResponseMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     多包数据通知回馈消息
        ///     <para>* 此包通常在要分包传送大数据之前的场景出现。</para>
        /// </summary>
        public CSNMultiPackageDataNotifyResponseMessage()
        {
            Header.ProtocolId = 7;
        }
            
        #endregion

        #region Members

        /// <summary>
        ///     获取或设置本次分包所涉及的事物序列号
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public int SerialNumber { get; set; }

        #endregion
    }
}