namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     动态服务心跳请求消息
    /// </summary>
    public class DynamicServiceHeartBeatRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     动态服务心跳请求消息
        /// </summary>
        public DynamicServiceHeartBeatRequestMessage()
        {
            Header.ProtocolId = 2;
        }

        #endregion
    }
}