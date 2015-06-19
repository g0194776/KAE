using KJFramework.ApplicationEngine.Clusters;
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
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Pools;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///     系统工作者，提供了相关的基本操作
    /// </summary>
    public static class SystemWorker
    {
        #region Members

        private static bool _isInitialized;
        private static IKAEHostProxy _hostProxy;
        private static INetworkCluster<BaseMessage> _clsuter;
        private static INetworkCluster<MetadataContainer> _metadataCluster;
        private static IProtocolStackContainer _protocolStackContainer;
        private static MessageTransactionManager _transactionManager;
        private static MetadataTransactionManager _metadataTransactionManager;
        private static ConnectionPool<BaseMessage> _baseMessageConnectionPool;
        private static ConnectionPool<MetadataContainer> _metadataConnectionPool; 
        private static IMessageTransactionProxy<BaseMessage> _baseMessageTransactionProxy; 
        private static IMessageTransactionProxy<MetadataContainer> _metadataMessageTransactionProxy;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(SystemWorker));

        #endregion

        #region Implementation of ISystemWorker

        private static Guid _appUniqueId;
        private static IRemoteConfigurationProxy _configurationProxy;
        private static Func<IDictionary<string, string>, ApplicationLevel> _greyPolicy;

        /// <summary>
        ///     获取配置信息代理器
        /// </summary>
        public static IRemoteConfigurationProxy ConfigurationProxy
        {
            get { return _configurationProxy; }
        }
        /// <summary>
        ///     获取宿主初始化时使用的角色
        /// </summary>
        public static string Role { get; private set; }
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
        public static void Initialize(string role, RemoteConfigurationSetting setting, IRemoteConfigurationProxy configurationProxy, ITracingNotificationHandler notificationHandler = null)
        {
            if (IsInitialized) return;
            if (setting == null) setting = RemoteConfigurationSetting.Default;
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            if (configurationProxy == null) throw new ArgumentNullException("configurationProxy");
            _configurationProxy = configurationProxy;
            //Regist("LGS", new LGSProtocolStack());
            TracingManager.NotificationHandler = notificationHandler ?? new RemoteLogProxy();
            //config remote configuration loader.
            KJFramework.Configurations.Configurations.RemoteConfigLoader = new RemoteConfigurationLoader(setting);
            InitializeCore(role);
            //initialize long...long memory buffer for tcp layer.
            ChannelConst.Initialize();
            _isInitialized = true;
            IsInSpecifiedKPP = false;
        }

        /// <summary>
        ///     为KPP专门设计的初始化SystemWorker的函数
        /// </summary>
        /// <param name="role">服务角色</param>
        /// <param name="proxy">KAE宿主代理器</param>
        /// <param name="settings">KJFramework网络层设置集</param>
        /// <param name="appUniqueId">APP唯一编号</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        internal static void InitializeForKPP(string role, IKAEHostProxy proxy, ChannelInternalConfigSettings settings, Guid appUniqueId)
        {
            if (IsInitialized) return;
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            _hostProxy = proxy;
            _appUniqueId = appUniqueId;
            _configurationProxy = new KPPConfigurationProxy(proxy);
            //Regist("LGS", new LGSProtocolStack());
            TracingManager.NotificationHandler = new RemoteLogProxy();
            //config remote configuration loader.
            KJFramework.Configurations.Configurations.RemoteConfigLoader = new RemoteConfigurationLoader(RemoteConfigurationSetting.Default);
            InitializeCore(role);
            //initialize long...long memory buffer for tcp layer.
            ChannelConst.Initialize(settings);
            _isInitialized = true;
            IsInSpecifiedKPP = true;
        }

        /// <summary>
        ///    初始化内部通用数据
        /// </summary>
        private static void InitializeCore(string role)
        {
            Role = role;
            FixedTypeManager.Add(typeof(MessageIdentity), 5);
            IntellectTypeProcessorMapping.Instance.Regist(new MessageIdentityProcessor());
            IntellectTypeProcessorMapping.Instance.Regist(new TransactionIdentityProcessor());
            ExtensionTypeMapping.Regist(typeof(MessageIdentityValueStored));
            ExtensionTypeMapping.Regist(typeof(TransactionIdentityValueStored));
            MemoryAllotter.Instance.Initialize();
            _transactionManager = new MessageTransactionManager(new TransactionIdentityComparer());
            _metadataTransactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());
            _protocolStackContainer = new ProtocolStackContainer();
            _baseMessageConnectionPool = new IntellectObjectSystemConnectionPool();
            _metadataConnectionPool = new MetadataSystemConnectionPool();
            _clsuter = new NetworkCluster<BaseMessage>(_transactionManager, _baseMessageConnectionPool, ProtocolTypes.Intellegence);
            _metadataCluster = new NetworkCluster<MetadataContainer>(_metadataTransactionManager, _metadataConnectionPool, ProtocolTypes.Metadata);
            _baseMessageTransactionProxy = new BusinessMessageTransactionProxy(_protocolStackContainer, _clsuter, _transactionManager, _hostProxy, _appUniqueId);
            _metadataMessageTransactionProxy = new MetadataMessageTransactionProxy(_protocolStackContainer, _metadataCluster, _metadataTransactionManager, _hostProxy, _appUniqueId);
        }

        /// <summary>
        ///    为SystemWorker注入已更新的灰度升级策略
        /// </summary>
        /// <param name="callback">回调方法</param>
        internal static void UpdateGreyPolicy(Func<IDictionary<string,string>, ApplicationLevel> callback)
        {
            _baseMessageTransactionProxy.UpdateGreyPolicy(callback);
            _metadataMessageTransactionProxy.UpdateGreyPolicy(callback);
        }

        /// <summary>
        ///     注册指定服务角色的协议栈
        /// </summary>
        /// <param name="role">服务角色</param>
        /// <param name="protocolStack">协议栈</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public static void Regist(string role, IProtocolStack protocolStack)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            _protocolStackContainer.Regist(role, protocolStack);
        }

        /// <summary>
        ///    根据一个消息类型来获取指定的事务代理器
        /// </summary>
        /// <typeparam name="TMessage">事务代理器所承载的消息类型</typeparam>
        /// <param name="protocol">消息类型</param>
        /// <returns>返回所对应的事务代理器</returns>
        /// <exception cref="NotImplementedException">目前不支持的协议类型</exception>
        public static IMessageTransactionProxy<TMessage> GetTransactionProxy<TMessage>(ProtocolTypes protocol)
        {
            switch (protocol)
            {
                case ProtocolTypes.Intellegence:
                    return (IMessageTransactionProxy<TMessage>) _baseMessageTransactionProxy;
                case ProtocolTypes.Metadata:
                    return (IMessageTransactionProxy<TMessage>) _metadataMessageTransactionProxy;
                default:
                    throw new NotImplementedException("#Sadly, We have not supported current message type of transacion proxy: " + typeof (TMessage).Name);
            }
        }

        /// <summary>
        ///    更新网络缓存信息
        /// </summary>
        /// <param name="level">应用等级</param>
        /// <param name="cache">远程目标终结点信息列表</param>
        /// <param name="identity">通信协议</param>
        /// <param name="protocolTypes">协议类型</param>
        public static void UpdateCache(MessageIdentity identity, ProtocolTypes protocolTypes, ApplicationLevel level, List<string> cache)
        {
            if (protocolTypes == ProtocolTypes.Metadata) _metadataCluster.UpdateCache(identity, level, cache);
            else if (protocolTypes == ProtocolTypes.Intellegence) _clsuter.UpdateCache(identity, level, cache);
        }

        #endregion
    }
}