using System;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.Data.Synchronization.Policies;
using KJFramework.EventArgs;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     远程数据订阅者接口
    /// </summary>
    /// <typeparam name="K">key类型</typeparam>
    /// <typeparam name="V">value类型</typeparam>
    public interface IRemoteDataSubscriber<K, V> : ISubscriber
    {
        /// <summary>
        ///     获取或设置一个值，该值标示了当前的远程数据订阅者在断开数据连接后是否启用自动重连机制
        /// </summary>
        bool IsAutoReconnect { get; set; }
        /// <summary>
        ///     获取当前订阅者所订阅的类别
        /// </summary>
        string Catalog { get; }
        /// <summary>
        ///     获取远程发布者的策略信息
        /// </summary>
        IPublisherPolicy Policy { get; }
        /// <summary>
        ///     获取当前订阅者所使用的网络资源
        /// </summary>
        INetworkResource Resource { get; }
        /// <summary>
        ///     获取或设置订阅超时时间
        ///     <para>* 默认时间: 30s.</para>
        /// </summary>
        TimeSpan SubscribeTimeout { get; set; }
        /// <summary>
        ///     绑定一个网络资源到当前的订阅者
        /// </summary>
        /// <param name="res">网络资源</param>
        /// <exception cref="System.Exception">无效的网络资源</exception>
        void Bind(INetworkResource res);
        /// <summary>
        ///     关闭当前订阅者
        ///     <para>* 此操作将会导致断开与远程发布者的通信信道</para>
        /// </summary>
        void Close();
        /// <summary>
        ///     开启订阅者
        /// </summary>
        /// <returns>返回开启后的状态</returns>
        SubscriberState Open();
        /// <summary>
        ///     接收到数据同步的消息事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<DataRecvEventArgs<K, V>>> MessageRecv;
    }
}