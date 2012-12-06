using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.SMC.Common.Performances;
using KJFramework.Platform.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     心跳请求消息
    /// </summary>
    public class DSCHeartBeatRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     动态服务心跳请求消息
        /// </summary>
        public DSCHeartBeatRequestMessage()
        {
            Header.ProtocolId = 4;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     获取或设置服务中的性能
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public ServicePerformanceItem[] PerformanceItems { get; set; }

        #endregion
    }
}