using System;
using KJFramework.Enums;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     动态程序域组件元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDynamicDomainComponent
    {
        /// <summary>
        ///     获取名称
        /// </summary>
        String Name { get; }
        /// <summary>
        ///     检查当前组件的健康状况
        /// </summary>
        /// <returns>返回健康状况</returns>
        HealthStatus CheckHealth();
        /// <summary>
        ///     获取唯一标示
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///      获取或设置可用标示
        /// </summary>
        bool Enable { get; set; }
        /// <summary>
        ///     获取或设置插件信息
        /// </summary>
        PluginInfomation PluginInfo { get; }
        /// <summary>
        ///     获取或设置当前组件所宿主的服务
        /// </summary>
        IDynamicDomainService OwnService { get; set; }
        /// <summary>
        ///     开始执行
        /// </summary>
        void Start();
        /// <summary>
        ///     停止执行
        /// </summary>
        void Stop();
        /// <summary>
        ///     加载后需要做的动作
        /// </summary>
        void OnLoading();
    }
}