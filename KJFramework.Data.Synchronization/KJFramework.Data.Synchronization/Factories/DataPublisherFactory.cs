using System;
using System.Collections.Generic;
using KJFramework.Data.Synchronization.Policies;
using KJFramework.Tracing;

namespace KJFramework.Data.Synchronization.Factories
{
    /// <summary>
    ///     ���ݷ����߹���
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
        ///     ����һ�����ݷ�����
        /// </summary>
        /// <typeparam name="K">key����</typeparam>
        /// <typeparam name="V">value����</typeparam>
        /// <param name="catalog">��������</param>
        /// <param name="res">������Դ</param>
        /// <returns>���ش���������ݷ�����</returns>
        /// <exception cref="System.NullReferenceException">��������</exception>
        public IDataPublisher<K, V> Create<K, V>(string catalog, INetworkResource res)
        {
            return Create<K, V>(catalog, PublisherPolicy.Default, res);
        }

        /// <summary>
        ///     ����һ�����ݷ�����
        /// </summary>
        /// <typeparam name="K">key����</typeparam>
        /// <typeparam name="V">value����</typeparam>
        /// <param name="catalog">��������</param>
        /// <param name="policy">�����߲���</param>
        /// <param name="res">������Դ</param>
        /// <returns>���ش���������ݷ�����</returns>
        /// <exception cref="System.NullReferenceException">��������</exception>
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