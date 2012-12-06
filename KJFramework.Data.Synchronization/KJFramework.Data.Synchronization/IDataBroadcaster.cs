using System;

namespace KJFramework.Data.Synchronization
{
    public interface IDataBroadcaster<K, V>
    {
        /// <summary>
        ///     设置广播的超时时间
        /// </summary>
        TimeSpan BroadcasterTimeout { get; set; }

        /// <summary>
        ///     分类信息
        /// </summary>
        string Catalog { get; set; }

        /// <summary>
        ///     广播的资源
        /// </summary>
        INetworkResource Resource { get; }

        /// <summary>
        ///     当前是否可以发送
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        ///     绑定资源
        /// </summary>
        /// <param name="resource">资源信息</param>
        void Bind(INetworkResource resource);

        /// <summary>
        ///     同步的广播方法
        /// </summary>
        /// <param name="key">广播信息的Key</param>
        /// <param name="value">广播信息的Value</param>
        /// <returns>广播是否成功</returns>
        bool Broadcast(K key, V value);

        /// <summary>
        ///     广播的异步方法
        /// </summary>
        /// <param name="key">广播信息的Key</param>
        /// <param name="value">广播信息的Value</param>
        /// <param name="callback">回调函数，返回广播是否成功</param>
        void BroadcastAsync(K key, V value, Action<bool> callback);

        /// <summary>
        ///     关闭当前广播者
        ///     <para>* 此操作将会导致断开与远程发布者的通信信道</para>
        /// </summary>
        void Close();
    }
}