using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Caches
{
    /// <summary>
    ///     数据缓存工厂元接口，提供了相关的基本操作
    /// </summary>
    public interface IDataCacheFactory<T>
    {
        /// <summary>
        ///     创建一个缓存对象
        /// </summary>
        /// <param name="args">创建缓存对象的条件</param>
        /// <returns>返回创建的缓存对象</returns>
        IDataCache<T> Create(params object[] args);

        IDataCache<KeyValueItem[]> Create(string database,string table,string servicename);
    }
}