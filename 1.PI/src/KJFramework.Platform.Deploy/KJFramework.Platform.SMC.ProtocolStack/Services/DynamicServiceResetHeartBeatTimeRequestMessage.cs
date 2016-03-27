using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     动态服务重置心跳时间请求消息
    /// </summary>
    public class DynamicServiceResetHeartBeatTimeRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     动态服务重置心跳时间请求消息
        /// </summary>
        public DynamicServiceResetHeartBeatTimeRequestMessage()
        {
            Header.ProtocolId = 4;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置心跳间隔
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int Interval { get; set; }

        #endregion
    }
}