//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading;
//using KJFramework.Data.Synchronization.Enums;
//using KJFramework.Data.Synchronization.EventArgs;
//using KJFramework.Data.Synchronization.Messages;
//using KJFramework.Data.Synchronization.Policies;
//using KJFramework.Data.Synchronization.Transactions;
//using KJFramework.EventArgs;
//using KJFramework.Messages.Helpers;
//using KJFramework.Net.Channels;
//using KJFramework.Tracing;

//namespace KJFramework.Data.Synchronization
//{
//    /// <summary>
//    ///     Զ�����ݹ㲥��
//    /// </summary>
//    /// <typeparam name="K">����KEY����</typeparam>
//    /// <typeparam name="V">����VALUE����</typeparam>
//    internal class RemoteDataBroadcaster<K, V> : IRemoteDataBroadcaster<K, V>
//    {
//        #region Constructor

//        /// <summary>
//        ///     Զ�����ݹ㲥��
//        /// </summary>
//        /// <param name="catelog">��������</param>
//        /// <param name="res">������Դ����</param>
//        public RemoteDataBroadcaster(string catelog, params INetworkResource[] res)
//        {
//            if (string.IsNullOrEmpty(catelog)) throw new ArgumentNullException("catelog");
//            if (res == null) throw new ArgumentNullException("res");
//            if (res.Length == 0) throw new ArgumentException("#Cannot create relations with 0 element remote resource!");
//            SubscribeTimeout = new TimeSpan(0, 0, 30);
//            _resource = res;
//            _catalog = catelog;
//            Bind(res);
//        }

//        #endregion

//        #region Members

//        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (RemoteDataBroadcaster<K, V>));
//        private readonly INetworkResource[] _resource;
//        private IList<IMessageTransportChannel<BaseMessage>> _channels;
//        //20 bytes = Guid(16) + Increment id(4)
//        private byte[] _binaryIdentity;
//        private readonly object _lockIdentity = new object();
//        private int _packageSerialId;

//        #endregion

//        #region Implementation of ISubscriber

//        private Guid _id;
//        private DateTime _startTime;
//        private SubscriberState _state;
//        private string _catalog;
//        private int _connectedNodesCount;
//        private IPublisherPolicy _policy;
//        private TimeSpan _subscribeTimeout;
//        private int _subscribedNodesCount;

//        /// <summary>
//        ///     ��ȡ�����ߵ�Ψһ��ʾ
//        /// </summary>
//        public Guid Id
//        {
//            get { return _id; }
//        }

//        /// <summary>
//        ///     ��ȡ�����ߵĶ���ʱ��
//        /// </summary>
//        public DateTime StartTime
//        {
//            get { return _startTime; }
//        }

//        /// <summary>
//        ///     ��ȡ�����ߵ�ǰ��״̬
//        /// </summary>
//        public SubscriberState State
//        {
//            get { return _state; }
//        }

//        public event EventHandler Disconnected;

//        #endregion

//        #region Implementation of IRemoteDataBroadcaster<K,V>

//        /// <summary>
//        ///     ��ȡ��ǰ�㲥�������ĵ����
//        /// </summary>
//        public string Catalog
//        {
//            get { return _catalog; }
//        }

//        /// <summary>
//        ///     ��ȡ�Ѿ������ϵ����Ľڵ���
//        /// </summary>
//        public int ConnectedNodesCount
//        {
//            get { return _connectedNodesCount; }
//        }

//        /// <summary>
//        ///     ��ȡԶ�̷����ߵĲ�����Ϣ
//        /// </summary>
//        public IPublisherPolicy Policy
//        {
//            get { return _policy; }
//        }

//        /// <summary>
//        ///     ��ȡ�����ö��ĳ�ʱʱ��
//        ///     <para>* Ĭ��ʱ��: 30s.</para>
//        /// </summary>
//        public TimeSpan SubscribeTimeout
//        {
//            get { return _subscribeTimeout; }
//            set { _subscribeTimeout = value; }
//        }

//        /// <summary>
//        ///     ��ȡ�Ѿ����ĵ����Ľڵ���
//        /// </summary>
//        public int SubscribedNodesCount
//        {
//            get { return _subscribedNodesCount; }
//        }

//        /// <summary>
//        ///     ��һϵ��������Դ����ǰ�Ĺ㲥��
//        /// </summary>
//        /// <param name="res">������Դ����</param>
//        /// <exception cref="System.Exception">��Ч��������Դ</exception>
//        public void Bind(params INetworkResource[] res)
//        {
//            if (res == null) throw new ArgumentNullException("res");
//            if (res.Length == 0) throw new ArgumentException("#Cannot create relations with 0 element remote resource!");
//            var result = res.Where(r => r.Mode != ResourceMode.Remote);
//            if (result != null && result.Any()) throw new ArgumentException("Cannot create remote data subscriber by LOCAL resource mode!!!");
//            if (_state == SubscriberState.Subscribed) throw new System.Exception("You cannot bind remote resources twice!!!");
//            _channels = new List<IMessageTransportChannel<BaseMessage>>();
//            for (int i = 0; i < res.Length; i++)
//            {
//                IPEndPoint iep = res[i].GetResource<IPEndPoint>();
//                TcpTransportChannel channel = new TcpTransportChannel(iep);
//                channel.Connect();
//                if (!channel.IsConnected)
//                {
//                    _tracing.Error("#Remote central node cannot be connect! #addr: {0}", iep);
//                    continue;
//                }
//                IMessageTransportChannel<BaseMessage> msgChannel = new MessageTransportChannel<BaseMessage>(channel, Global.ProtocolStack);
//                msgChannel.Disconnected += ChannelDisconnected;
//                msgChannel.ReceivedMessage += ChannelReceivedMessage;
//                _channels.Add(msgChannel);
//                Interlocked.Increment(ref _connectedNodesCount);
//            }
//            //last check. anyone connections cannot be made.
//            if(_connectedNodesCount == 0)  _state = SubscriberState.Disconnected;
//            else _state = SubscriberState.ToBeSubscribe;
//        }

//        /// <summary>
//        ///     �㲥����
//        /// </summary>
//        /// <param name="key">����KEY</param>
//        /// <param name="value">����VALUE</param>
//        public void Broadcast(K key, V value)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        ///     �رյ�ǰ�㲥��
//        ///     <para>* �˲������ᵼ�¶Ͽ���Զ�̷����ߵ�ͨ���ŵ�</para>
//        /// </summary>
//        public void Close()
//        {
//            if (_state != SubscriberState.Disconnected)
//            {
//                _state = SubscriberState.Disconnected;
//                foreach (IMessageTransportChannel<BaseMessage> channel in _channels)
//                {
//                    channel.Disconnected -= ChannelDisconnected;
//                    channel.ReceivedMessage -= ChannelReceivedMessage;
//                    channel.Close();
//                }
//                _channels = null;
//            }
//        }

//        /// <summary>
//        ///     ��ȡ���Ľڵ㼯��
//        /// </summary>
//        /// <returns>�������Ľڵ㼯��</returns>
//        public INetworkResource[] GetCentralNodes()
//        {
//            return _resource;
//        }

//        /// <summary>
//        ///     ����������
//        /// </summary>
//        /// <returns>���ؿ������״̬</returns>
//        public SubscriberState Open()
//        {
//            if (_connectedNodesCount == 0) return SubscriberState.Disconnected;
//            if (_state != SubscriberState.ToBeSubscribe) throw new System.Exception("You *MUST* call Bind method at first time!");
//            if (_channels == null) throw new System.Exception("Inner failed!  #reason: inner message channels is null.");
//            InitializeRelation();
//            return _state;
//        }

//        /// <summary>
//        ///     ���յ�����ͬ������Ϣ�¼�
//        /// </summary>
//        public event EventHandler<LightSingleArgEventArgs<DataRecvEventArgs<K, V>>> MessageRecv;
//        protected void MessageRecvHandler(LightSingleArgEventArgs<DataRecvEventArgs<K, V>> e)
//        {
//            EventHandler<LightSingleArgEventArgs<DataRecvEventArgs<K, V>>> handler = MessageRecv;
//            if (handler != null) handler(this, e);
//        }

//        /// <summary>
//        ///     ����ʧ���¼�
//        /// </summary>
//        public event EventHandler<LightSingleArgEventArgs<LightSingleArgEventArgs<INetworkResource>>> RelationFail;
//        protected void RelationFailHandler(LightSingleArgEventArgs<LightSingleArgEventArgs<INetworkResource>> e)
//        {
//            EventHandler<LightSingleArgEventArgs<LightSingleArgEventArgs<INetworkResource>>> handler = RelationFail;
//            if (handler != null) handler(this, e);
//        }

//        #endregion

//        #region Methods

//        /// <summary>
//        ///     ��ʼ�����Ĺ�ϵ
//        /// </summary>
//        private void InitializeRelation()
//        {
//            System.Exception exception = null;
//            int index = 0;
//            AutoResetEvent[] events = new AutoResetEvent[_channels.Count];
//            foreach (IMessageTransportChannel<BaseMessage> channel in _channels)
//            {
//                AutoResetEvent autoResetEvent = new AutoResetEvent(false);
//                #region Create transaction.

//                ISyncDataTransaction transaction = SyncDataTransactionManager.Instance.Create(TransactionIdentity.New(channel.LocalEndPoint, true), channel);
//                transaction.Failed += delegate
//                {
//                    _tracing.Error("Remote resource cannot subscribed. #reason: network failed! #iep: " + channel.RemoteEndPoint);
//                    autoResetEvent.Set();
//                };
//                transaction.Timeout += delegate
//                {
//                    _tracing.Error("Remote resource cannot subscribed. #reason: get rsp msessage timeout! #iep: " + channel.RemoteEndPoint);
//                    autoResetEvent.Set();
//                };
//                transaction.ResponseRecv += delegate(object sender, LightSingleArgEventArgs<BaseMessage> e)
//                {
//                    SubscribeResponseMessage rsp = (SubscribeResponseMessage)e.Target;
//                    if (rsp.Result == SubscribeResult.Reject)
//                    {
//                        _tracing.Error("Remote resource cannot subscribed. #reason: " + rsp.Result + " #iep: " + channel.RemoteEndPoint);
//                        autoResetEvent.Set();
//                    }
//                    else
//                    {
//                        _state = SubscriberState.Subscribed;
//                        _id = (Guid)rsp.Id;
//                        //initialize identity.
//                        _binaryIdentity = new byte[20];
//                        //copy it to binary array top 16 offset.
//                        Buffer.BlockCopy(_id.ToByteArray(), 0, _binaryIdentity, 0, 16);
//                        _policy = rsp.Policy;
//                        _startTime = DateTime.Now;
//                        Interlocked.Increment(ref _subscribedNodesCount);
//                        autoResetEvent.Set();
//                    }
//                };

//                #endregion
//                transaction.SendRequest(new SubscribeRequestMessage { Catalog = _catalog });
//                events[index++] = autoResetEvent;
//            }
            
//            if (!_event.WaitOne(SubscribeTimeout))
//            {
//                _state = SubscriberState.Exception;
//                exception = new System.Exception("Remote resource cannot subscribed. #reason: timeout!");
//            }
//            _event.Dispose();
//            _event = null;
//            if (exception != null) throw exception;
//        }
        
//        /// <summary>
//        ///     ��ȡһ�������������Ķ����߷��Ψһ����ֵ
//        /// </summary>
//        /// <returns>����Ψһ����ֵ</returns>
//        /// <exception cref="System.Exception">���Ϸ��Ķ��Ĺ�ϵ</exception>
//        protected unsafe byte[] GetIncrementSerialNo()
//        {
//            if (_binaryIdentity == null || _state != SubscriberState.Subscribed)
//                throw new System.Exception("You *MUST* create relations with remote publisher at frist!!!");
//            lock (_lockIdentity)
//            {
//                //start with no 1 at default.
//                fixed (byte* pByte = &_binaryIdentity[16])
//                    *(int*)pByte = ++_packageSerialId;
//                return _binaryIdentity;
//            }
//        }

//        #endregion

//        #region Events

//        void ChannelReceivedMessage(object sender, LightSingleArgEventArgs<System.Collections.Generic.List<BaseMessage>> e)
//        {
//            foreach (BaseMessage msg in e.Target)
//            {
//                _tracing.Info(msg.ToString());
//                if (!msg.Identity.IsRequest) SyncDataTransactionManager.Instance.Active(msg.Identity, msg);
//                else
//                {
//                    SyncDataTransaction transaction = new SyncDataTransaction(msg.Identity, _msgChannel) { Request = msg };
//                    SyncDataRequestMessage reqMsg = (SyncDataRequestMessage)msg;
//                    //send response msg.
//                    try
//                    {
//                        transaction.SendResponse(new SyncDataResponseMessage { Identity = reqMsg.Identity });
//                        DataRecvEventArgs<K, V> args = new DataRecvEventArgs<K, V>(_catalog, (K)DataHelper.GetObject(typeof(K), reqMsg.Key), (V)DataHelper.GetObject(typeof(V), reqMsg.Value));
//                        MessageRecvHandler(new LightSingleArgEventArgs<DataRecvEventArgs<K, V>>(args));
//                    }
//                    catch (System.Exception ex) { _tracing.Error(ex, null); }
//                }
//            }
//        }

//        void ChannelDisconnected(object sender, System.EventArgs e)
//        {
//            Close();
//            DisconnectedHandler(null);
//        }

//        #endregion
//    }
//}