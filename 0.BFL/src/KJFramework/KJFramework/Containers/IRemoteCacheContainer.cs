using KJFramework.Proxy;

namespace KJFramework.Containers
{
    /// <summary>
    ///     远程缓存容器元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="K">缓存对象Key类型</typeparam>
    /// <typeparam name="V">缓存对象类型</typeparam>
    public interface IRemoteCacheContainer<K, V> : ICacheContainer<K, V>
    {
        /// <summary>
        ///     获取远程缓存代理器
        /// </summary>
        IRemoteCacheProxy<K, V> Proxy { get; }
    }
}