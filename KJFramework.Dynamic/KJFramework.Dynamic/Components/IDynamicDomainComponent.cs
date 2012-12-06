using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Visitors;
using KJFramework.Plugin;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     动态程序域组件元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDynamicDomainComponent : IPlugin
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
        ///     获取一个值，该值表示了当前是否开启了组件通讯隧道技术
        /// </summary>
        bool IsUseTunnel { get; }
        /// <summary>
        ///     获取此组件通讯隧道的地址
        ///     <para>* 仅当该组件的IsUseTunnel = true时才有意义</para>
        /// </summary>
        /// <exception cref="NotSupportedException">不支持该功能</exception>
        /// <returns>返回隧道地址</returns>
        string GetTunnelAddress();
        /// <summary>
        ///     获取组件访问器
        /// </summary>
        IComponentTunnelVisitor TunnelVisitor { get; }
        /// <summary>
        ///     获取或设置当前组件所宿主的服务
        /// </summary>
        IDynamicDomainService OwnService { get; set; }
        /// <summary>
        ///     设置所有可联系组件的隧道地址
        /// </summary>
        /// <param name="addresses">隧道地址</param>
        void SetTunnelAddresses(Dictionary<string, string> addresses);
        /// <summary>
        ///     获取指定组件的通讯隧道
        /// </summary>
        /// <param name="componentName">组件名称</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        /// <exception cref="System.Exception">无法找到当前组件的通讯隧道地址，或者创建隧道失败</exception>
        /// <returns>返回指定组件的通讯隧道</returns>
        T GetTunnel<T>(string componentName) where T : class;
        /// <summary>
        ///     开始执行
        /// </summary>
        void Start();
        /// <summary>
        ///     停止执行
        /// </summary>
        void Stop();
        /// <summary>
        ///     使用组件隧道技术
        ///     <para>* 调用此方法， 将会开启该组件的通讯隧道功能，使得此组件可以被其他组件访问</para>
        /// </summary>
        /// <param name="metadataExchange">
        ///     元数据开放标示
        ///     <para>* 默认为不开放元数据</para>
        /// </param>
        /// <exception cref="System.Exception">开启失败</exception>
        void UseTunnel<T>(bool metadataExchange = false);
    }
}