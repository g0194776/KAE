using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Clusters;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;
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
        protected MessageTransactionProxy(IProtocolStackContainer container, INetworkCluster<TMessage> cluster, ITransactionManager<TMessage> transactionManager)
        {
            _container = container;
            _cluster = cluster;
            _transactionManager = transactionManager;
        }

        #endregion

        #region Members.

        protected readonly IProtocolStackContainer _container;
        protected readonly INetworkCluster<TMessage> _cluster;
        protected Func<IDictionary<string, string>, ApplicationLevel> _callback;
        protected readonly ITransactionManager<TMessage> _transactionManager;

        #endregion

        #region Methods.

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(Protocols target, KAEResourceUri resourceUri, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(target, Global.TransactionTimeout, resourceUri, communicationType, protocolSelf);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="maximumRspTime">等待当前网络事务RSP消息的最大时间</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(Protocols target, TimeSpan maximumRspTime, KAEResourceUri resourceUri, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            string errMsg;
            ApplicationLevel level = (resourceUri == null ? ApplicationLevel.Stable : _callback(resourceUri));
            IServerConnectionAgent<TMessage> agent = _cluster.GetChannel(target, level, _container.GetProtocolStack(protocolSelf ?? "METADATA"), out errMsg);
            if(agent == null) return new FailMessageTransaction<TMessage>(errMsg);
            MessageTransaction<TMessage> transaction = NewTransaction(new Lease(DateTime.Now.Add(maximumRspTime)), agent.GetChannel());
            transaction.TransactionManager = _transactionManager;
            transaction.Identity = (communicationType == NetworkCommunicationTypes.Dulplex ? IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP) : IdentityHelper.CreateOneway(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP));
            return (_transactionManager.Add(transaction.Identity, transaction) ? transaction : null);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(Protocols target, KAEResourceUri resourceUri, long balanceFlag, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(target, Global.TransactionTimeout, resourceUri, balanceFlag, communicationType, protocolSelf);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="maximumRspTime">等待当前网络事务RSP消息的最大时间</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(Protocols target, TimeSpan maximumRspTime, KAEResourceUri resourceUri, long balanceFlag, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            string errMsg;
            ApplicationLevel level = (resourceUri == null ? ApplicationLevel.Stable : _callback(resourceUri));
            IServerConnectionAgent<TMessage> agent = _cluster.GetChannel(target, level, _container.GetProtocolStack(protocolSelf ?? "METADATA"), balanceFlag, out errMsg);
            if (agent == null) return new FailMessageTransaction<TMessage>(errMsg);
            MessageTransaction<TMessage> transaction = NewTransaction(new Lease(DateTime.Now.Add(maximumRspTime)), agent.GetChannel());
            transaction.TransactionManager = _transactionManager;
            transaction.Identity = (communicationType == NetworkCommunicationTypes.Dulplex ? IdentityHelper.Create(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP) : IdentityHelper.CreateOneway(agent.GetChannel().LocalEndPoint, TransportChannelTypes.TCP));
            return (_transactionManager.Add(transaction.Identity, transaction) ? transaction : null);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(Protocols target, KAEResourceUri resourceUri, string balanceFlag, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(target, Global.TransactionTimeout, resourceUri, balanceFlag.GetHashCode(), communicationType, protocolSelf);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="maximumRspTime">等待当前网络事务RSP消息的最大时间</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(Protocols target, TimeSpan maximumRspTime, KAEResourceUri resourceUri, string balanceFlag, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(target, maximumRspTime, resourceUri, balanceFlag.GetHashCode(), communicationType, protocolSelf);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(Protocols target, KAEResourceUri resourceUri, Guid balanceFlag, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(target, Global.TransactionTimeout, resourceUri, balanceFlag.GetHashCode(), communicationType, protocolSelf);
        }

        /// <summary>
        ///     创建一个新的网络事务
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="maximumRspTime">等待当前网络事务RSP消息的最大时间</param>
        /// <param name="resourceUri">KAE资源URI</param>
        /// <param name="balanceFlag">负载标识</param>
        /// <param name="communicationType">通信方式</param>
        /// <param name="protocolSelf">
        ///     使用的协议栈的角色
        ///     <para>* 如果当前事务代理器所承载的消息协议为MetadataContainer时请忽略此参数</para>
        /// </param>
        /// <returns>返回新的事务</returns>
        public IMessageTransaction<TMessage> CreateTransaction(Protocols target, TimeSpan maximumRspTime, KAEResourceUri resourceUri, Guid balanceFlag, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null)
        {
            return CreateTransaction(target, maximumRspTime, resourceUri, balanceFlag.GetHashCode(), communicationType, protocolSelf);
        }

        /// <summary>
        ///    为MessageTransactionProxy注入已更新的灰度升级策略
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void UpdateGreyPolicy(Func<IDictionary<string, string>, ApplicationLevel> callback)
        {
            _callback = callback;
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