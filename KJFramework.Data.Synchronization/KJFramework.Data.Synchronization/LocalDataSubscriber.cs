using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.Messages;
using KJFramework.Data.Synchronization.Policies;
using KJFramework.Data.Synchronization.Transactions;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction.Helpers;
using KJFramework.Net.Transaction.Messages;
using System;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     �������ݶ�����
    /// </summary>
    internal class LocalDataSubscriber : ILocalDataSubscriber
    {
        #region Constructor

        /// <summary>
        ///     �������ݶ�����
        /// </summary>
        /// <param name="policy">�����߲��� </param>
        /// <param name="channel">ͨ���ŵ�</param>
        public LocalDataSubscriber(IPublisherPolicy policy, IMessageTransportChannel<BaseMessage> channel)
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

        private IMessageTransportChannel<BaseMessage> _channel;

        #endregion

        #region Implementation of ISubscriber

        private readonly Guid _id;
        private readonly DateTime _startTime;
        private SubscriberState _state;

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

        #region Implementation of ILocalDataSubscriber

        /// <summary>
        ///     ��ȡ�����߲���
        /// </summary>
        public IPublisherPolicy Policy { get; private set; }

        /// <summary>
        ///     �رն�����
        ///     <para>* �˲������ᵼ�¹ر��붩����֮���ͨ���ŵ�</para>
        /// </summary>
        public void Close()
        {
            if(_state != SubscriberState.Disconnected)
            {
                _channel.Disconnected -= ChannelDisconnected;
                _channel.Close();
                _channel = null;
                _state = SubscriberState.Disconnected;
            }
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="catalog">��������</param>
        /// <param name="key">Ҫ���͵�KEY����</param>
        /// <param name="value">Ҫ���͵�VALUE����</param>
        /// <returns>���ط��͵�״̬</returns>
        /// <exception cref="System.NullReferenceException">�Ƿ�����</exception>
        public bool Send(string catalog, byte[] key, byte[] value)
        {
            if (string.IsNullOrEmpty(catalog)) throw new ArgumentNullException("catalog");
            if (key == null) throw new ArgumentNullException("key");
            //channel disconnected.
            if(!_channel.IsConnected)
            {
                Close();
                return false;
            }
            SyncDataRequestMessage msg = new SyncDataRequestMessage { Key = key, Value = value, Catalog = catalog };
            SyncDataTransaction transaction = SyncDataTransactionManager.Instance.Create(IdentityHelper.Create(_channel.RemoteEndPoint, Policy.IsOneway), _channel);
            //there is only Timeout event can be used.
            transaction.Timeout += delegate
            {
                if (!Policy.CanRetry) return;
                if (transaction.RetryCount >= Policy.RetryCount) return;
                transaction.RetryCount++;
                transaction.SendRequest(msg);
            };
            transaction.Failed += delegate
            {
                if (!Policy.CanRetry) return;
                if (transaction.RetryCount >= Policy.RetryCount) return;
                transaction.RetryCount++;
                transaction.SendRequest(msg);
            };
            transaction.SendRequest(msg);
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