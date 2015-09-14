using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.ApplicationEngine.Client.Proxies;
using KJFramework.ApplicationEngine.Clusters;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Net;
using KJFramework.Net.Configurations;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Pools;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Net.Transaction.ValueStored;

namespace KJFramework.ApplicationEngine.Client
{
    /// <summary>
    ///      KAE客户端
    /// </summary>
    public static class KAEClient
    {
        #region Members.

        private static string _roleName;
        private static IKAEResourceProxy _resourceProxy;
        private static Dictionary<string, string> _arguments;
        private static IRemotingProtocolRegister _protocolRegister;
        private static InternalIRemoteConfigurationProxy _configurationProxy;
        private static MessageTransactionManager _transactionManager;
        private static MetadataTransactionManager _metadataTransactionManager;
        private static ConnectionPool<BaseMessage> _baseMessageConnectionPool;
        private static ConnectionPool<MetadataContainer> _metadataConnectionPool;
        private static IMessageTransactionProxy<BaseMessage> _baseMessageTransactionProxy;
        private static INetworkCluster<BaseMessage> _clsuter;
        private static INetworkCluster<MetadataContainer> _metadataCluster;
        private static IProtocolStackContainer _protocolStackContainer;
        private static IMessageTransactionProxy<MetadataContainer> _metadataMessageTransactionProxy;

        /// <summary>
        ///     获取当前是否已经初始化成功
        /// </summary>
        public static bool IsInitialized { get; private set; }

        #endregion

        #region Methods.

        /// <summary>
        ///     初始化KAE客户端
        /// </summary>
        /// <param name="connectionStr">
        ///     连接串信息
        ///     <para>* 此连接串中至少要出现如下key信息:</para>
        ///     <para>* zk=ip:port,ip:port; (远程可访问的ZooKeeper地址)</para>
        ///     <para>* role=XXX (当前的程序角色名称)</para>
        ///     <para />
        ///     <para>* 可选的key如下:</para>
        ///     <para>* zkTimeout=00:00:30</para>
        ///     <para>* buffStubPoolSize=100000 (SocketAsyncEventArgs缓存的数量)</para>
        ///     <para>* maxMessageDataLength=10240000 (最大消息长度, 超过此长度的消息将会被舍弃)</para>
        ///     <para>* namedPipeBuffStubPoolSize=10000 (命名管道的缓冲池大小)</para>
        ///     <para>* noBuffStubPoolSize=50000 (网络IO所使用的无任何关联BUFF内存的SocketAsyncEventArgs缓存数量)</para>
        ///     <para>* recvBufferSize=1024000 (KJFramework老版本网络缓冲区大小)</para>
        ///     <para>* segmentSize=5012 (网络缓冲区所使用的单个内存片段大小)</para>
        /// </param>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="Exception">初始化失败</exception>
        /// <exception cref="ZooKeeperInitializationException">无法正确初始化远程ZooKeeper的状态</exception>
        public static void Initialize(string connectionStr)
        {
            if (IsInitialized) return;
            if (string.IsNullOrEmpty(connectionStr)) throw new ArgumentNullException("connectionStr");
            _arguments = connectionStr.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(v => v.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries)).ToDictionary(v => v[0].Trim(), vs => vs[1]);
            string zkStr, zkTimeoutStr, roleName;
            if (!_arguments.TryGetValue("role", out roleName)) throw new ArgumentException("#Lost KEY argument: \"role\"");
            if (!_arguments.TryGetValue("zk", out zkStr)) throw new ArgumentException("#Lost KEY argument: \"zk\"");
            TimeSpan zkTimeout;
            if (!_arguments.TryGetValue("zkTimeout", out zkTimeoutStr)) zkTimeout = new TimeSpan(0, 3, 0);
            else zkTimeout = TimeSpan.Parse(zkTimeoutStr);
            int buffStubPoolSize, maxMessageDataLength, namedPipeBuffStubPoolSize, noBuffStubPoolSize, recvBufferSize, segmentSize;
            string buffStubPoolSizeStr, maxMessageDataLengthStr, namedPipeBuffStubPoolSizeStr, noBuffStubPoolSizeStr, recvBufferSizeStr, segmentSizeStr;
            if (!_arguments.TryGetValue("buffStubPoolSize", out buffStubPoolSizeStr)) buffStubPoolSize = 100000;
            else buffStubPoolSize = int.Parse(buffStubPoolSizeStr);
            if (!_arguments.TryGetValue("maxMessageDataLength", out maxMessageDataLengthStr)) maxMessageDataLength = 10240000;
            else maxMessageDataLength = int.Parse(maxMessageDataLengthStr);
            if (!_arguments.TryGetValue("namedPipeBuffStubPoolSize", out namedPipeBuffStubPoolSizeStr)) namedPipeBuffStubPoolSize = 10000;
            else namedPipeBuffStubPoolSize = int.Parse(namedPipeBuffStubPoolSizeStr);
            if (!_arguments.TryGetValue("noBuffStubPoolSize", out noBuffStubPoolSizeStr)) noBuffStubPoolSize = 50000;
            else noBuffStubPoolSize = int.Parse(noBuffStubPoolSizeStr);
            if (!_arguments.TryGetValue("recvBufferSize", out recvBufferSizeStr)) recvBufferSize = 1024000;
            else recvBufferSize = int.Parse(recvBufferSizeStr);
            if (!_arguments.TryGetValue("segmentSize", out segmentSizeStr)) segmentSize = 5012;
            else segmentSize = int.Parse(segmentSizeStr);
            ChannelInternalConfigSettings settings = new ChannelInternalConfigSettings
            {
                BuffStubPoolSize = buffStubPoolSize,
                MaxMessageDataLength = maxMessageDataLength,
                NamedPipeBuffStubPoolSize = namedPipeBuffStubPoolSize,
                NoBuffStubPoolSize = noBuffStubPoolSize,
                RecvBufferSize = recvBufferSize,
                SegmentSize = segmentSize
            };
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
            _baseMessageTransactionProxy = new BusinessMessageTransactionProxy(_protocolStackContainer, _clsuter, _transactionManager, _resourceProxy, Guid.Empty);
            _metadataMessageTransactionProxy = new MetadataMessageTransactionProxy(_protocolStackContainer, _metadataCluster, _metadataTransactionManager, _resourceProxy, Guid.Empty);
            //initialize long...long memory buffer for tcp layer.
            ChannelConst.Initialize(settings);
            _roleName = roleName;
            _protocolRegister = new ZooKeeperProtocolRegister(zkStr, zkTimeout);
            _configurationProxy = new InternalIRemoteConfigurationProxy();
            _resourceProxy = new KAEClientResourceProxy(_protocolRegister, _configurationProxy);
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
                    return (IMessageTransactionProxy<TMessage>)_baseMessageTransactionProxy;
                case ProtocolTypes.Metadata:
                    return (IMessageTransactionProxy<TMessage>)_metadataMessageTransactionProxy;
                default:
                    throw new NotImplementedException("#Sadly, We have not supported current message type of transacion proxy: " + typeof(TMessage).Name);
            }
        }

        #endregion

        #region Events.


        #endregion
    }
}