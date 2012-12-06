using System;
using KJFramework.EventArgs;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     宿主传输通道元接口，提供了相关的基本操作。
    /// </summary>
    public interface IHostTransportChannel
    {
        /// <summary>
        ///     获取唯一标识
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     注册网络
        /// </summary>
        /// <returns>返回注册的状态</returns>
        bool Regist();
        /// <summary>
        ///     注销网络
        /// </summary>
        /// <returns>返回注册的状态</returns>
        bool UnRegist();
        /// <summary>
        ///     创建通道事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<ITransportChannel>> ChannelCreated;
        /// <summary>
        ///     通道断开事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<ITransportChannel>> ChannelDisconnected;
    }
}