using System;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;

namespace KJFramework.Net.Cloud.Urls
{
    /// <summary>
    ///   服务域地址元接口，提供了相关的基本属性结构。
    /// </summary>
    public interface IServerAreaUri : IDisposable
    {
        /// <summary>
        ///   创建一个宿主信道
        /// </summary>
        /// <returns>返回宿主通道</returns>
        IHostTransportChannel CreateHostChannel();
        /// <summary>
        ///   创建一个通讯信道
        /// </summary>
        /// <returns>返回通讯信道</returns>
        ITransportChannel CreateTransportChannel();
    }
}