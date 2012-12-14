namespace KJFramework.Data.Synchronization.Factories
{
    /// <summary>
    ///     远程数据广播者工厂
    /// </summary>
    public interface IDataBroadcasterFactory
    {
        /// <summary>
        ///     创建一个数据发布者
        /// </summary>
        /// <typeparam name="K">key类型</typeparam>
        /// <typeparam name="V">value类型</typeparam>
        /// <param name="catalog">分组名称</param>
        /// <param name="res">网络资源</param>
        /// <param name="isAutoReconnect">是否支持自动重连的标识</param>
        /// <returns>返回创建后的数据发布者</returns>
        /// <exception cref="System.NullReferenceException">参数错误</exception>
        IDataBroadcaster<K, V> Create<K, V>(string catalog, INetworkResource res, bool isAutoReconnect = false);
    }
}