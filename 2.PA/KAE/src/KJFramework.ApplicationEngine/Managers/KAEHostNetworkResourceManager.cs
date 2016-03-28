﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Exceptions;
using KJFramework.ApplicationEngine.Helpers;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Net;
using KJFramework.Net.HostChannels;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Uri;
using KJFramework.Tracing;

using Uri = KJFramework.Net.Uri.Uri;

namespace KJFramework.ApplicationEngine.Managers
{
    /// <summary>
    ///     KAE宿主网络资源管理器
    ///     <para>* 此资源管理器是进程级别的</para>
    /// </summary>
    internal static class KAEHostNetworkResourceManager
    {
        #region Members.

        /// <summary>
        ///    获取一个值，该值表示了当前KAE宿主网络资源管理器是否已经初始化完成
        /// </summary>
        public static bool IsInitialized;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(KAEHostNetworkResourceManager));
        private static readonly IDictionary<ProtocolTypes, Tuple<IHostTransportChannel, Uri>> _resources = new Dictionary<ProtocolTypes, Tuple<IHostTransportChannel, Uri>>();

        #endregion

        #region Methods.

        /// <summary>
        ///    初始化资源
        /// </summary>
        /// <param name="businessPort">KAE Agent启动时需要注册的业务通信端口</param>
        /// <param name="systemPort">KAE Agent启动时需要注册的系统管理通信端口</param>
        /// <param name="localIP">当前本机IP</param>
        public static void Initialize(ushort businessPort, ushort systemPort, string localIP = null)
        {
            if (IsInitialized) return;
            IsInitialized = true;
            if (string.IsNullOrEmpty(localIP)) localIP = NetworkHelper.GetCurrentMachineIP();
            /*Openning TCP port for business requests.*/
            TcpUri uri = new TcpUri(string.Format("tcp://{0}:{1}", localIP, businessPort));
            IHostTransportChannel hostChannel = new TcpHostTransportChannel(businessPort);
            hostChannel.ChannelCreated += ChannelCreated;
            if (!hostChannel.Regist())
            {
                hostChannel.ChannelCreated -= ChannelCreated;
                throw new AllocResourceFailedException("#Sadly, We couldn't alloc current network resource. #Uri: " + uri);
            }
            hostChannel.Tag = new KAENetworkResource { NetworkUri = uri, Protocol = ProtocolTypes.Metadata };
            _resources.Add(ProtocolTypes.Metadata, new Tuple<IHostTransportChannel, Uri>(hostChannel, uri));
            _tracing.DebugInfo("\t\t#Initialized network resource.\t(P: {0},\tURL: {1})", ConsoleColor.Gray, ProtocolTypes.Metadata, uri);
            /*Openning TCP port for system management*/
            uri = new TcpUri(string.Format("tcp://{0}:{1}", localIP, systemPort));
            hostChannel = new TcpHostTransportChannel(systemPort);
            hostChannel.ChannelCreated += ChannelCreated;
            if (!hostChannel.Regist())
            {
                hostChannel.ChannelCreated -= ChannelCreated;
                throw new AllocResourceFailedException("#Sadly, We couldn't alloc current network resource. #Uri: " + uri);
            }
            hostChannel.Tag = new KAENetworkResource { NetworkUri = uri, Protocol = ProtocolTypes.INTERNAL_SPECIAL_RESOURCE };
            _resources.Add(ProtocolTypes.INTERNAL_SPECIAL_RESOURCE, new Tuple<IHostTransportChannel, Uri>(hostChannel, uri));
            _tracing.DebugInfo("\t\t#Initialized network resource.\t(P: {0},\tURL: {1})", ConsoleColor.Gray, ProtocolTypes.INTERNAL_SPECIAL_RESOURCE, uri);
        }

        #region Reserved for openning TCP port automatically.

        //public static void Initialize()
        //{
        //    if (IsInitialized) return;
        //    IsInitialized = true;
        //    int startIndex = GetStartingResourceIndex();
        //    for (int i = startIndex, j = 0; i < startIndex + KAESettings.SupportedProtocolCount; i++, j++)
        //    {
        //        TcpUri uri = new TcpUri(string.Format("tcp://{0}:{1}", NetworkHelper.GetCurrentMachineIP(), i));
        //        IHostTransportChannel hostChannel = new TcpHostTransportChannel(i);
        //        hostChannel.ChannelCreated += ChannelCreated;
        //        if (!hostChannel.Regist())
        //        {
        //            hostChannel.ChannelCreated -= ChannelCreated;
        //            throw new AllocResourceFailedException("#Sadly, We couldn't alloc current network resource. #Uri: " + uri);
        //        }
        //        hostChannel.Tag = new KAENetworkResource { NetworkUri = uri, Protocol = KAESettings.SupportedProtocols[j] };
        //        _resources.Add(KAESettings.SupportedProtocols[j], new Tuple<IHostTransportChannel, Uri>(hostChannel, uri));
        //        _tracing.DebugInfo("\t\t#Initialized network resource.\t(P: {0},\tURL: {1})", ConsoleColor.DarkGray, KAESettings.SupportedProtocols[j], uri);
        //    }
        //}

        #endregion

        /// <summary>
        ///     获取指定被支持的网络传输协议在当前KAE进程中所属的通信地址
        /// </summary>
        /// <param name="protocol">被支持的网络通信协议</param>
        /// <returns>返回当前进程支持的通信地址</returns>
        public static Uri GetResourceUri(ProtocolTypes protocol)
        {
            Tuple<IHostTransportChannel, Uri> tuple;
            return !_resources.TryGetValue(protocol, out tuple) ? null : tuple.Item2;
        }

        /// <summary>
        ///    获取当前KAE进程级别宿主内部所需网络资源的起始编号
        /// </summary>
        /// <returns>返回内部所需网络资源的起始编号</returns>
        private static int GetStartingResourceIndex()
        {
            IEnumerable<Process> processSet = Process.GetProcesses().Where(pc => pc.ProcessName.Contains("KJFramework.ApplicationEngine.KAEWorker"));
            int m, n;
            if (!processSet.Any())
            {
                m = Process.GetCurrentProcess().Id;
                n = 1;
            }
            else
            {
                m = processSet.Max(pc => pc.Id);
                n = processSet.Count();
            }
            return m + n + (n*KAESettings.SupportedProtocolCount);
        }

        #endregion

        #region Events.

        internal static void ChannelCreated(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {
            IHostTransportChannel hostChannel = (IHostTransportChannel)sender;
            KAENetworkResource tag = (KAENetworkResource)hostChannel.Tag;
            if (tag.Protocol == ProtocolTypes.Metadata)
            {
                IMessageTransportChannel<MetadataContainer> msgChannel = new MessageTransportChannel<MetadataContainer>((IRawTransportChannel)e.Target, (IProtocolStack)NetworkHelper.GetProtocolStack(tag.Protocol));
                MetadataConnectionAgent agent = new MetadataConnectionAgent(msgChannel, (MetadataTransactionManager)NetworkHelper.GetTransactionManager(tag.Protocol));
                agent.Disconnected += AgentDisconnected;
                agent.NewTransaction += MetadataNewTransactionHandler;
                agent.Tag = tag;
            }
            else if (tag.Protocol == ProtocolTypes.Intellegence)
            {
                IMessageTransportChannel<BaseMessage> msgChannel = new MessageTransportChannel<BaseMessage>((IRawTransportChannel)e.Target, (IProtocolStack)NetworkHelper.GetProtocolStack(tag.Protocol));
                IntellectObjectConnectionAgent agent = new IntellectObjectConnectionAgent(msgChannel, (MessageTransactionManager)NetworkHelper.GetTransactionManager(tag.Protocol));
                agent.Disconnected += AgentDisconnected;
                agent.NewTransaction += IntellegenceNewTransactionHandler;
                agent.Tag = tag;
            }
        }

        /// <summary>
        ///    新的基于元数据协议的网络事务被创建事件
        /// </summary>
        public static event EventHandler<LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>>> MetadataNewTransaction;
        private static void OnMetadataNewTransaction(object sender, LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>> e)
        {
            EventHandler<LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>>> handler =
                MetadataNewTransaction;
            if (handler != null) handler(sender, e);
        }

        /// <summary>
        ///    新的基于智能对象协议的网络事务被创建事件
        /// </summary>
        public static event EventHandler<LightSingleArgEventArgs<IMessageTransaction<BaseMessage>>> IntellegenceNewTransaction;
        private static void OnIntellegenceNewTransaction(object sender, LightSingleArgEventArgs<IMessageTransaction<BaseMessage>> e)
        {
            EventHandler<LightSingleArgEventArgs<IMessageTransaction<BaseMessage>>> handler = IntellegenceNewTransaction;
            if (handler != null) handler(sender, e);
        }

        private static void AgentDisconnected(object sender, System.EventArgs e)
        {
            IConnectionAgent agent = (IConnectionAgent)sender;
            agent.Disconnected -= AgentDisconnected;
        }

        private static void MetadataNewTransactionHandler(object sender, LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>> e)
        {
            OnMetadataNewTransaction(sender, e);
        }

        private static void IntellegenceNewTransactionHandler(object sender, LightSingleArgEventArgs<IMessageTransaction<BaseMessage>> e)
        {
            OnIntellegenceNewTransaction(sender, e);
        }

        #endregion
    }
}