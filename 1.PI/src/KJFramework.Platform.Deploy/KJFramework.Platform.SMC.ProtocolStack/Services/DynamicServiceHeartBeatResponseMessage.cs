using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Performances;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     动态服务心跳反馈消息
    /// </summary>
    public class DynamicServiceHeartBeatResponseMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     动态服务心跳反馈消息
        /// </summary>
        public DynamicServiceHeartBeatResponseMessage()
        {
            Header.ProtocolId = 3;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置心跳结果
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     获取或设置服务名
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值标示了当前动态服务是否支持应用程序域的性能指标捕获
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public bool SupportDomainPerformance { get; set; }
        /// <summary>
        ///     获取或设置服务中每个域的性能
        /// </summary>
        [IntellectProperty(14, IsRequire = false)]
        public DomainPerformanceItem[] DomainItems { get; set; }
        /// <summary>
        ///     获取或设置服务中的性能
        /// </summary>
        [IntellectProperty(15, IsRequire = false)]
        public ServicePerformanceItem[] PerformanceItems { get; set; }
        /// <summary>
        ///     获取或设置组件健康项
        /// </summary>
        [IntellectProperty(16, IsRequire = false)]
        public ComponentHealthItem[] ComponentItems { get; set; }
        /// <summary>
        ///     获取或设置组件个数
        /// </summary>
        [IntellectProperty(17, IsRequire = false)]
        public int ComponentCount { get; set; }

        #endregion
    }
}