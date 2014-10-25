using KJFramework.ApplicationEngine.Configurations.Loaders;
using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Configurations;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Clusters;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Helpers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///     系统工作者，提供了相关的基本操作
    /// </summary>
    public sealed class SystemWorker
    {
        #region Constructor

        /// <summary>
        ///     系统工作者，提供了相关的基本操作
        /// </summary>
        private SystemWorker() { }

        #endregion

        #region Members

        private static bool _isInitialized;
        private static INetworkCluster<BaseMessage> _clsuter;
        private static INetworkCluster<MetadataContainer> _metadataCluster;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(SystemWorker));
        private MessageTransactionManager _transactionManager;
        private MetadataTransactionManager _metadataTransactionManager;
        private readonly Dictionary<string, IProtocolStack<BaseMessage>> _protocolStacks = new Dictionary<string, IProtocolStack<BaseMessage>>();
        private static readonly MetadataProtocolStack _metadataProtocolStack = new MetadataProtocolStack();

        /// <summary>
        ///     系统工作者，提供了相关的基本操作
        /// </summary>
        public static readonly SystemWorker Instance = new SystemWorker();

        #endregion

        #region Implementation of ISystemWorker

        private IRemoteConfigurationProxy _configurationProxy;
        private Func<IDictionary<string, string>, ApplicationLevel> _greyPolicy;

        /// <summary>
        ///     获取配置信息代理器
        /// </summary>
        public IRemoteConfigurationProxy ConfigurationProxy
        {
            get { return _configurationProxy; }
        }
        /// <summary>
        ///     获取宿主初始化时使用的角色
        /// </summary>
        public string Role { get; private set; }
        /// <summary>
        ///     获取当前SystemWorker是否已经初始化成功
        /// </summary>
        public static bool IsInitialized
        {
            get { return _isInitialized; }
        }
        /// <summary>
        ///    获取一个值，该值标示了当前SystemWorker的实例是否服务于指定的某个KPP
        /// </summary>
        public static bool IsInSpecifiedKPP { get; private set; }

        /// <summary>
        ///     初始化属于指定服务角色的系统工作者
        /// </summary>
        /// <param name="role">服务角色</param>
        /// <param name="setting">远程配置设置</param>
        /// <param name="configurationProxy">远程配置站访问代理器</param>
        /// <param name="notificationHandler">异常通知处理器</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public void Initialize(string role, RemoteConfigurationSetting setting, IRemoteConfigurationProxy configurationProxy, ITracingNotificationHandler notificationHandler = null)
        {
            if (IsInitialized) return;
            if (setting == null) setting = RemoteConfigurationSetting.Default;
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            if (configurationProxy == null) throw new ArgumentNullException("configurationProxy");
            _configurationProxy = configurationProxy;
            //Regist("LGS", new LGSProtocolStack());
            TracingManager.NotificationHandler = notificationHandler ?? new RemoteLogProxy();
            Role = role;
            FixedTypeManager.Add(typeof(MessageIdentity), 5);
            IntellectTypeProcessorMapping.Instance.Regist(new MessageIdentityProcessor());
            IntellectTypeProcessorMapping.Instance.Regist(new TransactionIdentityProcessor());
            ExtensionTypeMapping.Regist(typeof(MessageIdentityValueStored));
            ExtensionTypeMapping.Regist(typeof(TransactionIdentityValueStored));
            MemoryAllotter.Instance.Initialize();
            //config remote configuration loader.
            KJFramework.Configurations.Configurations.RemoteConfigLoader = new RemoteConfigurationLoader(setting);
            //initialize long...long memory buffer for tcp layer.
            ChannelConst.Initialize();
            _transactionManager = new MessageTransactionManager(new TransactionIdentityComparer());
            _metadataTransactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());
            _isInitialized = true;
            IsInSpecifiedKPP = false;
        }

        /// <summary>
        ///     为KPP专门设计的初始化SystemWorker的函数
        /// </summary>
        /// <param name="role">服务角色</param>
        /// <param name="proxy">KAE宿主代理器</param>
        /// <param name="settings">KJFramework网络层设置集</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        internal void InitializeForKPP(string role, IKAEHostProxy proxy, ChannelInternalConfigSettings settings)
        {
            if (IsInitialized) return;
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            _configurationProxy = new KPPConfigurationProxy(proxy);
            //Regist("LGS", new LGSProtocolStack());
            TracingManager.NotificationHandler = new RemoteLogProxy();
            Role = role;
            FixedTypeManager.Add(typeof(MessageIdentity), 5);
            IntellectTypeProcessorMapping.Instance.Regist(new MessageIdentityProcessor());
            IntellectTypeProcessorMapping.Instance.Regist(new TransactionIdentityProcessor());
            ExtensionTypeMapping.Regist(typeof(MessageIdentityValueStored));
            ExtensionTypeMapping.Regist(typeof(TransactionIdentityValueStored));
            MemoryAllotter.Instance.Initialize();
            //config remote configuration loader.
            KJFramework.Configurations.Configurations.RemoteConfigLoader = new RemoteConfigurationLoader(RemoteConfigurationSetting.Default);
            //initialize long...long memory buffer for tcp layer.
            ChannelConst.Initialize(settings);
            _transactionManager = new MessageTransactionManager(new TransactionIdentityComparer());
            _metadataTransactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());
            _isInitialized = true;
            IsInSpecifiedKPP = true;
        }

        /// <summary>
        ///    为SystemWorker注入已更新的灰度升级策略
        /// </summary>
        /// <param name="callback">回调方法</param>
        internal void UpdateGreyPolicy(Func<IDictionary<string,string>, ApplicationLevel> callback)
        {
            _greyPolicy = callback;
        }

        /// <summary>
        ///     注册指定服务角色的协议栈
        /// </summary>
        /// <param name="role">服务角色</param>
        /// <param name="protocolStack">协议栈</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public void Regist(string role, IProtocolStack<BaseMessage> protocolStack)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            _protocolStacks[role] = protocolStack;
        }

        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <param name="role">针对的服务角色</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public MetadataMessageTransaction CreateMetadataTransaction(string role)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            string errMsg;
            IServerConnectionAgent<MetadataContainer> agent = _metadataCluster.GetChannel(role, _metadataProtocolStack, out errMsg);
            return agent == null
                       ? new FailMetadataTransaction(errMsg)
                       : _metadataTransactionManager.Create(IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP), agent.GetChannel());
        }

        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <param name="role">针对的服务角色</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public BusinessMessageTransaction CreateTransaction(string role)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            string errMsg;
            IServerConnectionAgent<BaseMessage> agent = _clsuter.GetChannel(role, _protocolStacks[role], out errMsg);
            return agent == null
                       ? new FailMessageTransaction(errMsg)
                       : _transactionManager.Create(IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP), agent.GetChannel());
        }

        /// <summary>
        ///     创建一个新的事务
        ///     <para>* 按照固定手机号的负载策略</para>
        /// </summary>
        /// <param name="role">针对的服务角色</param>
        /// <param name="mobileNo">手机号</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public BusinessMessageTransaction CreateTransaction(string role, long mobileNo)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            string errMsg;
            IServerConnectionAgent<BaseMessage> agent = _clsuter.GetChannelBySpecificCondition(role, _protocolStacks[role], mobileNo, out errMsg);
            return agent == null
                       ? new FailMessageTransaction(errMsg)
                       : _transactionManager.Create(IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP), agent.GetChannel());
        }

        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <param name="targetRole">针对的服务角色</param>
        /// <param name="protocolSelf">使用的协议栈</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public BusinessMessageTransaction CreateTransaction(string targetRole, string protocolSelf)
        {
            if (string.IsNullOrEmpty(targetRole)) throw new ArgumentNullException("targetRole");
            if (string.IsNullOrEmpty(protocolSelf)) throw new ArgumentNullException("protocolSelf");
            string errMsg;
            IServerConnectionAgent<BaseMessage> agent = _clsuter.GetChannel(targetRole, _protocolStacks[protocolSelf], out errMsg);
            return agent == null
                       ? new FailMessageTransaction(errMsg)
                       : _transactionManager.Create(IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP), agent.GetChannel());
        }

        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <param name="targetRole">针对的服务角色</param>
        /// <param name="protocolSelf">使用的协议栈</param>
        /// <param name="userId">需要定位的用户编号</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public BusinessMessageTransaction CreateTransaction(string targetRole, string protocolSelf, int userId)
        {
            if (string.IsNullOrEmpty(targetRole)) throw new ArgumentNullException("targetRole");
            if (string.IsNullOrEmpty(protocolSelf)) throw new ArgumentNullException("protocolSelf");
            string errMsg;
            IServerConnectionAgent<BaseMessage> agent = _clsuter.GetChannelBySpecificCondition(targetRole, _protocolStacks[protocolSelf], userId, out errMsg);
            return agent == null
                       ? new FailMessageTransaction(errMsg)
                       : _transactionManager.Create(IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP), agent.GetChannel());
        }

        /// <summary>
        ///     创建一个新的事务
        ///     <para>* 按照固定手机号的负载策略</para>
        /// </summary>
        /// <param name="targetRole">针对的服务角色</param>
        /// <param name="protocolSelf">使用的协议栈</param>
        /// <param name="mobileNo">手机号</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public BusinessMessageTransaction CreateTransaction(string targetRole, string protocolSelf, long mobileNo)
        {
            if (string.IsNullOrEmpty(targetRole)) throw new ArgumentNullException("targetRole");
            if (string.IsNullOrEmpty(protocolSelf)) throw new ArgumentNullException("protocolSelf");
            string errMsg;
            IServerConnectionAgent<BaseMessage> agent = _clsuter.GetChannelBySpecificCondition(targetRole, _protocolStacks[protocolSelf], mobileNo, out errMsg);
            return agent == null
                       ? new FailMessageTransaction(errMsg)
                       : _transactionManager.Create(IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP), agent.GetChannel());
        }

        /// <summary>
        ///     创建一个新的单向通信事务
        ///     <para>* 使用此函数创建的事务是单向通信的，也就意味着新的事务不会触发RecvRsp事件</para>
        /// </summary>
        /// <param name="role">针对的服务角色</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public BusinessMessageTransaction CreateOnewayTransaction(string role)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            string errMsg;
            IServerConnectionAgent<BaseMessage> agent = _clsuter.GetChannel(role, _protocolStacks[role], out errMsg);
            return agent == null
                       ? new FailMessageTransaction(errMsg)
                       : new BusinessMessageTransaction(new Lease(DateTime.MaxValue), agent.GetChannel()) { Identity = IdentityHelper.CreateOneway(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP) };
        }

        /// <summary>
        ///     创建一个新的单向通信事务
        ///     <para>* 使用此函数创建的事务是单向通信的，也就意味着新的事务不会触发RecvRsp事件</para>
        /// </summary>
        /// <param name="role">针对的服务角色</param>
        /// <param name="userId">需要定位的用户编号</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public BusinessMessageTransaction CreateOnewayTransaction(string role, int userId)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            string errMsg;
            IServerConnectionAgent<BaseMessage> agent = _clsuter.GetChannelBySpecificCondition(role, _protocolStacks[role], userId, out errMsg);
            return agent == null
                       ? new FailMessageTransaction(errMsg)
                       : new BusinessMessageTransaction(new Lease(DateTime.MaxValue), agent.GetChannel()) { Identity = IdentityHelper.CreateOneway(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP) };
        }

        /// <summary>
        ///     创建一个新的单向通信事务
        ///     <para>* 使用此函数创建的事务是单向通信的，也就意味着新的事务不会触发RecvRsp事件</para>
        /// </summary>
        /// <param name="targetRole">针对的服务角色</param>
        /// <param name="protocolSelf">使用的协议栈</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public BusinessMessageTransaction CreateOnewayTransaction(string targetRole, string protocolSelf)
        {
            if (string.IsNullOrEmpty(targetRole)) throw new ArgumentNullException("targetRole");
            if (string.IsNullOrEmpty(protocolSelf)) throw new ArgumentNullException("protocolSelf");
            string errMsg;
            IServerConnectionAgent<BaseMessage> agent = _clsuter.GetChannel(protocolSelf, _protocolStacks[protocolSelf], out errMsg);
            return agent == null
                       ? new FailMessageTransaction(errMsg)
                       : new BusinessMessageTransaction(new Lease(DateTime.MaxValue), agent.GetChannel()) { Identity = IdentityHelper.CreateOneway(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP) };
        }

        /// <summary>
        ///     创建一个新的单向通信事务
        ///     <para>* 使用此函数创建的事务是单向通信的，也就意味着新的事务不会触发RecvRsp事件</para>
        /// </summary>
        /// <param name="targetRole">针对的服务角色</param>
        /// <param name="protocolSelf">使用的协议栈</param>
        /// <param name="userId">需要定位的用户编号</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public BusinessMessageTransaction CreateOnewayTransaction(string targetRole, string protocolSelf, int userId)
        {
            if (string.IsNullOrEmpty(targetRole)) throw new ArgumentNullException("targetRole");
            if (string.IsNullOrEmpty(protocolSelf)) throw new ArgumentNullException("protocolSelf");
            string errMsg;
            IServerConnectionAgent<BaseMessage> agent = _clsuter.GetChannelBySpecificCondition(protocolSelf, _protocolStacks[protocolSelf], userId, out errMsg);
            return agent == null
                       ? new FailMessageTransaction(errMsg)
                       : new BusinessMessageTransaction(new Lease(DateTime.MaxValue), agent.GetChannel()) { Identity = IdentityHelper.CreateOneway(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP) };
        }

        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <param name="role">针对的服务角色</param>
        /// <param name="userId">需要定位的用户编号</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public BusinessMessageTransaction CreateTransaction(string role, int userId)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            string errMsg;
            IServerConnectionAgent<BaseMessage> agent = _clsuter.GetChannelBySpecificCondition(role, _protocolStacks[role], userId, out errMsg);
            return agent == null
                       ? new FailMessageTransaction(errMsg)
                       : _transactionManager.Create(IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP), agent.GetChannel());
        }

        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <param name="role">针对的服务角色</param>
        /// <param name="userId">需要定位的用户编号</param>
        /// <returns>返回新的事务</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public MetadataMessageTransaction CreateMetadataTransaction(string role, int userId)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            string errMsg;
            IServerConnectionAgent<MetadataContainer> agent = _metadataCluster.GetChannelBySpecificCondition(role, _metadataProtocolStack, userId, out errMsg);
            return agent == null
                       ? new FailMetadataTransaction(errMsg)
                       : _metadataTransactionManager.Create(IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP), agent.GetChannel());
        }

        #endregion
    }
}