using System;

namespace KJFramework.Data.Synchronization.Factories
{
    /// <summary>
    ///     Զ�����ݶ����߹���
    /// </summary>
    public class DataSubscriberFactory : IDataSubscriberFactory
    {
        #region Constructor

        /// <summary>
        ///     Զ�����ݶ����߹���
        /// </summary>
        private DataSubscriberFactory()
        {
            
        }

        #endregion

        #region Members

        /// <summary>
        ///     Զ�����ݶ����߹���
        /// </summary>
        public static readonly DataSubscriberFactory Instance = new DataSubscriberFactory();

        #endregion

        #region Implementation of IDataSubscriberFactory

        /// <summary>
        ///     ����һ�����ݷ�����
        /// </summary>
        /// <typeparam name="K">key����</typeparam>
        /// <typeparam name="V">value����</typeparam>
        /// <param name="catalog">��������</param>
        /// <param name="res">������Դ</param>
        /// <param name="isAutoReconnect">�Ƿ��Զ����������ı�ʶ</param>
        /// <returns>���ش���������ݷ�����</returns>
        /// <exception cref="System.NullReferenceException">��������</exception>
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