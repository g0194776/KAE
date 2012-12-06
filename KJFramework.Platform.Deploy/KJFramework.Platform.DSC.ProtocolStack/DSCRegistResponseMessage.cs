using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     DSC注册回馈消息
    /// </summary>
    public class DSCRegistResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     DSC注册回馈消息
        /// </summary>
        public DSCRegistResponseMessage()
        {
            Header.ProtocolId = 1;
        }

        #endregion

        #region Members
        
        /// <summary>
        ///     获取或设置注册的结果
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }

        #endregion
    }
}