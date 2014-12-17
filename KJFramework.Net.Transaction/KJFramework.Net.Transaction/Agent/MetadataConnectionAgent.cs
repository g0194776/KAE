using System;
using System.Collections.Generic;
using System.Net;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Helpers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction.Agent
{
    /// <summary>
    ///     ���Ӵ��������ṩ����صĻ�������
    /// </summary>
    public class MetadataConnectionAgent : IServerConnectionAgent<MetadataContainer>
    {
        #region Constructor

        /// <summary>
        ///     ���Ӵ��������ṩ����صĻ�������
        /// </summary>
        /// <param name="channel">��Ϣͨ���ŵ�</param>
        /// <param name="transactionManager">���������</param>
        /// <exception cref="System.NullReferenceException">��������Ϊ��</exception>
        public MetadataConnectionAgent(IMessageTransportChannel<MetadataContainer> channel, MetadataTransactionManager transactionManager)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            if (!channel.IsConnected) throw new ArgumentException("Cannot wrap this msg channel, because current msg channel has been disconnected!");
            if (transactionManager == null) throw new ArgumentNullException("transactionManager");
            _channel = channel;
            _transactionManager = transactionManager;
            _channel.Disconnected += ChannelDisconnected;
            _channel.ReceivedMessage += ChannelReceivedMessage;
        }

        #endregion

        #region Members

        private bool _isInitiativeDisconnect;
        private IMessageTransportChannel<MetadataContainer> _channel;
        protected static readonly ITracing _tracing = TracingManager.GetTracing(typeof(MetadataConnectionAgent));

        #endregion

        #region Events

        //inner msg channel disconect event.
        void ChannelDisconnected(object sender, System.EventArgs e)
        {
            _channel.Disconnected -= ChannelDisconnected;
            _channel.ReceivedMessage -= ChannelReceivedMessage;
            if (_isInitiativeDisconnect) return;
            DisconnectedHandler(null);
        }

        //new message arrived.
        void ChannelReceivedMessage(object sender, LightSingleArgEventArgs<List<MetadataContainer>> e)
        {
            IMessageTransportChannel<MetadataContainer> msgChannel = (IMessageTransportChannel<MetadataContainer>)sender;
            foreach (MetadataContainer message in e.Target)
            {
                if (message == null) continue;
                _tracing.Info("L: {0}\r\nR: {1}\r\n{2}", msgChannel.LocalEndPoint, msgChannel.RemoteEndPoint, message.ToString());
                TransactionIdentity identity;
                if (!message.IsAttibuteExsits(0x01)) identity = msgChannel.GenerateRequestIdentity(0U);
                else identity = message.GetAttribute(0x01).GetValue<TransactionIdentity>();
                //response.
                if(!identity.IsRequest)
                {
                    _transactionManager.Active(identity, message);
                    continue;
                }
                //create transaction by IsOneway flag.
                IMessageTransaction<MetadataContainer> transaction = new MetadataMessageTransaction(_channel)
                {
                    NeedResponse = !identity.IsOneway,
                    TransactionManager = _transactionManager,
                    Identity = identity,
                    Request = message
                };
                NewTransactionHandler(new LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>>(transaction));
            }
        }

        #endregion

        #region Implementation of IConnectionAgent

        private MetadataTransactionManager _transactionManager;
        private object _tag;

        /// <summary>
        ///     ��ȡ�����ø�������
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     ��ȡ���������
        /// </summary>
        public MetadataTransactionManager TransactionManager
        {
            get { return _transactionManager; }
        }

        /// <summary>
        ///     �����ر����Ӵ�����
        ///     * �����رյ���Ϊ����ر��ڲ���ͨ���ŵ�����
        /// </summary>
        public void Close()
        {
            _isInitiativeDisconnect = true;
            _channel.Disconnect();
            DisconnectedHandler(null);
        }

        /// <summary>
        ///     ��ȡ�ڲ���ͨ���ŵ�
        ///     * һ�㲻����ʹ�ô˷���ֱ�Ӳ����ڲ���ͨ���ŵ�
        /// </summary>
        /// <returns>����ͨ���ŵ�</returns>
        public IMessageTransportChannel<MetadataContainer> GetChannel()
        {
            return _channel;
        }

        /// <summary>
        ///     ����һ���µ�����
        /// </summary>
        /// <returns>�����´���������</returns>
        /// <exception cref="ArgumentNullException">��������</exception>
        public MessageTransaction<MetadataContainer> CreateTransaction()
        {
            return _transactionManager.Create(IdentityHelper.Create(_channel.LocalEndPoint, _channel.ChannelType), _channel);
        }

        /// <summary>
        ///     ����һ���µĵ�������
        /// </summary>
        /// <returns>�����´����ĵ�������</returns>
        /// <exception cref="ArgumentNullException">��������</exception>
        public MessageTransaction<MetadataContainer> CreateOnewayTransaction()
        {
            return _transactionManager.Create(IdentityHelper.CreateOneway(_channel.LocalEndPoint, _channel.ChannelType), _channel);
        }

        /// <summary>
        ///     �Ͽ��¼�
        /// </summary>
        public event EventHandler Disconnected;
        private void DisconnectedHandler(System.EventArgs e)
        {
            EventHandler handler = Disconnected;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     �µ����ﴴ��������ʱ������¼�
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>>> NewTransaction;
        private void NewTransactionHandler(LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>> e)
        {
            EventHandler<LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>>> handler = NewTransaction;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     ����һ���µ����Ӵ�����
        /// </summary>
        /// <param name="iep">Զ���ս���ַ</param>
        /// <param name="protocolStack">Э��ջ</param>
        /// <param name="transactionManager">���������</param>
        /// <returns>����޷����ӵ�Զ�̵�ַ���򷵻�null.</returns>
        /// <exception cref="System.ArgumentNullException">�Ƿ�����</exception>
        public static IServerConnectionAgent<MetadataContainer> Create(IPEndPoint iep, IProtocolStack protocolStack, MetadataTransactionManager transactionManager)
        {
            if (iep == null) throw new ArgumentNullException("iep");
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            if (transactionManager == null) throw new ArgumentNullException("transactionManager");
            ITransportChannel transportChannel = new TcpTransportChannel(iep);
            transportChannel.Connect();
            if (!transportChannel.IsConnected) return null;
            IMessageTransportChannel<MetadataContainer> msgChannel = new MessageTransportChannel<MetadataContainer>((IRawTransportChannel)transportChannel, protocolStack);
            return new MetadataConnectionAgent(msgChannel, transactionManager);
        }

        #endregion
    }
}