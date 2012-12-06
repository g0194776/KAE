using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.Data.Synchronization.Messages;
using KJFramework.Data.Synchronization.Policies;
using KJFramework.Messages.Helpers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     ���ݷ�����
    /// </summary>
    /// <typeparam name="K">����key����</typeparam>
    /// <typeparam name="V">����value����</typeparam>
    internal class DataPublisher<K, V> : IDataPublisher<K, V>, ICloneable
    {
        #region Constructor

        /// <summary>
        ///     ���ݷ�����
        /// </summary>
        /// <param name="catalog">�����߷�������</param>
        /// <param name="resource">������Դ</param>
        public DataPublisher(string catalog, INetworkResource resource)
            : this(catalog, resource, PublisherPolicy.Default)
        {
        }

        /// <summary>
        ///     ���ݷ�����
        /// </summary>
        /// <param name="catalog">�����߷�������</param>
        /// <param name="resource">������Դ</param>
        /// <param name="policy">�����߲���</param>
        public DataPublisher(string catalog, INetworkResource resource, IPublisherPolicy policy)
        {
            if (string.IsNullOrEmpty(catalog)) throw new ArgumentNullException("catalog");
            if (resource == null) throw new ArgumentNullException("resource");
            if (policy == null) throw new ArgumentNullException("policy");
            if (resource.Mode != ResourceMode.Local) throw new ArgumentException("Cannot create publisher for data sync, illegal network resource mode!    #mode: " + resource.Mode);
            _catalog = catalog;
            _policy = policy;
            _kType = typeof(K);
            _vType = typeof(V);
            Bind(resource);
        }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DataPublisher<K, V>));
        private PublisherResourceStub _stub;

        /// <summary>
        ///     �ڲ���ȡ�����ߵ���Դ���
        /// </summary>
        public PublisherResourceStub ResourceStub
        {
            get { return _stub; }
        }

        #endregion

        #region Implementation of IDataPublisher<K,V>

        private readonly Type _kType;
        private readonly Type _vType;
        internal bool IsClonePublisher = false;
        protected readonly string _catalog;
        protected readonly IPublisherPolicy _policy;
        protected INetworkResource _resource;
        protected PublisherState _state = PublisherState.Unknown;
        protected Dictionary<string, ILocalDataSubscriber> _subscribers = new Dictionary<string, ILocalDataSubscriber>();
        private static readonly object _lockSubObj = new object();

        /// <summary>
        ///     ��ȡ�����������ڵ����
        /// </summary>
        public string Catalog
        {
            get { return _catalog; }
        }

        /// <summary>
        ///     ��ȡ�����߲���
        /// </summary>
        public IPublisherPolicy Policy
        {
            get { return _policy; }
        }

        /// <summary>
        ///     ��ȡ��ǰ��������ʹ�õ�������Դ
        /// </summary>
        public INetworkResource Resource
        {
            get { return _resource; }
        }

        /// <summary>
        ///     ��ȡ�����ߵ�ǰ��״̬
        /// </summary>
        public PublisherState State
        {
            get { return _state; }
        }

        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        public int SubscriberCount
        {
            get { return _subscribers.Count; }
        }

        /// <summary>
        ///     ��һ��������Դ����ǰ������
        /// </summary>
        /// <param name="res">������Դ</param>
        /// <exception cref="System.Exception">��Ч��������Դ</exception>
        public void Bind(INetworkResource res)
        {
            try
            {
                if (res == null) throw new ArgumentNullException("res");
                if (res.Mode != ResourceMode.Local) throw new ArgumentException("Cannot create publisher for data sync, illegal network resource mode!    #mode: " + res.Mode);
                _stub = SystemResourcePool.Instance.Regist(res);
                _stub.NewTransaction += NewTransaction;
                _resource = res;
                _state = PublisherState.Prepared;
            }
            catch (System.Exception ex)
            {
                _state = PublisherState.Exception;
                _tracing.Error(ex, null);
                throw;
            }
        }

        /// <summary>
        ///     �رշ�����
        ///     <para>* �˲�������ر����е�ǰ�����ߵ�ͨ���ŵ�</para>
        /// </summary>
        public void Close()
        {
            if (_state != PublisherState.Close)
            {
                _state = PublisherState.Close;
                _stub.NewTransaction -= NewTransaction;
                _stub.Discard();
                _stub = null;
                //clean subscribers when network resource used count == 0.
                lock (_lockSubObj)
                {
                    foreach (var pair in _subscribers)
                    {
                        pair.Value.Disconnected -= SubscriberDisconnected;
                        pair.Value.Close();
                    }
                    _subscribers.Clear();
                }
            }
        }

        /// <summary>
        ///     ����������
        /// </summary>
        /// <returns>���ؿ������״̬</returns>
        /// <exception cref="System.Exception">����ʧ��</exception>
        public PublisherState Open()
        {
            if (_state == PublisherState.Open) return _state;
            if (_state == PublisherState.Prepared) return _state = PublisherState.Open;
            throw new System.Exception("Cannot open a data sync publisher, because inner state has incorrect!  #state: " + _state);
        }

        /// <summary>
        ///     �����еĶ����߷�������
        /// </summary>
        /// <param name="key">�ؼ���</param>
        /// <param name="value">ֵ</param>
        public void Publish(K key, V value)
        {
            if (_subscribers.Count == 0) return;
            byte[] kData = DataHelper.ToBytes(_kType, key);
            byte[] vData = DataHelper.ToBytes(_vType, value);
            lock (_lockSubObj)
                foreach (ILocalDataSubscriber subscriber in _subscribers.Values) subscriber.Send(_catalog, kData, vData);
        }

        #endregion

        #region Implementation of ICloneable

        /// <summary>
        ///     ���Կ�¡һ��ӵ��ͬ��������Դ�����ݷ�����
        ///     <para>* ��¡���������ݷ����ߣ��������ͬ���Ĺ��ܡ�</para>
        /// </summary>
        /// <returns>���ؿ�¡������ݷ�����</returns>
        public object Clone()
        {
            DataPublisher<K, V> clonePublisher = (DataPublisher<K, V>) MemberwiseClone();
            clonePublisher.IsClonePublisher = true;
            clonePublisher.ResourceStub.AddUseRef();
            return clonePublisher;
        }

        #endregion

        #region Events

        void NewTransaction(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<NewTransactionEventArgs> e)
        {
            NewTransactionEventArgs args = e.Target;
            BaseMessage msg = args.Transaction.Request;

            #region Subscribe.

            if (msg.MessageIdentity.ProtocolId == 0 && msg.MessageIdentity.ServiceId == 0 && msg.MessageIdentity.DetailsId == 0)
            {
                SubscribeRequestMessage req = (SubscribeRequestMessage)msg;
                if (req.Catalog.ToLower() != _catalog.ToLower()) return;
                LocalDataSubscriber subscriber = new LocalDataSubscriber(_policy, args.Channel);
                subscriber.Disconnected += SubscriberDisconnected;
                lock (_lockSubObj) _subscribers.Add(subscriber.Id.ToString(), subscriber);
                e.Target.Transaction.SendResponse(new SubscribeResponseMessage
                                                      {
                                                          Policy = (PublisherPolicy)_policy,
                                                          Result = SubscribeResult.Allow,
                                                          Id = subscriber.Id
                                                      });
                return;
            }

            #endregion

            #region Unsubscribe.

            if (msg.MessageIdentity.ProtocolId == 1 && msg.MessageIdentity.ServiceId == 0 && msg.MessageIdentity.DetailsId == 0)
            {
                UnSubscribeRequestMessage req = (UnSubscribeRequestMessage)msg;
                if (req.Catelog.ToLower() != _catalog.ToLower()) return;
                lock (_lockSubObj)
                {
                    ILocalDataSubscriber subscriber;
                    if (!_subscribers.TryGetValue(req.Id.ToString(), out subscriber)) return;
                    subscriber.Disconnected -= SubscriberDisconnected;
                    subscriber.Close();
                    _subscribers.Remove(req.Id.ToString());
                }
                e.Target.Transaction.SendResponse(new UnSubscribeResponseMessage());
                return;
            }

            if (msg.MessageIdentity.ProtocolId == 3 && msg.MessageIdentity.ServiceId == 0 && msg.MessageIdentity.DetailsId == 0)
            {
                BroadcastRequestMessage broadcastRequest = (BroadcastRequestMessage)msg;
                if (broadcastRequest.Catalog.ToLower() != _catalog.ToLower()) return;
                //send rsp at first.
                e.Target.Transaction.SendResponse(new BroadcastResponseMessage());
                //broadcast it for each subscriber.
                lock (_lockSubObj)
                {
                    foreach (ILocalDataSubscriber subscriber in _subscribers.Values)
                    {
                        try { subscriber.Send(_catalog, broadcastRequest.Key, broadcastRequest.Value); }
                        catch (System.Exception ex) { _tracing.Error(ex, null); }
                    }
                }
            }

            #endregion
        }

        void SubscriberDisconnected(object sender, System.EventArgs e)
        {
            ILocalDataSubscriber subscriber = (ILocalDataSubscriber)sender;
            subscriber.Disconnected -= SubscriberDisconnected;
            lock (_lockSubObj) _subscribers.Remove(subscriber.Id.ToString());
        }

        #endregion
    }
}