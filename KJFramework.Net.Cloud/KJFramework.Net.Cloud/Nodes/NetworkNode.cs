using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Cloud.Accessors;
using KJFramework.Net.Cloud.Accessors.Rules;
using KJFramework.Net.Cloud.Enums;
using KJFramework.Net.Cloud.Exceptions;
using KJFramework.Net.Cloud.Objects;
using KJFramework.Net.Cloud.Virtuals;
using KJFramework.Net.Cloud.Virtuals.Accessors;
using KJFramework.Net.Cloud.Virtuals.Processors;
using KJFramework.Net.ProtocolStacks;
using KJFramework.ServiceModel.Bussiness.Default.Proxy;
using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Elements;
using KJFramework.ServiceModel.Enums;
using KJFramework.ServiceModel.Proxy;
using KJFramework.Tracing;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     网络节点，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public class NetworkNode<T> : INetworkNode<T>
    {
        #region 构造函数

        /// <summary>
        ///     网络节点，提供了相关的基本操作。
        /// </summary>
        public NetworkNode(IProtocolStack<T> protocolStack)
        {
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            if(!protocolStack.Initialize())throw new ArgumentException("Cannot initialize a protocol stack, #name: " + protocolStack);
            _protocolStack = protocolStack;
            _id = Guid.NewGuid();
        }

        #endregion

        #region Members

        protected readonly Guid _id;
        protected IAccessor _accessor;
        protected readonly IProtocolStack<T> _protocolStack;
        private Object _lockHostsObject = new Object();
        private Object _lockServiceObject = new Object();
        protected ConcurrentDictionary<Guid, IMessageTransportChannel<T>> _transportChannels = new ConcurrentDictionary<Guid, IMessageTransportChannel<T>>();
        protected Dictionary<Guid, IHostTransportChannel> _hostChannels = new Dictionary<Guid, IHostTransportChannel>();
        protected Dictionary<Uri, ServiceHost> _hostServices = new Dictionary<Uri, ServiceHost>();
        //protected LightTimer _timer;
        protected ArrayList _clients = new ArrayList();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(NetworkNode<T>));

        #endregion

        #region Implementation of INetServiceNode

        /// <summary>
        ///     连接到一个远程的服务节点
        /// </summary>
        /// <param name="binding">服务地址绑定对象</param>
        /// <returns>返回连接后的客户端代理</returns>
        public TContract Connect<TContract>(Binding binding)
            where TContract : class
        {
            try
            {
                IClientProxy<TContract> client = new DefaultClientProxy<TContract>(binding);
                //connect ok.
                if (client.Status == ProxyStatus.Opend)
                {
                    lock (_clients.SyncRoot) _clients.Add(client);
                    return client.Channel;
                }
                //connect failed.
                return null;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
        }

        /// <summary>
        ///     获取远程服务代理
        /// </summary>
        /// <typeparam name="TContract">远程服务契约类型</typeparam>
        /// <param name="uri">远程服务地址</param>
        /// <returns>返回客户端代理</returns>
        public TContract GetService<TContract>(Uri uri)
            where TContract : class
        {
            if (uri == null) throw new ArgumentNullException("uri");
            //lock (_clients.SyncRoot)
            //{
            //    try
            //    {
            //        foreach (Object client in _clients)
            //        {
            //            //find correct contract client.
            //            if (client is IClientProxy<TContract>)
            //            {
            //                IClientProxy<TContract> result = (IClientProxy<TContract>)client;
            //                //find it, compare logic address.
            //                if (result.LogicalAddress == uri)
            //                {
            //                    return result.Channel;
            //                }
            //            }
            //        }
            //        return null;
            //    }
            //    catch (System.Exception ex)
            //    {
            //        _tracing.Error(ex, null);
            //        return null;
            //    }
            //}
            return null;
        }

        /// <summary>
        ///     注册一个远程服务
        /// </summary>
        /// <param name="binding">绑定方式</param>
        /// <param name="type">契约类型</param>
        public void Regist(Binding binding, Type type)
        {
            if (binding == null || binding.LogicalAddress == null || type == null)
                throw new System.Exception("[Regist Host Service]非法的参数。");
            try
            {
                ServiceHost serviceHost = new ServiceHost(type, binding);
                serviceHost.Open();
                if (serviceHost.CommunicationState == CommunicationStates.Opened)
                {
                    lock (_lockServiceObject)
                    {
                        _hostServices.Add(binding.LogicalAddress, serviceHost);
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
        }

        /// <summary>
        ///     注销一个远程服务
        /// </summary>
        /// <param name="uri">调用地址</param>
        public void UnRegist(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            lock (_hostServices)
            {
                try
                {
                    if (!_hostServices.ContainsKey(uri)) return;
                    _hostServices[uri].Close();
                    _hostServices.Remove(uri);
                }
                catch (System.Exception ex)
                {
                    _tracing.Error(ex, null);
                    throw;
                }
            }
        }

        #endregion

        #region Implementation of INetworkNode<T>

        /// <summary>
        ///     获取或设置访问器
        /// </summary>
        public IAccessor Accessor
        {
            get { return _accessor; }
            set { _accessor = value; }
        }

        /// <summary>
        ///     获取唯一键值
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     获取或设置协议栈
        /// </summary>
        public IProtocolStack<T> ProtocolStack
        {
            get { return _protocolStack; }
        }

        /// <summary>
        ///     将指定元数据广播到所有当前所有的信道上。
        /// </summary>
        /// <param name="data">需要广播的元数据</param>
        public void Broadcast(byte[] data)
        {
            foreach (KeyValuePair<Guid, IMessageTransportChannel<T>> transportChannel in _transportChannels)
            {
                try
                {
                    //Check channel state.
                    if (transportChannel.Value.IsConnected)
                        transportChannel.Value.Send(data);
                }
                catch (System.Exception ex)
                {
                    _tracing.Error(ex, null);
                }
            }
        }

        /// <summary>
        ///   将指定消息广播到所有当前所有的信道上。 
        /// </summary>
        /// <param name="message">需要广播的消息</param>
        public void Broadcast(T message)
        {
            if (Comparer.Default.Compare(message, default(T)) == 0) return;
            foreach (IMessageTransportChannel<T> msgChannel  in _transportChannels.Values)
            {
                if (msgChannel.IsConnected) msgChannel.Send(message);
            }
        }

        /// <summary>
        ///     开启当前网络节点
        /// </summary>
        public void Open() { /*nothing.*/ }

        /// <summary>
        ///     关闭当前网络节点
        /// </summary>
        public void Close()
        {
            #region clear host channel.

            //clear host channel.
            lock (_lockHostsObject)
            {
                foreach (KeyValuePair<Guid, IHostTransportChannel> hostTransportChannel in _hostChannels)
                {
                    try
                    {
                        hostTransportChannel.Value.ChannelCreated -= ChannelCreated;
                        hostTransportChannel.Value.ChannelDisconnected -= ChannelDisconnected;
                        hostTransportChannel.Value.UnRegist();
                    }
                    catch (System.Exception ex) { _tracing.Error(ex, null); }
                }
                _hostChannels.Clear();
            }
            
            #endregion

            #region clear transport channel.

            //clear transport channel.
            foreach (IMessageTransportChannel<T> transportChannel in _transportChannels.Values)
            {
                try
                {
                    transportChannel.Disconnect();
                }
                catch (System.Exception ex)
                {
                    _tracing.Error(ex, null);
                }
            }
            _transportChannels.Clear();

            #endregion

            #region clear host service.

            //clear host service.
            lock (_lockServiceObject)
            {
                foreach (KeyValuePair<Uri, ServiceHost> hostService in _hostServices)
                {
                    try
                    {
                        hostService.Value.Close();
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, null);
                    }
                }
                _hostServices.Clear();
            }

            #endregion

            #region clear service client

            lock (_clients.SyncRoot)
            {
                foreach (CommunicationObject client in _clients)
                {
                    try
                    {
                        client.Close();
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, null);
                    }
                }
                _clients.Clear();
            }

            #endregion
        }

        /// <summary>
        ///     从一个传输通道中，执行连接到远程终结点的操作
        /// </summary>
        /// <param name="channel">传输通道</param>
        /// <returns>返回连接的状态</returns>
        public bool Connect(IRawTransportChannel channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            try
            {
                channel.Connect();
                if (channel.IsConnected) WrapTransportChannel(channel);
                return channel.IsConnected;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return false;
            }
        }

        /// <summary>
        ///     获取一个具有指定ID的传输通道
        /// </summary>
        /// <param name="id">通道唯一标示</param>
        /// <returns>返回传输通道</returns>
        public ITransportChannel GetTransportChannel(Guid id)
        {
            IMessageTransportChannel<T> msgChannel;
            return _transportChannels.TryGetValue(id, out msgChannel) ? msgChannel : null;
        }

        /// <summary>
        ///     注册一个宿主通道
        /// </summary>
        /// <param name="channel">宿主通道</param>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        /// <exception cref="System.Exception">注册失败</exception>
        public void Regist(IHostTransportChannel channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            try
            {
                if (!channel.Regist()) throw new System.Exception("Cannot regist a host channel. #channel: " + channel);
                channel.ChannelCreated += ChannelCreated;
                channel.ChannelDisconnected += ChannelDisconnected;
                lock (_lockHostsObject)
                {
                    _hostChannels.Add(channel.Id, channel);
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
        }

        /// <summary>
        ///     注销一个宿主通道
        /// </summary>
        /// <param name="id">宿主通道唯一标示</param>
        public void UnRegist(Guid id)
        {
            lock (_hostChannels)
            {
                IHostTransportChannel hostChannel;
                if (!_hostChannels.TryGetValue(id, out hostChannel)) return;
                hostChannel.ChannelCreated -= ChannelCreated;
                hostChannel.ChannelDisconnected -= ChannelDisconnected;
                hostChannel.UnRegist();
                _hostChannels.Remove(id);
            }
        }

        /// <summary>
        ///     发送元数据到具有指定通道编号的远程节点上
        /// </summary>
        /// <param name="id">传输通道编号</param>
        /// <param name="data">元数据</param>
        /// <exception cref="TransportChannelNotFoundException">传输通道不存在</exception>
        public void Send(Guid id, byte[] data)
        {
            ITransportChannel channel = GetTransportChannel(id);
            if (channel == null) return;
            if (channel.IsConnected) channel.Send(data);
        }

        /// <summary>
        ///     发送一个消息到具有指定通道编号的远程节点上
        /// </summary>
        /// <param name="id">传输通道编号</param>
        /// <param name="message">元数据</param>
        /// <exception cref="TransportChannelNotFoundException">传输通道不存在</exception>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        public void Send(Guid id, T message)
        {
            if (Comparer.Default.Compare(message, null) != 1) 
                throw new ArgumentNullException("message");
            IMessageTransportChannel<T> msgChannel;
            if(!_transportChannels.TryGetValue(id, out msgChannel))
                throw new TransportChannelNotFoundException();
            msgChannel.Send(message);
        }

        /// <summary>
        ///     被拒绝的链接事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<IAccessRule>> ConnectedButNotAllow;
        protected void ConnectedButNotAllowHandler(LightSingleArgEventArgs<IAccessRule> e)
        {
            EventHandler<LightSingleArgEventArgs<IAccessRule>> handler = ConnectedButNotAllow;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     创建新的传输通道事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<IRawTransportChannel>> NewTransportChannelCreated;
        protected void NewTransportChannelCreatedHandler(LightSingleArgEventArgs<IRawTransportChannel> e)
        {
            EventHandler<LightSingleArgEventArgs<IRawTransportChannel>> handler = NewTransportChannelCreated;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     接收到新消息事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<ReceivedMessageObject<T>>> NewMessageReceived;
        protected void NewMessageReceivedHandler(LightSingleArgEventArgs<ReceivedMessageObject<T>> e)
        {
            EventHandler<LightSingleArgEventArgs<ReceivedMessageObject<T>>> handler = NewMessageReceived;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     传输通道被移除事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<Guid>> TransportChannelRemoved;
        protected void TransportChannelRemovedHandler(LightSingleArgEventArgs<Guid> e)
        {
            EventHandler<LightSingleArgEventArgs<Guid>> handler = TransportChannelRemoved;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Events

        //host channel accept a connector
        void ChannelCreated(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {
            //across the assessor layer.
            if (_accessor != null)
            {
                //get the rule of access.
                IAccessRule rule = _accessor.GetAccessRule(e.Target);
                //this mean is : if rule is null, then acceess it, or will not.
                if (rule != null && !rule.Accessed)
                {
                    e.Target.Disconnect();
                    ConnectedButNotAllowHandler(new LightSingleArgEventArgs<IAccessRule>(rule));
                    return;
                }
            }
            IMessageTransportChannel<T> msgChannel = new MessageTransportChannel<T>((IRawTransportChannel) e.Target, _protocolStack);
            msgChannel.ReceivedMessage += MsgChannelReceivedMessage;
            msgChannel.Disconnected += MsgChannelDisconnected;
            _transportChannels.TryAdd(e.Target.Key, msgChannel);
            NewTransportChannelCreatedHandler(new LightSingleArgEventArgs<IRawTransportChannel>((IRawTransportChannel) e.Target));
        }

        //message channel disconnected.
        void MsgChannelDisconnected(object sender, System.EventArgs e)
        {
            IMessageTransportChannel<T> msgChannel = (IMessageTransportChannel<T>)sender;
            msgChannel.ReceivedMessage -= MsgChannelReceivedMessage;
            msgChannel.Disconnected -= MsgChannelDisconnected;
        }

        //a channel has been disconnected.
        void ChannelDisconnected(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {
            ITransportChannel channel = e.Target;
            TransportChannelRemovedHandler(new LightSingleArgEventArgs<Guid>(channel.Key));
        }

        //a message transport channel has been receive a message.
        void MsgChannelReceivedMessage(object sender, LightSingleArgEventArgs<List<T>> e)
        {
            if (e.Target == null) return;
            IMessageTransportChannel<T> msgChannel = (IMessageTransportChannel<T>) sender;
            List<T> messages = e.Target;
            //try to parse it.
            foreach (T message in messages)
            {
                //parse successed.
                NewMessageReceivedHandler(
                    new LightSingleArgEventArgs<ReceivedMessageObject<T>>(new ReceivedMessageObject<T>
                    {
                        NodeId = _id,
                        Channel = msgChannel,
                        Message = message
                    }));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     注册并包装一个传输信道
        /// </summary>
        /// <param name="channel">传输信道</param>
        private void WrapTransportChannel(IRawTransportChannel channel)
        {
            IMessageTransportChannel<T> msgChannel = new MessageTransportChannel<T>(channel, _protocolStack);
            msgChannel.ReceivedMessage += MsgChannelReceivedMessage;
            msgChannel.Disconnected += MsgChannelDisconnected;
            _transportChannels.TryAdd(channel.Key, msgChannel);
            NewTransportChannelCreatedHandler(new LightSingleArgEventArgs<IRawTransportChannel>(channel));
        }

        /// <summary>
        ///     创建一个网络节点
        ///     <para>* 使用此方法，将会更加智能的为不同的应用场景来构造网络节点</para>
        /// </summary>
        /// <param name="runtimeType">运行时环境</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="processors">自定义的傀儡功能处理器</param>
        /// <returns>返回创建后的网络节点</returns>
        public static INetworkNode<T> Create(RuntimeTypes runtimeType, IProtocolStack<T> protocolStack, params IPuppetFunctionProcessor[] processors)
        {
            //just care 2 scenarios now. :)
            switch (runtimeType)
            {
                //at the same time, initializing all of operations at default.
                case RuntimeTypes.LocalAlone:
                    PuppetNetworkNode<T> puppetNode = new PuppetNetworkNode<T> { Accessor = new PuppetAccessor() }
                        .Attach(new ConnectPuppetFunctionProcessor<T>())
                        .Attach(new OpenPuppetFunctionProcessor<T>())
                        .Attach(new ClosePuppetFunctionProcessor<T>())
                        .Attach(new BroadcastDataPuppetFunctionProcessor<T>())
                        .Attach(new BroadcastMessagePuppetFunctionProcessor<T>())
                        .Attach(new GetTransportChannelPuppetFunctionProcessor<T>())
                        .Attach(new RegistPuppetFunctionProcessor<T>())
                        .Attach(new RegistServicePuppetFunctionProcessor<T>())
                        .Attach(new SendPuppetFunctionProcessor<T>())
                        .Attach(new UnRegistPuppetFunctionProcessor<T>())
                        .Attach(new UnRegistServicePuppetFunctionProcessor<T>());
                    //attach customer puppet function processor(s).
                    if (processors != null && processors.Length > 0)
                    {
                        foreach (var functionProcessor in processors)
                        {
                            puppetNode.Attach(functionProcessor);
                        }
                    }
                    return puppetNode;
                default:
                    return new NetworkNode<T>(protocolStack);
            }
        }

        #endregion
    }
}