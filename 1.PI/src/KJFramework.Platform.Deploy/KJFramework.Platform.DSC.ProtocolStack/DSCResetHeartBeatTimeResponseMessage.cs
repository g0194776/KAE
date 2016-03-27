using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     重置心跳时间回馈消息
    /// </summary>
    public class DSCResetHeartBeatTimeResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     重置心跳时间回馈消息
        /// </summary>
        public DSCResetHeartBeatTimeResponseMessage()
        {
            Header.ProtocolId = 11;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置重置心跳时间的结果
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     获取或设置机器名称
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}