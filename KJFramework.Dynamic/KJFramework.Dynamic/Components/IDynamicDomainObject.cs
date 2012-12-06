using System;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Exceptions;
using KJFramework.Dynamic.Structs;
using KJFramework.Plugin;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     动态程序域对象，提供了相关的基本操作。
    /// </summary>
    internal interface IDynamicDomainObject : IDisposable
    {
        /// <summary>
        ///     获取内部动态程序域组件
        /// </summary>
        IDynamicDomainComponent Component { get; }
        /// <summary>
        ///     检查当前组件的健康状况
        /// </summary>
        /// <returns>返回健康状况</returns>
        HealthStatus CheckHealth();
        /// <summary>
        ///     获取创建时间
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     获取或设置上次更新时间
        /// </summary>
        DateTime LastUpdateTime { get; set; }
        /// <summary>
        ///     获取应用程序域组建入口信息
        /// </summary>
        DomainComponentEntryInfo EntryInfo { get; }
        /// <summary>
        ///     获取插件的详细信息
        /// </summary>
        PluginInfomation Infomation { get; }
        /// <summary>
        ///     获取内部应用程序域
        /// </summary>
        /// <returns>返回应用程序域</returns>
        AppDomain GetDomain();
        /// <summary>
        ///     获取唯一标示
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     开始执行
        /// </summary>
        void Start();
        /// <summary>
        ///     停止执行
        /// </summary>
        void Stop();
        /// <summary>
        ///     更新当前动态程序域
        /// </summary>
        /// <exception cref="DynamicDomainObjectUpdateFailedException">更新失败</exception>
        void Update();
        /// <summary>
        ///     重新续订组件的生命周期
        /// </summary>
        /// <param name="time">过期时间</param>
        void ReLease(TimeSpan time);
        /// <summary>
        ///     动态程序域对象退出事件
        /// </summary>
        event EventHandler Exited;
    }
}