using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     多包数据通知请求消息
    ///     <para>* 此包通常在要分包传送大数据之前的场景出现。</para>
    /// </summary>
    public class CSNMultiPackageDataNotifyRequestMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     多包数据通知请求消息
        ///     <para>* 此包通常在要分包传送大数据之前的场景出现。</para>
        /// </summary>
        public CSNMultiPackageDataNotifyRequestMessage()
        {
            Header.ProtocolId = 6;
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

        #endregion
    }
}