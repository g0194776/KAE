using KJFramework.Data.Synchronization.Policies;

namespace KJFramework.Data.Synchronization.Factories
{
    /// <summary>
    ///     数据发布者工厂接口
    /// </summary>
    public interface IDataPublisherFactory
    {
        /// <summary>
        ///     创建一个数据发布者
        /// </summary>
        /// <typeparam name="K">key类型</typeparam>
        /// <typeparam name="V">value类型</typeparam>
        /// <param name="catalog">分组名称</param>
        /// <param name="res">网络资源</param>
        /// <returns>返回创建后的数据发布者</returns>
        /// <exception cref="System.NullReferenceException">参数错误</exception>
        IDataPublisher<K, V> Create<K, V>(string catalog, INetworkResource res);
        /// <summary>
        ///     创建一个数据发布者
        /// </summary>
        /// <typeparam name="K">key类型</typeparam>
        /// <typeparam name="V">value类型</typeparam>
        /// <param name="catalog">分组名称</param>
        /// <param name="policy">发布者策略</param>
        /// <param name="res">网络资源</param>
        /// <returns>返回创建后的数据发布者</returns>
        /// <exception cref="System.NullReferenceException">参数错误</exception>
        IDataPublisher<K, V> Create<K, V>(string catalog, IPublisherPolicy policy, INetworkResource res);
    }
}
