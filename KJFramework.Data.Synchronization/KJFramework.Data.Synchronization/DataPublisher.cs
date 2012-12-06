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
    ///     数据发布者
    /// </summary>
    /// <typeparam name="K">数据key类型</typeparam>
    /// <typeparam name="V">数据value类型</typeparam>
    internal class DataPublisher<K, V> : IDataPublisher<K, V>, ICloneable
    {
        #region Constructor

        /// <summary>
        ///     数据发布者
        /// </summary>
        /// <param name="catalog">发布者分组名称</param>
        /// <param name="resource">网络资源</param>
        public DataPublisher(string catalog, INetworkResource resource)
            : this(catalog, resource, PublisherPolicy.Default)
        {
        }

        /// <summary>
        ///     数据发布者
        /// </summary>
        /// <param name="catalog">发布者分组名称</param>
        /// <param name="resource">网络资源</param>
        /// <param name="policy">发布者策略</param>
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
        ///     内部获取发布者的资源存根
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
        ///     获取发布者所属于的类别
        /// </summary>
        public string Catalog
        {
            get { return _catalog; }
        }

        /// <summary>
        ///     获取发布者策略
        /// </summary>
        public IPublisherPolicy Policy
        {
            get { return _policy; }
        }

        /// <summary>
        ///     获取当前发布者所使用的网络资源
        /// </summary>
        public INetworkResource Resource
        {
            get { return _resource; }
        }

        /// <summary>
        ///     获取发布者当前的状态
        /// </summary>
        public PublisherState State
        {
            get { return _state; }
        }

        /// <summary>
        ///     获取订阅人数
        /// </summary>
        public int SubscriberCount
        {
            get { return _subscribers.Count; }
        }

        /// <summary>
        ///     绑定一个网络资源到当前发布者
        /// </summary>
        /// <param name="res">网络资源</param>
        /// <exception cref="System.Exception">无效的网络资源</exception>
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
        ///     关闭发布者
        ///     <para>* 此操作将会关闭所有当前订阅者的通信信道</para>
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
        ///     开启发布者
        /// </summary>
        /// <returns>返回开启后的状态</returns>
        /// <exception cref="System.Exception">开启失败</exception>
        public PublisherState Open()
        {
            if (_state == PublisherState.Open) return _state;
            if (_state == PublisherState.Prepared) return _state = PublisherState.Open;
            throw new System.Exception("Cannot open a data sync publisher, because inner state has incorrect!  #state: " + _state);
        }

        /// <summary>
        ///     向所有的订阅者发布数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
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
        ///     尝试克隆一个拥有同样网络资源的数据发布者
        ///     <para>* 克隆出来的数据发布者，将会具有同样的功能。</para>
        /// </summary>
        /// <returns>返回克隆后的数据发布者</returns>
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