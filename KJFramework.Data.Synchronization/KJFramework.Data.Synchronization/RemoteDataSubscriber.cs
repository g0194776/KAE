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
    ///     远程数据订阅者
    /// </summary>
    /// <typeparam name="K">数据KEY类型</typeparam>
    /// <typeparam name="V">数据VALUE类型</typeparam>
    internal class RemoteDataSubscriber<K, V> : IRemoteDataSubscriber<K, V>
    {
        #region Constructor

        /// <summary>
        ///     远程数据订阅者
        /// </summary>
        /// <param name="catelog">分类名称</param>
        /// <param name="res">网络资源</param>
        /// <param name="isAutoReconnect">是否自动启动重连的标识</param>
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
        ///     获取订阅者的唯一标示
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     获取订阅者的订阅时间
        /// </summary>
        public DateTime StartTime
        {
            get { return _startTime; }
        }

        /// <summary>
        ///     获取订阅者当前的状态
        /// </summary>
        public SubscriberState State
        {
            get { return _state; }
        }

        /// <summary>
        ///     订阅者断开连接事件
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
        ///     获取或设置一个值，该值标示了当前的远程数据订阅者在断开数据连接后是否启用自动重连机制
        /// </summary>
        public bool IsAutoReconnect { get; set; }

        /// <summary>
        ///     获取当前订阅者所订阅的类别
        /// </summary>
        public string Catalog
        {
            get { return _catalog; }
        }

        /// <summary>
        ///     获取远程发布者的策略信息
        /// </summary>
        public IPublisherPolicy Policy
        {
            get { return _policy; }
        }

        /// <summary>
        ///     获取当前订阅者所使用的网络资源
        /// </summary>
        public INetworkResource Resource
        {
            get { return _resource; }
        }

        /// <summary>
        ///     获取或设置订阅超时时间
        ///     <para>* 默认时间: 30s.</para>
        /// </summary>
        public TimeSpan SubscribeTimeout { get; set; }

        /// <summary>
        ///     绑定一个网络资源到当前的订阅者
        /// </summary>
        /// <param name="res">网络资源</param>
        /// <exception cref="System.Exception">无效的网络资源</exception>
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
        ///     关闭当前订阅者
        ///     <para>* 此操作将会导致断开与远程发布者的通信信道</para>
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
        ///     开启订阅者
        /// </summary>
        /// <returns>返回开启后的状态</returns>
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
        ///     接收到数据同步的消息事件
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
        ///     准备重连的线程
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
        ///     初始化订阅关系
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