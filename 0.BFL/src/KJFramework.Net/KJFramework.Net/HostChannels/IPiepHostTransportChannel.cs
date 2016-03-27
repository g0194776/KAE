using System;
using KJFramework.Net.Uri;

namespace KJFramework.Net.HostChannels
{
    /// <summary>
    ///     命名管道通讯通道元接口，提供了相关的基本操作。
    /// </summary>
    internal interface IPiepHostTransportChannel : IHostTransportChannel
    {
        /// <summary>
        ///     获取实例个数
        /// </summary>
        int InstanceCount { get; }
        /// <summary>
        ///     获取命名管道的实例名称
        /// </summary>
        String Name { get; }
        /// <summary>
        ///     获取监听的管道地址
        /// </summary>
        PipeUri LogicalAddress { get; }
    }
}