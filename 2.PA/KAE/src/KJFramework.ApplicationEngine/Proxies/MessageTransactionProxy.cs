﻿using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Clusters;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.ApplicationEngine.Rings;
using KJFramework.Net;
using KJFramework.Net.Enums;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Enums;
using KJFramework.Net.Transaction.Helpers;
using KJFramework.Net.Transaction.Objects;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///    网络消息事务代理器抽象父类
    /// </summary>
    /// <typeparam name="TMessage">消息事务所承载的消息类型</typeparam>
    internal abstract class MessageTransactionProxy<TMessage> : IMessageTransactionProxy<TMessage>
    {
        #region Constructor.

        /// <summary>
        ///    网络消息事务代理器抽象父类
        /// </summary>
        /// <param name="container">网络协议栈容器</param>
        /// <param name="cluster">网络负载器</param>
        /// <param name="transactionManager">事务管理器</param>
        /// <param name="hostProxy">KAE宿主透明代理</param>
        /// <param name="appUniqueId">应用唯一编号</param>
        /// <exception cref="ArgumentException">参数不能为空</exception>
        protected MessageTransactionProxy(IProtocolStackContainer container, INetworkCluster<TMessage> cluster, ITransactionManager<TMessage> transactionManager, IKAEResourceProxy hostProxy, Guid appUniqueId)
        {
            if(container == null) throw new ArgumentNullException("container");
            if (cluster == null) throw new ArgumentNullException("cluster");
            if (transactionManager == null) throw new ArgumentNullException("transactionManager");
            if (hostProxy == null) throw new ArgumentNullException("hostProxy");
            _container = container;
            _cluster = cluster;
            _transactionManager = transactionManager;
            _hostProxy = hostProxy;
            _appUniqueId = appUniqueId;
        }

        #endregion

        #region Members.

        private readonly Guid _appUniqueId;
        private readonly IKAEResourceProxy _hostProxy;
        private readonly object _lockObj = new object();
        protected readonly IProtocolStackContainer _container;
        protected readonly INetworkCluster<TMessage> _cluster;
        private readonly object _protectiveLockObj = new object();
        protected Func<IDictionary<string, string>, ApplicationLevel> _callback;
        private readonly TimeSpan _protectiveTimeSpan = new TimeSpan(0, 0, 10);
        protected readonly ITransactionManager<TMessage> _transactionManager;
        private readonly IDictionary<string, DateTime> _protectiveCheckingTimeSpan = new Dictionary<string, DateTime>(); 

        #endregion

        #region Methods.
        
        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="appName">目标APP的名称</param>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="version">
        ///     目标APP的版本
        ///     <para>* 默认为: latest</para>
        /// </param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(appName, target, Global.TransactionTimeout, version, resourceUri, communicationType, protocolSelf);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="appName">目标APP的名称</param>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="maximumRspTime">等待当前网络事务RSP消息的最大时间</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="version">
        ///     目标APP的版本
        ///     <para>* 默认为: latest</para>
        /// </param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, TimeSpan maximumRspTime, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            if (string.IsNullOrEmpty(appName)) throw new ArgumentNullException("appName");
            string errMsg;
            ApplicationLevel level = (resourceUri == null ? ApplicationLevel.Stable : _callback(resourceUri));
            KAERingNode ringNode;
            IServerConnectionAgent<TMessage> agent = _cluster.GetChannel(target, level, _container.GetDefaultProtocolStack(_cluster.ProtocolType), out errMsg, out ringNode);
            if (agent == null)
            {
                lock (_lockObj)
                {
                    //try to obtains agent object again for ensuring that the newest remoting addresses can be appliy in the multiple threading env.
                    agent = _cluster.GetChannel(target, level, _container.GetDefaultProtocolStack(_cluster.ProtocolType), out errMsg, out ringNode);
                    if (agent == null && !GetMissedRemoteAddresses(appName, version, target, level)) return new FailMessageTransaction<TMessage>(errMsg);
                    agent = _cluster.GetChannel(target, level, _container.GetDefaultProtocolStack(_cluster.ProtocolType), out errMsg, out ringNode);
                    //check returned value again for avoiding couldnt connect to the remote address now.
                    if (agent == null) return new FailMessageTransaction<TMessage>(errMsg);
                }
            }
            MessageTransaction<TMessage> transaction = NewTransaction(new Lease(DateTime.Now.Add(maximumRspTime)), agent.GetChannel());
            transaction.SetMessageIdentity(new MessageIdentity { ProtocolId = (byte)target.ProtocolId, ServiceId = (byte)target.ServiceId, DetailsId = (byte)target.DetailsId });
            transaction.TransactionManager = _transactionManager;
            transaction.Identity = (communicationType == NetworkCommunicationTypes.Dulplex ? IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP) : IdentityHelper.CreateOneway(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP));
            transaction.KPPUniqueId = ringNode.KPPUniqueId;
            return (_transactionManager.Add(transaction.Identity, transaction) ? transaction : null);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="appName">目标APP的名称</param>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="version">
        ///     目标APP的版本
        ///     <para>* 默认为: latest</para>
        /// </param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, long balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(appName, target, Global.TransactionTimeout, balanceFlag, version, resourceUri, communicationType, protocolSelf);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="appName">目标APP的名称</param>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="maximumRspTime">等待当前网络事务RSP消息的最大时间</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="version">
        ///     目标APP的版本
        ///     <para>* 默认为: latest</para>
        /// </param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, TimeSpan maximumRspTime, long balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            if(string.IsNullOrEmpty(appName)) throw new ArgumentNullException("appName");
            string errMsg;
            ApplicationLevel level = (resourceUri == null ? ApplicationLevel.Stable : _callback(resourceUri));
            KAERingNode ringNode;
            IServerConnectionAgent<TMessage> agent = _cluster.GetChannel(target, level, _container.GetDefaultProtocolStack(_cluster.ProtocolType), balanceFlag, out errMsg, out ringNode);
            if (agent == null)
            {
                lock (_lockObj)
                {
                    //try to obtains agent object again for ensuring that the newest remoting addresses can be appliy in the multiple threading env.
                    agent = _cluster.GetChannel(target, level, _container.GetDefaultProtocolStack(_cluster.ProtocolType), balanceFlag, out errMsg, out ringNode);
                    if (agent == null && !GetMissedRemoteAddresses(appName, version, target, level)) return new FailMessageTransaction<TMessage>(errMsg);
                    agent = _cluster.GetChannel(target, level, _container.GetDefaultProtocolStack(_cluster.ProtocolType), balanceFlag, out errMsg, out ringNode);
                    //check returned value again for avoiding couldnt connect to the remote address now.
                    if (agent == null) return new FailMessageTransaction<TMessage>(errMsg);
                }
            }
            MessageTransaction<TMessage> transaction = NewTransaction(new Lease(DateTime.Now.Add(maximumRspTime)), agent.GetChannel());
            transaction.SetMessageIdentity(new MessageIdentity { ProtocolId = (byte)target.ProtocolId, ServiceId = (byte)target.ServiceId, DetailsId = (byte)target.DetailsId });
            transaction.TransactionManager = _transactionManager;
            transaction.Identity = (communicationType == NetworkCommunicationTypes.Dulplex ? IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP) : IdentityHelper.CreateOneway(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP));
            transaction.KPPUniqueId = ringNode.KPPUniqueId;
            return (_transactionManager.Add(transaction.Identity, transaction) ? transaction : null);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="appName">目标APP的名称</param>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="version">
        ///     目标APP的版本
        ///     <para>* 默认为: latest</para>
        /// </param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, string balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(appName, target, Global.TransactionTimeout, balanceFlag.GetHashCode(), version, resourceUri, communicationType, protocolSelf);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="appName">目标APP的名称</param>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="maximumRspTime">等待当前网络事务RSP消息的最大时间</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="version">
        ///     目标APP的版本
        ///     <para>* 默认为: latest</para>
        /// </param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, TimeSpan maximumRspTime, string balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(appName, target, maximumRspTime, balanceFlag.GetHashCode(), version, resourceUri, communicationType, protocolSelf);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="appName">目标APP的名称</param>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="version">
        ///     目标APP的版本
        ///     <para>* 默认为: latest</para>
        /// </param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, Guid balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(appName, target, Global.TransactionTimeout, balanceFlag.GetHashCode(), version, resourceUri, communicationType, protocolSelf);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="appName">目标APP的名称</param>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="maximumRspTime">等待当前网络事务RSP消息的最大时间</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="version">
        ///     目标APP的版本
        ///     <para>* 默认为: latest</para>
        /// </param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, TimeSpan maximumRspTime, Guid balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(appName, target, maximumRspTime, balanceFlag.GetHashCode(), version, resourceUri, communicationType, protocolSelf);
        }

        /// <summary>
        ///    为MessageTransactionProxy注入已更新的灰度升级策略
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void UpdateGreyPolicy(Func<IDictionary<string, string>, ApplicationLevel> callback)
        {
            _callback = callback;
        }

        private bool GetMissedRemoteAddresses(string appName, string version, Protocols protocol, ApplicationLevel level)
        {
            if (CheckProtect(appName, version, protocol, level, _cluster.ProtocolType)) return false;
            if (_hostProxy == null) return false;
            IList<string> addresses = _hostProxy.GetRemoteAddresses(appName, version, _appUniqueId, protocol, _cluster.ProtocolType, level);
            if (addresses == null || addresses.Count == 0) return false;
            _cluster.UpdateCache(protocol, level, addresses);
            return true;
        }

        //avoids for obtaining non-existed remote address frequently under big overloads.
        //return true when the targeted condition had reached.
        private bool CheckProtect(string appName, string version, Protocols protocol, ApplicationLevel level, ProtocolTypes protocolType)
        {
            lock (_protectiveLockObj)
            {
                string key = string.Format("{0}-{1}-{2}_{3}_{4}_{5}_{6}", appName, version, protocol.ProtocolId, protocol.ServiceId, protocol.DetailsId, level, protocolType);
                DateTime time;
                if (!_protectiveCheckingTimeSpan.TryGetValue(key, out time))
                {
                    _protectiveCheckingTimeSpan[key] = DateTime.Now;
                    return false;
                }
                if ((DateTime.Now - time) <= _protectiveTimeSpan) return true;
                _protectiveCheckingTimeSpan[key] = DateTime.Now;
                return false;
            }
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="lease">网络事务的生命周期</param>
        /// <param name="channel">网络事务内部包含的通信信道</param>
        /// <returns>返回一个新的网络事务</returns>
        protected abstract MessageTransaction<TMessage> NewTransaction(Lease lease, IMessageTransportChannel<TMessage> channel);

        #endregion
    }
}