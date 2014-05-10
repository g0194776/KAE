﻿using System;
using System.Collections.Generic;
using System.Configuration;
using KJFramework.ApplicationEngine.Configurations.Loaders;
using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Net.Channels;
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
        private IRemoteDataSyncProxy _syncProxy;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(SystemWorker));
        private MessageTransactionManager _transactionManager;
        private MetadataTransactionManager _metadataTransactionManager;
        private readonly Dictionary<string, IProtocolStack<BaseMessage>> _protocolStacks = new Dictionary<string, IProtocolStack<BaseMessage>>();
        private static readonly MetadataProtocolStack _metadataProtocolStack = new MetadataProtocolStack();

        /// <summary>
        ///     获取远程数据同步代理器
        /// </summary>
        public IRemoteDataSyncProxy DataSyncProxy
        {
            get
            {
                if (_syncProxy == null) throw new System.Exception("You *MUST* call SystemWorker.Instance.Initialize() first!!!");
                return _syncProxy;
            }
        }

        /// <summary>
        ///     系统工作者，提供了相关的基本操作
        /// </summary>
        public static readonly SystemWorker Instance = new SystemWorker();

        #endregion

        #region Implementation of ISystemWorker

        private IRemoteConfigurationProxy _configurationProxy;

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
        ///     初始化属于指定服务角色的系统工作者
        ///     <para>* 当使用此方法，并传递useRemoteConfig=true时，此方法内部将使用默认的远程配置设置</para>
        /// </summary>
        /// <param name="role">服务角色</param>
        /// <param name="useRemoteConfig">是否使用远程配置的标示</param>
        /// <param name="notificationHandler">异常通知处理器</param>
        /// <param name="csnAddress">远程CSN的IP地址</param>
        public void Initialize(string role, bool useRemoteConfig = false, ITracingNotificationHandler notificationHandler = null, string csnAddress = null)
        {
            Initialize(role, useRemoteConfig, RemoteConfigurationSetting.Default, notificationHandler, csnAddress ?? ConfigurationManager.AppSettings["CSN"]);
        }

        /// <summary>
        ///     初始化属于指定服务角色的系统工作者
        /// </summary>
        /// <param name="role">服务角色</param>
        /// <param name="useRemoteConfig">是否使用远程配置的标示</param>
        /// <param name="setting">远程配置设置</param>
        /// <param name="notificationHandler">异常通知处理器</param>
        /// <param name="csnAddress">远程CSN的IP地址</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public void Initialize(string role, bool useRemoteConfig, RemoteConfigurationSetting setting, ITracingNotificationHandler notificationHandler = null, string csnAddress = null)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            if (useRemoteConfig && setting == null) throw new ArgumentNullException("setting");
            if (IsInitialized) return;
            _configurationProxy = (!useRemoteConfig ? (IRemoteConfigurationProxy)new RemoteConfigurationProxy(csnAddress) : new SolitaryRemoteConfigurationProxy(csnAddress));
            //Regist("LGS", new LGSProtocolStack());
            TracingManager.NotificationHandler = notificationHandler ?? new RemoteLogProxy();
            Role = role;
            FixedTypeManager.Add(typeof(MessageIdentity), 5);
            IntellectTypeProcessorMapping.Instance.Regist(new MessageIdentityProcessor());
            IntellectTypeProcessorMapping.Instance.Regist(new TransactionIdentityProcessor());
            ExtensionTypeMapping.Regist(typeof(MessageIdentityValueStored));
            ExtensionTypeMapping.Regist(typeof(TransactionIdentityValueStored));
            //config remote configuration loader.
            if (useRemoteConfig)
                KJFramework.Configurations.Configurations.RemoteConfigLoader = new RemoteConfigurationLoader(setting);
            //initialize long...long memory buffer for tcp layer.
            ChannelConst.Initialize();
            CommonCounter.Instance.Initialize();
            _transactionManager = new MessageTransactionManager(new TransactionIdentityComparer());
            _metadataTransactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());
            _syncProxy = new RemoteDataSyncProxy();
            _isInitialized = true;
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