using System;
using System.Net;
using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Helpers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction.Agent
{
    /// <summary>
    ///     连接代理器，提供了相关的基本操作
    /// </summary>
    public class IntellectObjectConnectionAgent : IServerConnectionAgent<BaseMessage>
    {
        #region Constructor

        /// <summary>
        ///     连接代理器，提供了相关的基本操作
        /// </summary>
        /// <param name="channel">消息通信信道</param>
        /// <param name="transactionManager">事务管理器</param>
        /// <exception cref="System.NullReferenceException">参数不能为空</exception>
        public IntellectObjectConnectionAgent(IMessageTransportChannel<BaseMessage> channel, MessageTransactionManager transactionManager)
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
        private IMessageTransportChannel<BaseMessage> _channel;
        protected static readonly ITracing _tracing = TracingManager.GetTracing(typeof (IntellectObjectConnectionAgent));

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
        void ChannelReceivedMessage(object sender, LightSingleArgEventArgs<System.Collections.Generic.List<BaseMessage>> e)
        {
            IMessageTransportChannel<BaseMessage> msgChannel = (IMessageTransportChannel<BaseMessage>) sender;
            foreach (BaseMessage message in e.Target)
            {
                if (message == null) continue;
                _tracing.Info("L: {0}\r\nR: {1}\r\n{2}", msgChannel.LocalEndPoint, msgChannel.RemoteEndPoint, message.ToString());
                TransactionIdentity identity;
                if (message.TransactionIdentity == null) identity = msgChannel.GenerateRequestIdentity(0U);
                else identity = message.TransactionIdentity;
                //response.
                if(!identity.IsRequest)
                {
                    _transactionManager.Active(identity, message);
                    continue;
                }
                //create transaction by IsOneway flag.
                IMessageTransaction<BaseMessage> transaction = new BusinessMessageTransaction(_channel)
                {
                    NeedResponse = !identity.IsOneway,
                    TransactionManager = _transactionManager,
                    Identity = identity,
                    Request = message
                };
                NewTransactionHandler(new LightSingleArgEventArgs<IMessageTransaction<BaseMessage>>(transaction));
            }
        }

        #endregion

        #region Implementation of IConnectionAgent

        private MessageTransactionManager _transactionManager;
        private object _tag;

        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     获取事务管理器
        /// </summary>
        public MessageTransactionManager TransactionManager
        {
            get { return _transactionManager; }
        }

        /// <summary>
        ///     主动关闭连接代理器
        ///     * 主动关闭的行为将会关闭内部的通信信道连接
        /// </summary>
        public void Close()
        {
            _isInitiativeDisconnect = true;
            _channel.Disconnect();
            DisconnectedHandler(null);
        }

        /// <summary>
        ///     获取内部的通信信道
        ///     * 一般不建议使用此方法直接操作内部的通信信道
        /// </summary>
        /// <returns>返回通信信道</returns>
        public IMessageTransportChannel<BaseMessage> GetChannel()
        {
            return _channel;
        }

        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <returns>返回新创建的事务</returns>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public MessageTransaction<BaseMessage> CreateTransaction()
        {
            return _transactionManager.Create(IdentityHelper.Create(_channel.LocalEndPoint, _channel.ChannelType), _channel);
        }

        /// <summary>
        ///     创建一个新的单向事务
        /// </summary>
        /// <returns>返回新创建的单向事务</returns>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public MessageTransaction<BaseMessage> CreateOnewayTransaction()
        {
            return _transactionManager.Create(IdentityHelper.CreateOneway(_channel.LocalEndPoint, _channel.ChannelType), _channel);
        }

        /// <summary>
        ///     断开事件
        /// </summary>
        public event EventHandler Disconnected;
        private void DisconnectedHandler(System.EventArgs e)
        {
            EventHandler handler = Disconnected;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     新的事物创建被创建时激活此事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<IMessageTransaction<BaseMessage>>> NewTransaction;
        private void NewTransactionHandler(LightSingleArgEventArgs<IMessageTransaction<BaseMessage>> e)
        {
            EventHandler<LightSingleArgEventArgs<IMessageTransaction<BaseMessage>>> handler = NewTransaction;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     创建一个新的连接代理器
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="transactionManager">事务管理器</param>
        /// <returns>如果无法连接到远程地址，则返回null.</returns>
        /// <exception cref="System.ArgumentNullException">非法参数</exception>
        public static IServerConnectionAgent<BaseMessage> Create(IPEndPoint iep, IProtocolStack<BaseMessage> protocolStack, MessageTransactionManager transactionManager)
        {
            if (iep == null) throw new ArgumentNullException("iep");
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            if (transactionManager == null) throw new ArgumentNullException("transactionManager");
            ITransportChannel transportChannel = new TcpTransportChannel(iep);
            transportChannel.Connect();
            if (!transportChannel.IsConnected) return null;
            IMessageTransportChannel<BaseMessage> msgChannel = new MessageTransportChannel<BaseMessage>((IRawTransportChannel) transportChannel, protocolStack);
            return new IntellectObjectConnectionAgent(msgChannel, transactionManager);
        }

        #endregion
    }
}