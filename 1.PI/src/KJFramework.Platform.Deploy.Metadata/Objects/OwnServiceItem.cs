using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     拥有服务项
    /// </summary>
    public class OwnServiceItem : IntellectObject
    {
        /// <summary>
        ///     获取或设置服务别名
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public string Name { get; set; }
        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置服务版本号
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string Version { get; set; }
        /// <summary>
        ///     获取或设置服务描述
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     获取或设置组件个数
        /// </summary>
        [IntellectProperty(4, IsRequire = true)]
        public int ComponentCount { get; set; }
        /// <summary>
        ///     获取或设置组件详情
        /// </summary>
        [IntellectProperty(5, IsRequire = false)]
        public OwnComponentItem[] Componnets { get; set; }
        /// <summary>
        ///     获取或设置最后更新时间
        /// </summary>
        [IntellectProperty(6, IsRequire = false)]
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        ///     获取或设置最后心跳时间
        /// </summary>
        [IntellectProperty(7, IsRequire = false)]
        public DateTime LastHeartbeatTime { get; set; }
        /// <summary>
        ///     获取或设置控制服务地址
        /// </summary>
        [IntellectProperty(8, IsRequire = false)]
        public string ControlServiceAddress { get; set; }
        /// <summary>
        ///     获取或设置服务存活状态
        /// </summary>
        [IntellectProperty(9, IsRequire = false)]
        public ServiceLiveStatus LiveStatus { get; set; }
        /// <summary>
        ///     获取或设置是否支持程序域的性能监控
        /// </summary>
        [IntellectProperty(10, IsRequire = false)]
        public bool SupportDomainPerformance { get; set; }
        /// <summary>
        ///     获取或设置外壳版本号
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public string ShellVersion { get; set; }
        /// <summary>
        ///     获取或设置服务中的性能
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public ServicePerformanceItem[] PerformanceItems { get; set; }
        /// <summary>
        ///     获取或设置服务应用程序域的性能
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public DomainPerformanceItem[] DomainPerformanceItems { get; set; }
    }
}