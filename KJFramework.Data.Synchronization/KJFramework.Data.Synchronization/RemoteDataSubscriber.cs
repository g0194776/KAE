using System;
using System.Net;
using System.Threading;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.Data.Synchronization.Policies;
using KJFramework.Data.Synchronization.Transactions;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     Զ�����ݶ�����
    /// </summary>
    /// <typeparam name="K">����KEY����</typeparam>
    /// <typeparam name="V">����VALUE����</typeparam>
    internal class RemoteDataSubscriber<K, V> : IRemoteDataSubscriber<K, V>
    {
        #region Constructor

        /// <summary>
        ///     Զ�����ݶ�����
        /// </summary>
        /// <param name="catelog">��������</param>
        /// <param name="res">������Դ</param>
        /// <param name="isAutoReconnect">�Ƿ��Զ����������ı�ʶ</param>
        public RemoteDataSubscriber(string catelog, INetworkResource res, bool isAutoReconnect)
        {
            if (string.IsNullOrEmpty(catelog)) throw new ArgumentNullException("catelog");
            if (res == null) throw new ArgumentNullException("res");
            SubscribeTimeout = new TimeSpan(0, 0, 30);
            _resource = res;
            _catalog = catelog;
            IsAutoReconnect = isAutoReconnect;
            Bind(res);
        }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (RemoteDataSubscriber<K, V>));
        private IMessageTransportChannel<MetadataContainer> _msgChannel;
        private AutoResetEvent _event;
        //reconnect time start at 3s, step N*2.
        private int _reconnectTime = 3000;
        private Thread _reconnectThread;

        #endregion

        #region Implementation of ISubscriber

        private Guid _id;
        private DateTime _startTime;
        private SubscriberState _state = SubscriberState.Unknown;
        private string _catalog;
        private IPublisherPolicy _policy;
        private INetworkResource _resource;

        /// <summary>
        ///     ��ȡ�����ߵ�Ψһ��ʾ
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     ��ȡ�����ߵĶ���ʱ��
        /// </summary>
        public DateTime StartTime
        {
            get { return _startTime; }
        }

        /// <summary>
        ///     ��ȡ�����ߵ�ǰ��״̬
        /// </summary>
        public SubscriberState State
        {
            get { return _state; }
        }

        /// <summary>
        ///     �����߶Ͽ������¼�
        /// </summary>
        public event EventHandler Disconnected;
        protected void DisconnectedHandler(System.EventArgs e)
        {
            EventHandler handler = Disconnected;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Implementation of IRemoteDataSubscriber<K,V>

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ��Զ�����ݶ������ڶϿ��������Ӻ��Ƿ������Զ���������
        /// </summary>
        public bool IsAutoReconnect { get; set; }

        /// <summary>
        ///     ��ȡ��ǰ�����������ĵ����
        /// </summary>
        public string Catalog
        {
            get { return _catalog; }
        }

        /// <summary>
        ///     ��ȡԶ�̷����ߵĲ�����Ϣ
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
        ///     ��ȡ�����ö��ĳ�ʱʱ��
        ///     <para>* Ĭ��ʱ��: 30s.</para>
        /// </summary>
        public TimeSpan SubscribeTimeout { get; set; }

        /// <summary>
        ///     ��һ��������Դ����ǰ�Ķ�����
        /// </summary>
        /// <param name="res">������Դ</param>
        /// <exception cref="System.Exception">��Ч��������Դ</exception>
        public void Bind(INetworkResource res)
        {
            if (res == null) throw new ArgumentNullException("res");
            if (res.Mode != ResourceMode.Remote) throw new ArgumentException("Cannot create remote data subscriber by LOCAL resource mode! #mode: " + res.Mode);
            if (_state == SubscriberState.Subscribed) throw new System.Exception("You cannot bind remote resources twice!!!");
            IPEndPoint iep = res.GetResource<IPEndPoint>();
            TcpTransportChannel channel = new TcpTransportChannel(iep);
            channel.Connect();
            if (!channel.IsConnected)
            {
                if (!IsAutoReconnect) throw new System.Exception("Remote endpoint cannot reachable! #addr: " + iep);
                _state = SubscriberState.WaitReconnect;
                //run reconnect thread.
                if (_reconnectThread == null) PrepareReconnectThread();
                return;
            }
            _msgChannel = new MessageTransportChannel<MetadataContainer>(channel, Global.ProtocolStack);
            _msgChannel.Disconnected += ChannelDisconnected;
            _msgChannel.ReceivedMessage += ChannelReceivedMessage;
            _state = SubscriberState.ToBeSubscribe;
        }

        /// <summary>
        ///     �رյ�ǰ������
        ///     <para>* �˲������ᵼ�¶Ͽ���Զ�̷����ߵ�ͨ���ŵ�</para>
        /// </summary>
        public void Close()
        {
            if(_state != SubscriberState.Disconnected)
            {
                _state = SubscriberState.Disconnected;
                _msgChannel.Disconnected -= ChannelDisconnected;
                _msgChannel.ReceivedMessage -= ChannelReceivedMessage;
                _msgChannel.Close();
                _msgChannel = null;
            }
        }

        /// <summary>
        ///     ����������
        /// </summary>
        /// <returns>���ؿ������״̬</returns>
        public SubscriberState Open()
        {
            if (_state == SubscriberState.WaitReconnect) return _state;
            if (_state != SubscriberState.ToBeSubscribe) throw new System.Exception("You *MUST* call Bind method at first time!");
            if (_msgChannel == null) throw new System.Exception("Inner failed!  #reason: inner message channel is null.");
            if (!_msgChannel.IsConnected) throw new System.Exception("Inner failed!  #reason: inner message channel has been disconnected.");
            InitializeRelation();
            return _state;
        }

        /// <summary>
        ///     ���յ�����ͬ������Ϣ�¼�
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<DataRecvEventArgs<K, V>>> MessageRecv;
        protected void MessageRecvHandler(LightSingleArgEventArgs<DataRecvEventArgs<K, V>> e)
        {
            EventHandler<LightSingleArgEventArgs<DataRecvEventArgs<K, V>>> handler = MessageRecv;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     ׼���������߳�
        /// </summary>
        private void PrepareReconnectThread()
        {
            if (_reconnectThread != null) return;
            //reconnect target publisher.
            _reconnectThread = new Thread(delegate()
            {
                while (true)
                {
                    try
                    {
                        Bind(_resource);
                        if (_state == SubscriberState.ToBeSubscribe && Open() == SubscriberState.Subscribed)
                        {
                            //reset to default value.
                            _reconnectTime = 3000;
                            _reconnectThread = null;
                            break;
                        }
                    }
                    catch (System.Exception ex) { _tracing.Error(ex, null); }
                    //check again.
                    if (!IsAutoReconnect)
                    {
                        _reconnectThread = null;
                        _state = SubscriberState.Disconnected;
                        return;
                    }
                    _reconnectTime = _reconnectTime * 2;
                    Thread.Sleep(_reconnectTime);
                }
            }) { Name = "Thread::ReconnectPublisher" };
            _reconnectThread.Start();
        }

        /// <summary>
        ///     ��ʼ�����Ĺ�ϵ
        /// </summary>
        private void InitializeRelation()
        {
            System.Exception exception = null;
            _event = new AutoResetEvent(false);
            #region Create transaction.

            SyncDataTransaction transaction = SyncDataTransactionManager.Instance.Create(_msgChannel);
            transaction.Failed += delegate
            {
                exception = new System.Exception("Remote resource cannot subscribed. #reason: network failed!");
                _state = SubscriberState.Exception;
                _event.Set();
            };
            transaction.Timeout += delegate
            {
                exception = new System.Exception("Remote resource cannot subscribed. #reason: get rsp msessage timeout!");
                _state = SubscriberState.Exception;
                _event.Set();
            };
            transaction.ResponseArrived += delegate(object sender, LightSingleArgEventArgs<MetadataContainer> e)
            {
                MetadataContainer rsp = e.Target;
                if ((SubscribeResult)rsp.GetAttributeAsType<byte>(0x0D) == SubscribeResult.Reject)
                {
                    exception = new System.Exception("Remote resource cannot subscribed. #reason: " + (SubscribeResult)rsp.GetAttributeAsType<byte>(0x0D));
                    _state = SubscriberState.Disconnected;
                    if (_event != null) _event.Set();
                }
                else
                {
                    _state = SubscriberState.Subscribed;
                    _id = rsp.GetAttributeAsType<Guid>(0x0E);
                    ResourceBlock rb = rsp.GetAttribute(0x0C).GetValue<ResourceBlock>();
                    _policy = new PublisherPolicy
                    {
                        CanRetry = rb.GetAttributeAsType<bool>(0x00),
                        IsOneway = rb.GetAttributeAsType<bool>(0x01),
                        RetryCount = rb.GetAttributeAsType<byte>(0x02),
                        TimeoutSec = rb.GetAttributeAsType<int>(0x03)
                    };
                    _startTime = DateTime.Now;
                    if (_event != null) _event.Set();
                }
            };

            #endregion
            transaction.SendRequest((MetadataContainer)new MetadataContainer()
                .SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity
                    {
                        ProtocolId = 0,
                        ServiceId = 0,
                        DetailsId = 0
                    }))
                .SetAttribute(0x0A, new StringValueStored(_catalog)));
            if (!_event.WaitOne(SubscribeTimeout))
            {
                _state = SubscriberState.Exception;
                exception = new System.Exception("Remote resource cannot subscribed. #reason: timeout!");
            }
            _event.Dispose();
            _event = null;
            if (exception != null) throw exception;
        }

        #endregion

        #region Events

        void ChannelReceivedMessage(object sender, LightSingleArgEventArgs<System.Collections.Generic.List<MetadataContainer>> e)
        {
            IMessageTransportChannel<MetadataContainer> msgChannel = (IMessageTransportChannel<MetadataContainer>)sender;
            foreach (MetadataContainer msg in e.Target)
            {
                _tracing.Info("L: {0}\r\nR: {1}\r\n{2}", msgChannel.LocalEndPoint, msgChannel.RemoteEndPoint, msg.ToString());
                if (!msg.GetAttribute(0x01).GetValue<TransactionIdentity>().IsRequest) SyncDataTransactionManager.Instance.Active(msg.GetAttribute(0x01).GetValue<TransactionIdentity>(), msg);
                else
                {
                    SyncDataTransaction transaction = new SyncDataTransaction(_msgChannel) { Identity = msg.GetAttribute(0x01).GetValue<TransactionIdentity>(), Request = msg };
                    //send response msg.
                    try
                    {
                        transaction.SendResponse((MetadataContainer)new MetadataContainer().SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity
                        {
                            ProtocolId = 2,
                            ServiceId = 0,
                            DetailsId = 1
                        })));
                        DataRecvEventArgs<K, V> args = new DataRecvEventArgs<K, V>(_catalog, (K)DataHelper.GetObject(typeof(K), msg.GetAttributeAsType<byte[]>(0x0B)), (V)DataHelper.GetObject(typeof(V), msg.GetAttributeAsType<byte[]>(0x0C)));
                        MessageRecvHandler(new LightSingleArgEventArgs<DataRecvEventArgs<K, V>>(args));
                    }
                    catch (System.Exception ex) { _tracing.Error(ex, null); }
                }
            }
        }

        void ChannelDisconnected(object sender, System.EventArgs e)
        {
            Close();
            if (!IsAutoReconnect) DisconnectedHandler(null);
            else PrepareReconnectThread();
        }

        #endregion
    }
}