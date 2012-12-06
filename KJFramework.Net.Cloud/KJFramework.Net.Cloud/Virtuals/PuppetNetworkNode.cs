using System;
using KJFramework.EventArgs;
using KJFramework.Net.Cloud.Accessors;
using KJFramework.Net.Cloud.Accessors.Rules;
using KJFramework.Net.Cloud.Exceptions;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Objects;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Net.Cloud.Virtuals.Processors;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.ServiceModel.Elements;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.Net.Cloud.Virtuals
{
    /// <summary>
    ///     傀儡网络节点，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public class PuppetNetworkNode<T> : INetworkNode<T>, IPuppetBehavior<T>
    {
        #region Constructor

        /// <summary>
        ///     傀儡网络节点，提供了相关的基本操作
        /// </summary>
        public PuppetNetworkNode()
        {
            _id = Guid.NewGuid();
        }

        #endregion

        #region Members

        internal delegate void DelegateBroadcastData(byte[] data);
        internal delegate void DelegateBroadcastMessage(T message);
        internal delegate void DelegateOpen();
        internal delegate void DelegateClose();
        internal delegate bool DelegateConnect(ITransportChannel channel);
        internal delegate IRawTransportChannel DelegateGetTransportChannel(Guid id);
        internal delegate void DelegateRegist(IHostTransportChannel channel);
        internal delegate void DelegateUnRegist(Guid id);
        internal delegate void DelegateUnRegistService(Uri uri);
        internal delegate void DelegateSend(Guid id, byte[] data);
        internal delegate void DelegateSendMessage(Guid id, T message);
        internal delegate void DelegateRegistService(Binding binding, Type type);
        internal DelegateBroadcastData DBroadcastData;
        internal DelegateBroadcastMessage DBroadcastMessage;
        internal DelegateOpen DOpen;
        internal DelegateOpen DClose;
        internal DelegateConnect DConnect;
        internal DelegateGetTransportChannel DGetTransportChannel;
        internal DelegateRegist DRegist;
        internal DelegateUnRegist DUnRegist;
        internal DelegateSend DSend;
        internal DelegateSendMessage DSendMessage;
        internal DelegateRegistService DRegistService;
        internal DelegateUnRegistService DUnRegistService;
        private IRequestScheduler<T> _puppetScheduler;

        #endregion

        #region Implementation of IDisposable

        protected IAccessor _accessor;
        protected Guid _id;
        protected IProtocolStack<T> _protocolStack;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of INetServiceNode

        /// <summary>
        ///     连接到一个远程的服务节点
        /// </summary>
        /// <param name="binding">服务地址绑定对象</param>
        /// <returns>返回连接后的客户端代理</returns>
        public T1 Connect<T1>(Binding binding) where T1 : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     获取远程服务代理
        /// </summary>
        /// <typeparam name="T">远程服务契约类型</typeparam>
        /// <param name="uri">远程服务地址</param>
        /// <returns>返回客户端代理</returns>
        public T1 GetService<T1>(Uri uri) where T1 : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     注册一个远程服务
        /// </summary>
        /// <param name="binding">绑定方式</param>
        /// <param name="type">契约类型</param>
        public void Regist(Binding binding, Type type)
        {
            CheckNull(DRegistService);
            DRegistService(binding, type);
        }

        /// <summary>
        ///     注销一个远程服务
        /// </summary>
        /// <param name="uri">调用地址</param>
        public void UnRegist(Uri uri)
        {
            CheckNull(DUnRegistService);
            DUnRegistService(uri);
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
            set { _protocolStack = value; }
        }

        /// <summary>
        ///     将指定元数据广播到所有当前所有的信道上。
        /// </summary>
        /// <param name="data">需要广播的元数据</param>
        public void Broadcast(byte[] data)
        {
            CheckNull(DBroadcastData);
            DBroadcastData(data);
        }

        /// <summary>
        ///   将指定消息广播到所有当前所有的信道上。 
        /// </summary>
        /// <param name="message">需要广播的消息</param>
        public void Broadcast(T message)
        {
            CheckNull(DBroadcastMessage);
            DBroadcastMessage(message);
        }

        /// <summary>
        ///     开启当前网络节点
        /// </summary>
        public void Open()
        {
            CheckNull(DOpen);
            DOpen();
        }

        /// <summary>
        ///     关闭当前网络节点
        /// </summary>
        public void Close()
        {
            CheckNull(DClose);
            DClose();
        }

        /// <summary>
        ///     从一个传输通道中，执行连接到远程终结点的操作
        /// </summary>
        /// <param name="channel">传输通道</param>
        /// <returns>返回连接的状态</returns>
        public bool Connect(IRawTransportChannel channel)
        {
            CheckNull(DConnect);
            return DConnect(channel);
        }

        /// <summary>
        ///     获取一个具有指定ID的传输通道
        /// </summary>
        /// <param name="id">通道唯一标示</param>
        /// <returns>返回传输通道</returns>
        public ITransportChannel GetTransportChannel(Guid id)
        {
            CheckNull(DGetTransportChannel);
            return DGetTransportChannel(id);
        }

        /// <summary>
        ///     注册一个宿主通道
        /// </summary>
        /// <param name="channel">宿主通道</param>
        public void Regist(IHostTransportChannel channel)
        {
            CheckNull(DRegist);
            DRegist(channel);
        }

        /// <summary>
        ///     注销一个宿主通道
        /// </summary>
        /// <param name="id">宿主通道唯一标示</param>
        public void UnRegist(Guid id)
        {
            CheckNull(DUnRegist);
            DUnRegist(id);
        }

        /// <summary>
        ///     发送元数据到具有指定通道编号的远程节点上
        /// </summary>
        /// <param name="id">传输通道编号</param>
        /// <param name="data">元数据</param>
        /// <exception cref="TransportChannelNotFoundException">传输通道不存在</exception>
        public void Send(Guid id, byte[] data)
        {
            CheckNull(DSend);
            DSend(id, data);
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
            CheckNull(DSendMessage);
            DSendMessage(id, message);
        }

        /// <summary>
        ///     被拒绝的链接事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<IAccessRule>> ConnectedButNotAllow;
        internal void ConnectedButNotAllowHandler(LightSingleArgEventArgs<IAccessRule> e)
        {
            EventHandler<LightSingleArgEventArgs<IAccessRule>> handler = ConnectedButNotAllow;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     创建新的传输通道事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<IRawTransportChannel>> NewTransportChannelCreated;
        internal void NewTransportChannelCreatedHandler(LightSingleArgEventArgs<IRawTransportChannel> e)
        {
            EventHandler<LightSingleArgEventArgs<IRawTransportChannel>> handler = NewTransportChannelCreated;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     接收到新消息事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<ReceivedMessageObject<T>>> NewMessageReceived;
        internal void NewMessageReceivedHandler(LightSingleArgEventArgs<ReceivedMessageObject<T>> e)
        {
            EventHandler<LightSingleArgEventArgs<ReceivedMessageObject<T>>> handler = NewMessageReceived;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     传输通道被移除事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<Guid>> TransportChannelRemoved;
        internal void TransportChannelRemovedHandler(LightSingleArgEventArgs<Guid> e)
        {
            EventHandler<LightSingleArgEventArgs<Guid>> handler = TransportChannelRemoved;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Implementation of IPuppetBehavior

        /// <summary>
        ///     附加一个傀儡功能处理器
        /// </summary>
        /// <param name="processor">傀儡功能处理器</param>
        /// <returns>返回自身</returns>
        public PuppetNetworkNode<T> Attach(IPuppetFunctionProcessor processor)
        {
            if (processor == null) throw new ArgumentNullException("processor");
            if (!processor.Initialize(this))
            {
                string errorMsg = "Puppet function processor initialize failed. #" + processor;
                AttachFailedHandler(new LightSingleArgEventArgs<string>(errorMsg));
                throw new System.Exception(errorMsg);
            }
            AttachSuccessedHandler(null);
            return this;
        }

        /// <summary>
        ///     创建一个傀儡任务调度器
        /// </summary>
        /// <param name="taskCount">初始化的任务数量</param>
        /// <returns>返回创建后的傀儡任务调度器</returns>
        public IRequestScheduler<T> CreateScheduler(int taskCount = 100)
        {
            //create a request scheduler to contians a puppet network node :)
            //for function node, will be add by customer later.
            if (_puppetScheduler == null)
            {
                _puppetScheduler = new RequestScheduler<T>(taskCount);
                _puppetScheduler.Regist(this);
            }
            return _puppetScheduler;
        }

        /// <summary>
        ///     附加成功事件
        /// </summary>
        public event EventHandler AttachSuccessed;
        protected void AttachSuccessedHandler(System.EventArgs e)
        {
            EventHandler handler = AttachSuccessed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     附加失败事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<string>> AttachFailed;
        protected void AttachFailedHandler(LightSingleArgEventArgs<string> e)
        {
            EventHandler<LightSingleArgEventArgs<string>> handler = AttachFailed;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     检查指定功能是否被实现
        /// </summary>
        /// <param name="targetFunction">指定功能</param>
        private void CheckNull(System.Delegate targetFunction)
        {
            if (targetFunction == null)
            {
                throw new NotImplementedException(string.Intern("Current function does not be implement yet!"));
            }
        }

        #endregion
    }
}