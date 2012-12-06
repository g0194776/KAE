using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     动态服务注册请求消息
    /// </summary>
    public class DynamicServiceRegistRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     动态服务注册请求消息
        /// </summary>
        public DynamicServiceRegistRequestMessage()
        {
            Header.ProtocolId = 0;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置服务名
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置服务版本
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string Version { get; set; }
        /// <summary>
        ///     获取或设置服务别名
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string Name { get; set; }
        /// <summary>
        ///     获取或设置服务描述
        /// </summary>
        [IntellectProperty(14, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值表示了当前动态服务是否支持程序与的性能捕获
        /// </summary>
        [IntellectProperty(15, IsRequire = true)]
        public bool SupportDomainPerformance { get; set; }
        /// <summary>
        ///     获取或设置进程名称
        /// </summary>
        [IntellectProperty(16, IsRequire = true)]
        public string ProcessName { get; set; }
        /// <summary>
        ///     获取或设置组件个数
        /// </summary>
        [IntellectProperty(17, IsRequire = true)]
        public int ComponentCount { get; set; }
        /// <summary>
        ///     获取或设置组件的健康状态
        /// </summary>
        [IntellectProperty(18, IsRequire = false)]
        public ComponentDetailItem[] Items { get; set; }
        /// <summary>
        ///     获取或设置当前被控端外壳版本号
        /// </summary>
        [IntellectProperty(19, IsRequire = false)]
        public string ShellVersion { get; set; }

        #endregion
    }
}