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
    ///     连接代理器，提供了相关的基本操作
    /// </summary>
    public class MetadataConnectionAgent : IServerConnectionAgent<MetadataContainer>
    {
        #region Constructor

        /// <summary>
        ///     连接代理器，提供了相关的基本操作
        /// </summary>
        /// <param name="channel">消息通信信道</param>
        /// <param name="transactionManager">事务管理器</param>
        /// <exception cref="System.NullReferenceException">参数不能为空</exception>
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
        public MetadataTransactionManager TransactionManager
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
        public IMessageTransportChannel<MetadataContainer> GetChannel()
        {
            return _channel;
        }

        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <returns>返回新创建的事务</returns>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public MessageTransaction<MetadataContainer> CreateTransaction()
        {
            return _transactionManager.Create(IdentityHelper.Create(_channel.LocalEndPoint, _channel.ChannelType), _channel);
        }

        /// <summary>
        ///     创建一个新的单向事务
        /// </summary>
        /// <returns>返回新创建的单向事务</returns>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public MessageTransaction<MetadataContainer> CreateOnewayTransaction()
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
        public event EventHandler<LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>>> NewTransaction;
        private void NewTransactionHandler(LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>> e)
        {
            EventHandler<LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>>> handler = NewTransaction;
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