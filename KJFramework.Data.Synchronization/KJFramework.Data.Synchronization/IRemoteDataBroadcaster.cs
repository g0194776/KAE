using System;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.Data.Synchronization.Policies;
using KJFramework.EventArgs;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     远程数据广播者接口
    /// </summary>
    /// <typeparam name="K">key类型</typeparam>
    /// <typeparam name="V">value类型</typeparam>
    public interface IRemoteDataBroadcaster<K, V> : ISubscriber
    {
        /// <summary>
        ///     获取当前广播者所订阅的类别
        /// </summary>
        string Catalog { get; }
        /// <summary>
        ///     获取远程发布者的策略信息
        /// </summary>
        IPublisherPolicy Policy { get; }
        /// <summary>
        ///     获取或设置订阅超时时间
        ///     <para>* 默认时间: 30s.</para>
        /// </summary>
        TimeSpan BroadCastTimeout { get; set; }
        /// <summary>
        ///     绑定一系列网络资源到当前的广播者
        /// </summary>
        /// <param name="res">网络资源集合</param>
        /// <exception cref="System.Exception">无效的网络资源</exception>
        void Bind(params INetworkResource[] res);
        /// <summary>
        ///     广播数据
        /// </summary>
        /// <param name="key">数据KEY</param>
        /// <param name="value">数据VALUE</param>
        void Broadcast(K key, V value);
        /// <summary>
        ///     关闭当前广播者
        ///     <para>* 此操作将会导致断开与远程发布者的通信信道</para>
        /// </summary>
        void Close();
        /// <summary>
        ///     获取中心节点集合
        /// </summary>
        /// <returns>返回中心节点集合</returns>
        INetworkResource[] GetCentralNodes();
        /// <summary>
        ///     开启订阅者
        /// </summary>
        /// <returns>返回开启后的状态</returns>
        SubscriberState Open();
    }
}