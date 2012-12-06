using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     动态服务重置心跳时间反馈消息
    /// </summary>
    public class DynamicServiceResetHeartBeatTimeResponseMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     动态服务重置心跳时间反馈消息
        /// </summary>
        public DynamicServiceResetHeartBeatTimeResponseMessage()
        {
            Header.ProtocolId = 5;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置结果
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string ServiceName { get; set; }

        #endregion
    }
}