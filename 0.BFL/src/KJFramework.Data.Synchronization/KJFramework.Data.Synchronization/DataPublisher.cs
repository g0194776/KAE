using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.Data.Synchronization.Policies;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using KJFramework.Net;
using KJFramework.Net.Identities;

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
        protected IDictionary<Guid, ILocalDataSubscriber> _subscribers = new Dictionary<Guid, ILocalDataSubscriber>();
        protected IDictionary<Guid, IMessageTransportChannel<MetadataContainer>> _broadcasters = new Dictionary<Guid, IMessageTransportChannel<MetadataContainer>>();
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
                //disconnect all Broadcaster.
                lock (_broadcasters)
                {
                    foreach (var pair in _broadcasters)
                    {
                        pair.Value.Disconnected -= BroadcasterDisconnected;
                        pair.Value.Close();
                    }
                    _broadcasters.Clear();
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
            MetadataContainer msg = args.Transaction.Request;
            MessageIdentity messageIdentity = msg.GetAttribute(0x00).GetValue<MessageIdentity>();

            #region Subscribe.

            if (messageIdentity.ProtocolId == 0 && messageIdentity.ServiceId == 0 && messageIdentity.DetailsId == 0)
            {
                MetadataContainer req = msg;
                if (req.GetAttributeAsType<string>(0x0A).ToLower() != _catalog.ToLower()) return;
                LocalDataSubscriber subscriber = new LocalDataSubscriber(_policy, args.Channel);
                subscriber.Disconnected += SubscriberDisconnected;
                lock (_lockSubObj) _subscribers.Add(subscriber.Id, subscriber);
                e.Target.Transaction.SendResponse((MetadataContainer) new MetadataContainer()
                                        .SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity
                                        {
                                            ProtocolId = 0,
                                            ServiceId = 0,
                                            DetailsId = 1
                                        }))
                                        .SetAttribute(0x0C, new ResourceBlockStored(new ResourceBlock()
                                            .SetAttribute(0x00, new BooleanValueStored(_policy.CanRetry))
                                            .SetAttribute(0x01, new BooleanValueStored(_policy.IsOneway))
                                            .SetAttribute(0x02, new ByteValueStored(_policy.RetryCount))
                                            .SetAttribute(0x03, new Int32ValueStored(_policy.TimeoutSec))))
                                        .SetAttribute(0x0D, new ByteValueStored((byte) SubscribeResult.Allow))
                                        .SetAttribute(0x0E, new GuidValueStored(subscriber.Id)));
                return;
            }

            #endregion

            #region Unsubscribe.

            if (messageIdentity.ProtocolId == 1 && messageIdentity.ServiceId == 0 && messageIdentity.DetailsId == 0)
            {
                MetadataContainer req = msg;
                if (req.GetAttributeAsType<string>(0x0B).ToLower() != _catalog.ToLower()) return;
                lock (_lockSubObj)
                {
                    ILocalDataSubscriber subscriber;
                    if (!_subscribers.TryGetValue(req.GetAttributeAsType<Guid>(0x0C), out subscriber)) return;
                    subscriber.Disconnected -= SubscriberDisconnected;
                    subscriber.Close();
                    _subscribers.Remove(req.GetAttributeAsType<Guid>(0x0C));
                }
                e.Target.Transaction.SendResponse((MetadataContainer)new MetadataContainer()
                                        .SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity
                                        {
                                            ProtocolId = 1,
                                            ServiceId = 0,
                                            DetailsId = 1
                                        })));
                return;
            }

            #endregion

            #region Broadcast.

            if (messageIdentity.ProtocolId == 3 && messageIdentity.ServiceId == 0 && messageIdentity.DetailsId == 0)
            {
                MetadataContainer broadcastRequest = msg;
                if (broadcastRequest.GetAttributeAsType<string>(0x0A).ToLower() != _catalog.ToLower()) return;
                #region Add to broadcaster list.

                lock (_broadcasters)
                {
                    if(!_broadcasters.ContainsKey(e.Target.Channel.Key))
                    {
                        _broadcasters.Add(e.Target.Channel.Key, e.Target.Channel);
                        e.Target.Channel.Disconnected += BroadcasterDisconnected;
                    }
                }

                #endregion
                //send rsp at first.
                e.Target.Transaction.SendResponse((MetadataContainer)new MetadataContainer()
                                        .SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity
                                        {
                                            ProtocolId = 3,
                                            ServiceId = 0,
                                            DetailsId = 1
                                        })));
                //broadcast it for each subscriber.
                lock (_lockSubObj)
                {
                    List<Guid> errorSubscribers = new List<Guid>();
                    foreach (ILocalDataSubscriber subscriber in _subscribers.Values)
                    {
                        try { subscriber.Send(_catalog, broadcastRequest.GetAttributeAsType<byte[]>(0x0B), broadcastRequest.GetAttributeAsType<byte[]>(0x0C)); }
                        catch (System.Exception ex)
                        {
                            _tracing.Error(ex, null);
                            errorSubscribers.Add(subscriber.Id);
                        }
                    }
                    if (errorSubscribers.Count > 0)
                        foreach (Guid id in errorSubscribers) _subscribers.Remove(id);
                }
            }

            #endregion
        }

        //broadcaster disconnected.
        void BroadcasterDisconnected(object sender, System.EventArgs e)
        {
            IMessageTransportChannel<BaseMessage> msgChannel = (IMessageTransportChannel<BaseMessage>) sender;
            msgChannel.Disconnected -= BroadcasterDisconnected;
            lock (_broadcasters) _broadcasters.Remove(msgChannel.Key);
        }

        //subscriber disconnected.
        void SubscriberDisconnected(object sender, System.EventArgs e)
        {
            ILocalDataSubscriber subscriber = (ILocalDataSubscriber)sender;
            subscriber.Disconnected -= SubscriberDisconnected;
            lock (_lockSubObj) _subscribers.Remove(subscriber.Id);
        }

        #endregion
    }
}