using KJFramework.Messages.Attributes;
using KJFramework.Platform.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     心跳回馈消息
    /// </summary>
    public class DSCHeartBeatResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     心跳回馈消息
        /// </summary>
        public DSCHeartBeatResponseMessage()
        {
            Header.ProtocolId = 5;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置心跳结果
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }

        #endregion
    }
}