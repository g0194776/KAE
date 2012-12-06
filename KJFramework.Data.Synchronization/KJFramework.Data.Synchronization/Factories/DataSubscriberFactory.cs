using System;

namespace KJFramework.Data.Synchronization.Factories
{
    /// <summary>
    ///     远程数据订阅者工厂
    /// </summary>
    public class DataSubscriberFactory : IDataSubscriberFactory
    {
        #region Constructor

        /// <summary>
        ///     远程数据订阅者工厂
        /// </summary>
        private DataSubscriberFactory()
        {
            
        }

        #endregion

        #region Members

        /// <summary>
        ///     远程数据订阅者工厂
        /// </summary>
        public static readonly DataSubscriberFactory Instance = new DataSubscriberFactory();

        #endregion

        #region Implementation of IDataSubscriberFactory

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
        public IRemoteDataSubscriber<K, V> Create<K, V>(string catalog, INetworkResource res, bool isAutoReconnect = false)
        {
            SyncDataFramework.Initialize();
            if (string.IsNullOrEmpty(catalog)) throw new ArgumentNullException(catalog);
            if (res == null) throw new ArgumentNullException("res");
            return new RemoteDataSubscriber<K, V>(catalog, res, isAutoReconnect);
        }

        #endregion
    }
}