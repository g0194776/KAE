using System;

namespace KJFramework.Data.Synchronization.Factories
{
    public class DataBroadcasterFactory : IDataBroadcasterFactory
    {
        private DataBroadcasterFactory()
        {
        }

        private static IDataBroadcasterFactory _instance = new DataBroadcasterFactory();

        public static IDataBroadcasterFactory Instance
        {
            get { return _instance; }
        }

        #region Implementation of IDataBroadcasterFactory

        /// <summary>
        ///     创建一个数据发布者
        /// </summary>
        /// <typeparam name="K">key类型</typeparam>
        /// <typeparam name="V">value类型</typeparam>
        /// <param name="catalog">分组名称</param>
        /// <param name="res">网络资源</param>
        /// <returns>返回创建后的数据发布者</returns>
        /// <exception cref="System.NullReferenceException">参数错误</exception>
        public IDataBroadcaster<K, V> Create<K, V>(string catalog, INetworkResource res)
        {
            SyncDataFramework.Initialize();
            if (string.IsNullOrEmpty(catalog)) throw new ArgumentNullException(catalog);
            if (res == null) throw new ArgumentNullException("res");
            return new DataBroadcaster<K, V>(catalog, res);
        }

        #endregion
    }
}