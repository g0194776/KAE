using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Enums;
using KJFramework.Net.Transaction.Objects;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///    消息事务代理器接口
    /// </summary>
    /// <typeparam name="TMessage">消息事务所承载的消息类型</typeparam>
    public interface IMessageTransactionProxy<TMessage>
    {
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
        IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null);
        /// <summary>string version= "", 
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
        IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, TimeSpan maximumRspTime, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null);
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
        IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, long balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null);
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
        IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, TimeSpan maximumRspTime, long balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null);
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
        IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, string balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null);
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
        IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, TimeSpan maximumRspTime, string balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null);
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
        IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, Guid balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null);
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
        IMessageTransaction<TMessage> CreateTransaction(string appName, Protocols target, TimeSpan maximumRspTime, Guid balanceFlag, string version = "latest", KAEResourceUri resourceUri = null, NetworkCommunicationTypes communicationType = NetworkCommunicationTypes.Dulplex, string protocolSelf = null);
        /// <summary>
        ///    为MessageTransactionProxy注入已更新的灰度升级策略
        /// </summary>
        /// <param name="callback">回调方法</param>
        void UpdateGreyPolicy(Func<IDictionary<string, string>, ApplicationLevel> callback);

        #endregion
    }
}