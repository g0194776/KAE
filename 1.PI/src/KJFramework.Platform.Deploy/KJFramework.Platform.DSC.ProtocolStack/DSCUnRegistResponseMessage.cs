using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     注销回馈消息
    /// </summary>
    public class DSCUnRegistResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     注销回馈消息
        /// </summary>
        public DSCUnRegistResponseMessage()
        {
            Header.ProtocolId = 3;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置注销结果
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }

        #endregion
    }
}