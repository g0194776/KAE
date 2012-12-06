using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     重置心跳间隔回馈消息
    /// </summary>
    public class ClientResetHeartBeatTimeResponseMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     重置心跳间隔回馈消息
        /// </summary>
        public ClientResetHeartBeatTimeResponseMessage()
        {
            Header.ProtocolId = 6;
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