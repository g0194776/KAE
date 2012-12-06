using System;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.Metadata.Performances;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Objects
{
    /// <summary>
    ///     动态服务记录项元接口
    /// </summary>
    public interface IDynamicServiceItem
    {
        /// <summary>
        ///     获取通道编号
        /// </summary>
        Guid ChannelId { get; } 
        /// <summary>
        ///     获取服务名称
        /// </summary>
        string ServiceName { get; }
        /// <summary>
        ///     获取服务版本号
        /// </summary>
        string Version { get; }
        /// <summary>
        ///     获取服务描述
        /// </summary>
        string Description { get; }
        /// <summary>
        ///     获取服务别名
        /// </summary>
        string Name { get; }
        /// <summary>
        ///     获取或设置外壳版本号
        /// </summary>
        string ShellVersion { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值表示了当前动态服务是否支持程序与的性能捕获
        /// </summary>
        bool SupportDomainPerformance { get; set; }
        /// <summary>
        ///     获取或设置服务存活状态
        /// </summary>
        ServiceLiveStatus LiveStatus { get; set; }
        /// <summary>
        ///     获取或设置服务的应用程序域个数
        /// </summary>
        int AppDomainCount { get; set; }
        /// <summary>
        ///     获取或设置服务的组件个数
        /// </summary>
        int ComponentCount { get; set; }
        /// <summary>
        ///     获取或设置最后心跳时间
        /// </summary>
        DateTime LastHeartBeatTime { get; set; }
        /// <summary>
        ///     获取或设置最后更新时间
        /// </summary>
        DateTime LastUpdateTime { get; set; }
        /// <summary>
        ///     获取或设置最后错误信息
        /// </summary>
        String LastError { get; set; }
        /// <summary>
        ///     获取或设置进程名称
        /// </summary>
        string ProcessName { get; set; }
        /// <summary>
        ///     获取或设置组件的详细信息
        /// </summary>
        ComponentDetailItem[] Components { get; set; }
        /// <summary>
        ///     更新性能项
        /// </summary>
        /// <param name="items">性能项</param>
        void Update(ServicePerformanceItem[] items);
        /// <summary>
        ///     更新性能项
        /// </summary>
        /// <param name="items">性能项</param>
        void Update(DomainPerformanceItem[] items);
        /// <summary>
        ///     更新性能项
        /// </summary>
        /// <param name="items">性能项</param>
        void Update(ComponentHealthItem[] items);
        /// <summary>
        ///     设置组件更新结果项
        /// </summary>
        /// <param name="items">更新结果项</param>
        void Update(ComponentUpdateResultItem[] items);
        /// <summary>
        ///     获取服务相关性能项
        /// </summary>
        /// <returns>返回服务相关性能项</returns>
        ServicePerformanceItem[] GetPerformances();
        /// <summary>
        ///     获取服务应用程序与相关性能项
        /// </summary>
        /// <returns>返回服务相关性能项</returns>
        DomainPerformanceItem[] GetDomainPerformances();
    }
}