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
    ///     ��������ڵ㣬�ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">��Ϣ����</typeparam>
    public class PuppetNetworkNode<T> : INetworkNode<T>, IPuppetBehavior<T>
    {
        #region Constructor

        /// <summary>
        ///     ��������ڵ㣬�ṩ����صĻ�������
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
        ///     ���ӵ�һ��Զ�̵ķ���ڵ�
        /// </summary>
        /// <param name="binding">�����ַ�󶨶���</param>
        /// <returns>�������Ӻ�Ŀͻ��˴���</returns>
        public T1 Connect<T1>(Binding binding) where T1 : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     ��ȡԶ�̷������
        /// </summary>
        /// <typeparam name="T">Զ�̷�����Լ����</typeparam>
        /// <param name="uri">Զ�̷����ַ</param>
        /// <returns>���ؿͻ��˴���</returns>
        public T1 GetService<T1>(Uri uri) where T1 : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     ע��һ��Զ�̷���
        /// </summary>
        /// <param name="binding">�󶨷�ʽ</param>
        /// <param name="type">��Լ����</param>
        public void Regist(Binding binding, Type type)
        {
            CheckNull(DRegistService);
            DRegistService(binding, type);
        }

        /// <summary>
        ///     ע��һ��Զ�̷���
        /// </summary>
        /// <param name="uri">���õ�ַ</param>
        public void UnRegist(Uri uri)
        {
            CheckNull(DUnRegistService);
            DUnRegistService(uri);
        }

        #endregion

        #region Implementation of INetworkNode<T>

        /// <summary>
        ///     ��ȡ�����÷�����
        /// </summary>
        public IAccessor Accessor
        {
            get { return _accessor; }
            set { _accessor = value; }
        }

        /// <summary>
        ///     ��ȡΨһ��ֵ
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     ��ȡ������Э��ջ
        /// </summary>
        public IProtocolStack<T> ProtocolStack
        {
            get { return _protocolStack; }
            set { _protocolStack = value; }
        }

        /// <summary>
        ///     ��ָ��Ԫ���ݹ㲥�����е�ǰ���е��ŵ��ϡ�
        /// </summary>
        /// <param name="data">��Ҫ�㲥��Ԫ����</param>
        public void Broadcast(byte[] data)
        {
            CheckNull(DBroadcastData);
            DBroadcastData(data);
        }

        /// <summary>
        ///   ��ָ����Ϣ�㲥�����е�ǰ���е��ŵ��ϡ� 
        /// </summary>
        /// <param name="message">��Ҫ�㲥����Ϣ</param>
        public void Broadcast(T message)
        {
            CheckNull(DBroadcastMessage);
            DBroadcastMessage(message);
        }

        /// <summary>
        ///     ������ǰ����ڵ�
        /// </summary>
        public void Open()
        {
            CheckNull(DOpen);
            DOpen();
        }

        /// <summary>
        ///     �رյ�ǰ����ڵ�
        /// </summary>
        public void Close()
        {
            CheckNull(DClose);
            DClose();
        }

        /// <summary>
        ///     ��һ������ͨ���У�ִ�����ӵ�Զ���ս��Ĳ���
        /// </summary>
        /// <param name="channel">����ͨ��</param>
        /// <returns>�������ӵ�״̬</returns>
        public bool Connect(IRawTransportChannel channel)
        {
            CheckNull(DConnect);
            return DConnect(channel);
        }

        /// <summary>
        ///     ��ȡһ������ָ��ID�Ĵ���ͨ��
        /// </summary>
        /// <param name="id">ͨ��Ψһ��ʾ</param>
        /// <returns>���ش���ͨ��</returns>
        public ITransportChannel GetTransportChannel(Guid id)
        {
            CheckNull(DGetTransportChannel);
            return DGetTransportChannel(id);
        }

        /// <summary>
        ///     ע��һ������ͨ��
        /// </summary>
        /// <param name="channel">����ͨ��</param>
        public void Regist(IHostTransportChannel channel)
        {
            CheckNull(DRegist);
            DRegist(channel);
        }

        /// <summary>
        ///     ע��һ������ͨ��
        /// </summary>
        /// <param name="id">����ͨ��Ψһ��ʾ</param>
        public void UnRegist(Guid id)
        {
            CheckNull(DUnRegist);
            DUnRegist(id);
        }

        /// <summary>
        ///     ����Ԫ���ݵ�����ָ��ͨ����ŵ�Զ�̽ڵ���
        /// </summary>
        /// <param name="id">����ͨ�����</param>
        /// <param name="data">Ԫ����</param>
        /// <exception cref="TransportChannelNotFoundException">����ͨ��������</exception>
        public void Send(Guid id, byte[] data)
        {
            CheckNull(DSend);
            DSend(id, data);
        }

        /// <summary>
        ///     ����һ����Ϣ������ָ��ͨ����ŵ�Զ�̽ڵ���
        /// </summary>
        /// <param name="id">����ͨ�����</param>
        /// <param name="message">Ԫ����</param>
        /// <exception cref="TransportChannelNotFoundException">����ͨ��������</exception>
        /// <exception cref="System.ArgumentNullException">��������</exception>
        public void Send(Guid id, T message)
        {
            CheckNull(DSendMessage);
            DSendMessage(id, message);
        }

        /// <summary>
        ///     ���ܾ��������¼�
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<IAccessRule>> ConnectedButNotAllow;
        internal void ConnectedButNotAllowHandler(LightSingleArgEventArgs<IAccessRule> e)
        {
            EventHandler<LightSingleArgEventArgs<IAccessRule>> handler = ConnectedButNotAllow;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     �����µĴ���ͨ���¼�
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<IRawTransportChannel>> NewTransportChannelCreated;
        internal void NewTransportChannelCreatedHandler(LightSingleArgEventArgs<IRawTransportChannel> e)
        {
            EventHandler<LightSingleArgEventArgs<IRawTransportChannel>> handler = NewTransportChannelCreated;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     ���յ�����Ϣ�¼�
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<ReceivedMessageObject<T>>> NewMessageReceived;
        internal void NewMessageReceivedHandler(LightSingleArgEventArgs<ReceivedMessageObject<T>> e)
        {
            EventHandler<LightSingleArgEventArgs<ReceivedMessageObject<T>>> handler = NewMessageReceived;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     ����ͨ�����Ƴ��¼�
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
        ///     ����һ�����ܹ��ܴ�����
        /// </summary>
        /// <param name="processor">���ܹ��ܴ�����</param>
        /// <returns>��������</returns>
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
        ///     ����һ���������������
        /// </summary>
        /// <param name="taskCount">��ʼ������������</param>
        /// <returns>���ش�����Ŀ������������</returns>
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
        ///     ���ӳɹ��¼�
        /// </summary>
        public event EventHandler AttachSuccessed;
        protected void AttachSuccessedHandler(System.EventArgs e)
        {
            EventHandler handler = AttachSuccessed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     ����ʧ���¼�
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
        ///     ���ָ�������Ƿ�ʵ��
        /// </summary>
        /// <param name="targetFunction">ָ������</param>
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