using System;
using KJFramework.Dynamic.Structs;
using KJFramework.Enums;
using KJFramework.EventArgs;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     动态程序域服务元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDynamicDomainService
    {
        /// <summary>
        ///     获取内部组件数量
        /// </summary>
        int ComponentCount { get; }
        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     获取唯一标示
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     获取工作目录
        /// </summary>
        String WorkRoot { get; }
        /// <summary>
        ///     获取服务描述信息
        /// </summary>
        ServiceDescriptionInfo Infomation { get; }
        /// <summary>
        ///     开始执行
        /// </summary>
        void Start();
        /// <summary>
        ///     停止执行
        /// </summary>
        void Stop();
        /// <summary>
        ///     更新服务
        /// </summary>
        /// <returns>返回更新的状态</returns>
        bool Update();
        /// <summary>
        ///     更新具有指定全名的组件
        /// </summary>
        /// <param name="fullname">组件全名</param>
        /// <returns>返回更新的状态</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        bool Update(string fullname);
        /// <summary>
        ///     检查健康状况
        /// </summary>
        /// <returns>返回健康状况</returns>
        HealthStatus CheckHealth();
        /// <summary>
        ///     根据组件名称获取一个程序域组件
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>返回获取到的程序域组件</returns>
        IDynamicDomainComponent GetObject(String name);
        /// <summary>
        ///     开始工作
        /// </summary>
        event EventHandler StartWork;
        /// <summary>
        ///     停止工作
        /// </summary>
        event EventHandler EndWork;
        /// <summary>
        ///     工作状态汇报事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<String>> WorkingProcess;
        /// <summary>
        ///     更新状态汇报事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<String>> Updating;
    }
}