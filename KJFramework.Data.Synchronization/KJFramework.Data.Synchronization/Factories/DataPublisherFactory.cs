using System;
using System.Collections.Generic;
using KJFramework.Data.Synchronization.Policies;
using KJFramework.Tracing;

namespace KJFramework.Data.Synchronization.Factories
{
    /// <summary>
    ///     数据发布者工厂
    /// </summary>
    public class DataPublisherFactory : IDataPublisherFactory
    {
        #region Constructor

        private DataPublisherFactory()
        {
            
        }

        #endregion

        #region Members

        private readonly object _lockObj = new object();
        private static readonly IDictionary<string, object> _publishers = new Dictionary<string, object>();
        public static readonly DataPublisherFactory Instance = new DataPublisherFactory();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (DataPublisherFactory));

        #endregion

        #region Implementation of IDataPublisherFactory

        /// <summary>
        ///     创建一个数据发布者
        /// </summary>
        /// <typeparam name="K">key类型</typeparam>
        /// <typeparam name="V">value类型</typeparam>
        /// <param name="catalog">分组名称</param>
        /// <param name="res">网络资源</param>
        /// <returns>返回创建后的数据发布者</returns>
        /// <exception cref="System.NullReferenceException">参数错误</exception>
        public IDataPublisher<K, V> Create<K, V>(string catalog, INetworkResource res)
        {
            return Create<K, V>(catalog, PublisherPolicy.Default, res);
        }

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
        public IDataPublisher<K, V> Create<K, V>(string catalog, IPublisherPolicy policy, INetworkResource res)
        {
            SyncDataFramework.Initialize();
            if (string.IsNullOrEmpty(catalog)) throw new ArgumentNullException("catalog");
            if (res == null) throw new ArgumentNullException("res");
            if (policy == null) throw new ArgumentNullException("policy");
            IDataPublisher<K, V> publisher;
            System.Exception exception;
            lock (_lockObj)
            {
                object tempObj;
                if (_publishers.TryGetValue(catalog, out tempObj)) return (IDataPublisher<K, V>) tempObj;
                try
                {
                    publisher = new DataPublisher<K, V>(catalog, res, policy);
                    _publishers.Add(catalog, publisher);
                    return publisher;
                }
                catch (System.Exception ex)
                {
                    _tracing.Error(ex, null);
                    exception = ex;
                }
            }
            //avoid throw exception at lock block.
            throw exception;
        }

        #endregion
    }
}