namespace KJFramework.Data.Synchronization.Factories
{
    /// <summary>
    ///     数据发布者工厂接口
    /// </summary>
    public interface IDataSubscriberFactory
    {
        /// <summary>
        ///     创建一个数据发布者
        /// </summary>
        /// <typeparam name="K">key类型</typeparam>
        /// <typeparam name="V">value类型</typeparam>
        /// <param name="catalog">分组名称</param>
        /// <param name="res">网络资源</param>
        /// <param name="isAutoReconnect">是否自动启动重连的标识</param>
        /// <returns>返回创建后的数据发布者</returns>
        /// <exception cref="System.NullReferenceException">参数错误</exception>
        IRemoteDataSubscriber<K, V> Create<K, V>(string catalog, INetworkResource res, bool isAutoReconnect = false);
    }
}
