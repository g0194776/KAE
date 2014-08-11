using System;
using KJFramework.Data.Synchronization;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.Data.Synchronization.Factories;
using KJFramework.EventArgs;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程数据同步代理器
    /// </summary>
    internal sealed class RemoteDataSyncProxy : IRemoteDataSyncProxy
    {
        #region Constructor

        /// <summary>
        ///     远程数据同步代理器
        /// </summary>
        /// <param name="iep">远程Data Sync Publisher的访问地址</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public RemoteDataSyncProxy(string iep)
        {
            if (string.IsNullOrEmpty(iep)) throw new ArgumentNullException(iep);
            _iep = iep;
        }

        #endregion

        #region Members

        private readonly string _iep;

        #endregion

        #region Implementation of IRemoteDataSyncProxy

        /// <summary>
        ///     创建一个数据订阅者，并且订阅到远程Data Publisher上
        /// </summary>
        /// <param name="catalog">Data Publisher上的分类</param>
        /// <param name="callback">回调函数</param>
        /// <param name="isAutoReconnect">连接断开后的重连标识</param>
        public void Regist(string catalog, EventHandler<LightSingleArgEventArgs<DataRecvEventArgs<string, string>>> callback, bool isAutoReconnect = false)
        {
            Regist(catalog, _iep, callback, isAutoReconnect);
        }

        /// <summary>
        ///     创建一个数据订阅者，并且订阅到远程Data Publisher上
        /// </summary>
        /// <typeparam name="K">Key类型</typeparam>
        /// <typeparam name="V">Value类型</typeparam>
        /// <param name="catalog">Data Publisher上的分类</param>
        /// <param name="iep">远程地址</param>
        /// <param name="callback">回调函数</param>
        /// <param name="isAutoReconnect">连接断开后的重连标识</param>
        public void Regist<K, V>(string catalog, string iep, EventHandler<LightSingleArgEventArgs<DataRecvEventArgs<K, V>>> callback, bool isAutoReconnect = false)
        {
            if (string.IsNullOrEmpty(catalog)) throw new ArgumentNullException("catalog");
            if (callback == null) throw new ArgumentNullException("callback");
            IRemoteDataSubscriber<K, V> subscriber = DataSubscriberFactory.Instance.Create<K, V>(catalog, new NetworkResource(iep), isAutoReconnect);
            if (subscriber == null) throw new System.Exception("#Cannot regist remote data subscriber to SAPS. #iep: " + iep);
            SubscriberState state = subscriber.Open();
            if (state != SubscriberState.Subscribed && !isAutoReconnect) throw new System.Exception("#Cannot regist remote data subscriber to SAPS. #iep: " + iep);
            subscriber.MessageRecv += callback;
        }

        #endregion
    }
}