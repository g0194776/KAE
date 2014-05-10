using System;
using KJFramework.Data.Synchronization;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.Data.Synchronization.Factories;
using KJFramework.EventArgs;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     Զ������ͬ��������
    ///     <para>* ���ڼ���SNC֮���ͨ�Ž���</para>
    /// </summary>
    internal sealed class RemoteDataSyncProxy : IRemoteDataSyncProxy
    {
        #region Constructor

        /// <summary>
        ///     Զ������ͬ��������
        ///     <para>* ���ڼ���SNC֮���ͨ�Ž���</para>
        /// </summary>
        public RemoteDataSyncProxy()
        {
            _sncGlobalConfigIep = SystemWorker.Instance.ConfigurationProxy.GetField("Common", "SAPS_Address");
        }

        #endregion

        #region Members

        private readonly string _sncGlobalConfigIep;

        #endregion

        #region Implementation of IRemoteDataSyncProxy

        /// <summary>
        ///     ����һ�����ݶ����ߣ����Ҷ��ĵ�Զ��SAPS��
        /// </summary>
        /// <param name="catalog">SAPS�ϵķ���</param>
        /// <param name="callback">�ص�����</param>
        /// <param name="isAutoReconnect">���ӶϿ����������ʶ</param>
        /// <returns>���ض��ĺ��״̬</returns>
        public IRemoteDataSubscriber<string, string> Regist(string catalog, EventHandler<LightSingleArgEventArgs<DataRecvEventArgs<string, string>>> callback, bool isAutoReconnect = false)
        {
            return Regist(catalog, _sncGlobalConfigIep, callback, isAutoReconnect);
        }

        /// <summary>
        ///     ����һ�����ݶ����ߣ����Ҷ��ĵ�Զ��SAPS��
        /// </summary>
        /// <typeparam name="K">Key����</typeparam>
        /// <typeparam name="V">Value����</typeparam>
        /// <param name="catalog">SAPS�ϵķ���</param>
        /// <param name="iep">Զ�̵�ַ</param>
        /// <param name="callback">�ص�����</param>
        /// <param name="isAutoReconnect">���ӶϿ����������ʶ</param>
        /// <returns>���ض��ĺ��״̬</returns>
        public IRemoteDataSubscriber<K, V> Regist<K, V>(string catalog, string iep, EventHandler<LightSingleArgEventArgs<DataRecvEventArgs<K, V>>> callback, bool isAutoReconnect = false)
        {
            if (string.IsNullOrEmpty(catalog)) throw new ArgumentNullException("catalog");
            if (callback == null) throw new ArgumentNullException("callback");
            IRemoteDataSubscriber<K, V> subscriber = DataSubscriberFactory.Instance.Create<K, V>(catalog, new NetworkResource(iep), isAutoReconnect);
            if (subscriber == null) throw new System.Exception("#Cannot regist remote data subscriber to SAPS. #iep: " + iep);
            SubscriberState state = subscriber.Open();
            if (state != SubscriberState.Subscribed && !isAutoReconnect) throw new System.Exception("#Cannot regist remote data subscriber to SAPS. #iep: " + iep);
            subscriber.MessageRecv += callback;
            return subscriber;
        }

        #endregion
    }
}