using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.Policies;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     数据发布者元接口
    /// </summary>
    /// <typeparam name="K">key类型</typeparam>
    /// <typeparam name="V">value类型</typeparam>
    public interface IDataPublisher<K, V>
    {
        /// <summary>
        ///     获取发布者所属于的类别
        /// </summary>
        string Catalog { get; }
        /// <summary>
        ///     获取发布者策略
        /// </summary>
        IPublisherPolicy Policy { get; }
        /// <summary>
        ///     获取当前发布者所使用的网络资源
        /// </summary>
        INetworkResource Resource { get; }
        /// <summary>
        ///     获取发布者当前的状态
        /// </summary>
        PublisherState State { get; }
        /// <summary>
        ///     获取订阅人数
        /// </summary>
        int SubscriberCount { get; }
        /// <summary>
        ///     绑定一个网络资源到当前发布者
        /// </summary>
        /// <param name="res">网络资源</param>
        /// <exception cref="System.Exception">无效的网络资源</exception>
        void Bind(INetworkResource res);
        /// <summary>
        ///     关闭发布者
        ///     <para>* 此操作将会关闭所有当前订阅者的通信信道</para>
        /// </summary>
        void Close();
        /// <summary>
        ///     开启发布者
        /// </summary>
        /// <returns>返回开启后的状态</returns>
        /// <exception cref="System.Exception">开启失败</exception>
        PublisherState Open();
        /// <summary>
        ///     向所有的订阅者发布数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        void Publish(K key, V value);
    }
}