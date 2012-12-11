using System;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.Metadata.Performances
{
    /// <summary>
    ///     动态服务性能报告元接口，提供了相关的基本操作
    /// </summary>
    public interface IDynamicServicePerformanceReport
    {
        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置服务版本
        /// </summary>
        string Version { get; set; }
        /// <summary>
        ///     获取或设置服务描述
        /// </summary>
        string Description { get; set; }
        /// <summary>
        ///     获取或设置服务别名
        /// </summary>
        string Name { get; set; }
        /// <summary>
        ///     获取或设置应用程序域个数
        /// </summary>
        int AppDomainCount { get; set; }
        /// <summary>
        ///     获取或设置组件个数
        /// </summary>
        int ComponentCount { get; set; }
        /// <summary>
        ///     获取或设置最后更新时间
        /// </summary>
        DateTime LastHeartBeatTime { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值表示了当前动态服务是否支持程序与的性能捕获
        /// </summary>
        bool SupportDomainPerformance { get; set; }
        /// <summary>
        ///     获取或设置最后错误信息
        /// </summary>
        String LastError { get; set; }
        /// <summary>
        ///     获取服务存活状态
        /// </summary>
        ServiceLiveStatus LiveStatus { get; set; }
        /// <summary>
        ///     获取或设置性能项
        /// </summary>
        ServicePerformanceItem[] PerformanceItems { get; set; }
        /// <summary>
        ///     获取或设置应用程序域性能项
        /// </summary>
        DomainPerformanceItem[] DomainItems { get; set; }
        /// <summary>
        ///     获取或设置组件的健康状态
        /// </summary>
        ComponentHealthItem[] ComponentItems { get; set; }
    }
}