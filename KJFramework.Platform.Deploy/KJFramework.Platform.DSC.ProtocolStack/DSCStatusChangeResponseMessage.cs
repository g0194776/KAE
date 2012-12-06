using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     状态变更的回馈消息
    /// </summary>
    public class DSCStatusChangeResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     状态变更的回馈消息
        /// </summary>
        public DSCStatusChangeResponseMessage()
        {
            Header.ProtocolId = 9;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置变更结果
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }

        #endregion
    }
}