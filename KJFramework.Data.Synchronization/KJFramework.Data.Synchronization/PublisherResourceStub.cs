using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.Data.Synchronization.Transactions;
using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Tracing;
using System;
using System.Threading;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     ��������Դ���
    /// </summary>
    internal class PublisherResourceStub : IPublisherResourceStub
    {
        #region Members

        private bool _disposed;
        private TcpHostTransportChannel _hostChannel;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(PublisherResourceStub));

        #endregion

        #region Implementation of IPublisherResourceStub

        private int _useCount;

        /// <summary>
        ///     ��ȡ��ǰ��Դ�����ʹ�ù�������
        /// </summary>
        public int UseCount
        {
            get { return _useCount; }
        }
        /// <summary>
        ///     ��ȡ��ԴΨһ��ʶ
        /// </summary>
        public string ResourceKey { get; private set; }

        /// <summary>
        ///     ��������Դ
        /// </summary>
        /// <param name="res">������Դ</param>
        public void Bind(INetworkResource res)
        {
            if (res == null) throw new ArgumentNullException("res");
            if (_useCount != 0) throw new System.Exception("You cannot bind more than one LOCAL resource at current stub!");
            int port = res.GetResource<int>();
            _hostChannel = new TcpHostTransportChannel(port);
            if (!_hostChannel.Regist()) throw new System.Exception("Cannot bind LOCAL network resource! #res: " + res);
            _hostChannel.ChannelCreated += ChannelCreated;
            ResourceKey = res.ToString();
        }

        /// <summary>
        ///     ����������Դ���
        /// </summary>
        public void Discard()
        {
            Interlocked.Decrement(ref _useCount);
            if (_useCount <= 0 && !_disposed)
            {
                _disposed = true;
                _useCount = 0;
                _hostChannel.ChannelCreated -= ChannelCreated;
                _hostChannel.UnRegist();
                _hostChannel = null;
                DisposedHandler(null);
            }
        }

        /// <summary>
        ///     ��ע���¼�
        /// </summary>
        public event EventHandler Disposed;
        protected void DisposedHandler(System.EventArgs e)
        {
            EventHandler handler = Disposed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     �������¼�
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<NewTransactionEventArgs>> NewTransaction;
        protected void NewTransactionHandler(LightSingleArgEventArgs<NewTransactionEventArgs> e)
        {
            EventHandler<LightSingleArgEventArgs<NewTransactionEventArgs>> handler = NewTransaction;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        ///      �ڲ�����������Դ��ʹ����
        /// </summary>
        internal void AddUseRef()
        {
            Interlocked.Increment(ref _useCount);
        }

        #endregion

        #region Events

        void ChannelCreated(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {
            IMessageTransportChannel<BaseMessage> msgChannel = new MessageTransportChannel<BaseMessage>((IRawTransportChannel) e.Target, Global.ProtocolStack);
            msgChannel.Disconnected += MsgChannelDisconnected;
            msgChannel.ReceivedMessage += MsgChannelReceivedMessage;
        }

        void MsgChannelReceivedMessage(object sender, LightSingleArgEventArgs<System.Collections.Generic.List<BaseMessage>> e)
        {
            IMessageTransportChannel<BaseMessage> msgChannel = (IMessageTransportChannel<BaseMessage>) sender;
            foreach (BaseMessage msg in e.Target)
            {
                _tracing.Info("L: {0}\r\nR: {1}\r\n{2}", msgChannel.LocalEndPoint, msgChannel.RemoteEndPoint, msg.ToString());
                if (!msg.TransactionIdentity.IsRequest) SyncDataTransactionManager.Instance.Active(msg.TransactionIdentity, msg);
                else NewTransactionHandler(new LightSingleArgEventArgs<NewTransactionEventArgs>(new NewTransactionEventArgs(new SyncDataTransaction(msgChannel) { Identity = msg.TransactionIdentity, Request = msg }, msgChannel)));
            }
        }

        void MsgChannelDisconnected(object sender, System.EventArgs e)
        {
            IMessageTransportChannel<BaseMessage> msgChannel = (IMessageTransportChannel<BaseMessage>) sender;
            msgChannel.Disconnected -= MsgChannelDisconnected;
            msgChannel.ReceivedMessage -= MsgChannelReceivedMessage;
        }

        #endregion
    }
}