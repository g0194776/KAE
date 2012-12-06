using System;
using KJFramework.ServiceModel.Elements;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.ServiceModel.Bussiness.Default.Services
{
    /// <summary>
    ///     服务句柄元接口，提供了相关的基本操作。
    /// </summary>
    internal interface IServiceHandle : IMetadataExchange
    {
        /// <summary>
        ///     获取绑定对象
        /// </summary>
        Binding[] Bindings { get; }
        /// <summary>
        ///     获取或设置地址URL
        /// </summary>
        Uri Uri { get; set; }
        /// <summary>
        ///     获取宿主服务
        /// </summary>
        HostService GetService();
        /// <summary>
        ///     获取一个值，该值表示了当前服务句柄的可用性
        /// </summary>
        bool Enable { get; }
        /// <summary>
        ///     开启服务
        /// </summary>
        void Open();
        /// <summary>
        ///     关闭服务
        /// </summary>
        void Close();
        /// <summary>
        ///     关闭服务，并关闭当前服务所开放的所有系统资源
        /// </summary>
        void Shutdown();
        /// <summary>
        ///     初始化
        /// </summary>
        void Initialize();
        /// <summary>
        ///     已经开启事件
        /// </summary>
        event EventHandler Opened;
        /// <summary>
        ///     关闭事件
        /// </summary>
        event EventHandler Closed;
    }
}