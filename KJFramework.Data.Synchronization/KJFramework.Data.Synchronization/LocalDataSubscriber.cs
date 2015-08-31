using System;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.Policies;
using KJFramework.Data.Synchronization.Transactions;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;
using KJFramework.Net;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction.Helpers;
using KJFramework.Net.Transaction.ValueStored;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     本地数据订阅者
    /// </summary>
    internal class LocalDataSubscriber : ILocalDataSubscriber
    {
        #region Constructor

        /// <summary>
        ///     本地数据订阅者
        /// </summary>
        /// <param name="policy">发布者策略 </param>
        /// <param name="channel">通信信道</param>
        public LocalDataSubscriber(IPublisherPolicy policy, IMessageTransportChannel<MetadataContainer> channel)
        {
            if (policy == null) throw new ArgumentNullException("policy");
            if (channel == null) throw new ArgumentNullException("channel");
            Policy = policy;
            _channel = channel;
            _channel.Disconnected += ChannelDisconnected;
            _id = Guid.NewGuid();
            _startTime = DateTime.Now;
            _state = SubscriberState.Subscribed;
        }

        #endregion

        #region Members

        private IMessageTransportChannel<MetadataContainer> _channel;

        #endregion

        #region Implementation of ISubscriber

        private readonly Guid _id;
        private readonly DateTime _startTime;
        private SubscriberState _state;

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

        #region Implementation of ILocalDataSubscriber

        /// <summary>
        ///     获取发布者策略
        /// </summary>
        public IPublisherPolicy Policy { get; private set; }

        /// <summary>
        ///     关闭订阅者
        ///     <para>* 此操作将会导致关闭与订阅者之间的通信信道</para>
        /// </summary>
        public void Close()
        {
            if(_state != SubscriberState.Disconnected)
            {
                if (_channel != null)
                {
                    _channel.Disconnected -= ChannelDisconnected;
                    _channel.Close();
                    _channel = null;
                }
                _state = SubscriberState.Disconnected;
            }
        }

        /// <summary>
        ///     发送数据
        /// </summary>
        /// <param name="catalog">分组名称</param>
        /// <param name="key">要发送的KEY数据</param>
        /// <param name="value">要发送的VALUE数据</param>
        /// <returns>返回发送的状态</returns>
        /// <exception cref="System.NullReferenceException">非法数据</exception>
        public bool Send(string catalog, byte[] key, byte[] value)
        {
            if (string.IsNullOrEmpty(catalog)) throw new ArgumentNullException("catalog");
            if (key == null) throw new ArgumentNullException("key");
            //channel disconnected.
            if(_channel == null || !_channel.IsConnected)
            {
                Close();
                return false;
            }
            MetadataContainer container = (MetadataContainer) new MetadataContainer().SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity
                    {
                        ProtocolId = 2,
                        ServiceId = 0,
                        DetailsId = 0
                    }))
                .SetAttribute(0x0A, new StringValueStored(catalog))
                .SetAttribute(0x0B, new ByteArrayValueStored(key))
                .SetAttribute(0x0C, new ByteArrayValueStored(value));
            SyncDataTransaction transaction = SyncDataTransactionManager.Instance.Create(IdentityHelper.Create(_channel.RemoteEndPoint, Policy.IsOneway, _channel.ChannelType), _channel);
            //there is only Timeout event can be used.
            transaction.Timeout += delegate
            {
                if (!Policy.CanRetry) return;
                if (transaction.RetryCount >= Policy.RetryCount) return;
                transaction.RetryCount++;
                transaction.SendRequest(container);
            };
            transaction.Failed += delegate
            {
                if (!Policy.CanRetry) return;
                if (transaction.RetryCount >= Policy.RetryCount) return;
                transaction.RetryCount++;
                transaction.SendRequest(container);
            };
            transaction.SendRequest(container);
            return true;
        }

        #endregion

        #region Events

        //channel disconnected event.
        void ChannelDisconnected(object sender, System.EventArgs e)
        {
            Close();
            DisconnectedHandler(null);
        }

        #endregion
    }
}