using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Rings;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.Pools;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.Clusters
{
    /// <summary>
    ///    网络软负载均衡器
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    internal class NetworkCluster<TMessage> : INetworkCluster<TMessage>
    {
        #region Constructor.

        /// <summary>
        ///     按照普通HASH方式的网络群集负载器
        /// </summary>
        /// <param name="transactionManager">网络事务管理器</param>
        /// <param name="connectionPool">内部连接池</param>
        /// <param name="protocolType">协议类型</param>
        public NetworkCluster(ITransactionManager<TMessage> transactionManager, ConnectionPool<TMessage> connectionPool, ProtocolTypes protocolType)
        {
            _transactionManager = transactionManager;
            _connectionPool = connectionPool;
            _protocolType = protocolType;
        }

        #endregion

        #region Members.

        private readonly ProtocolTypes _protocolType;
        protected readonly object _lockObj = new object();
        protected readonly ConnectionPool<TMessage> _connectionPool;
        protected readonly ITransactionManager<TMessage> _transactionManager;
        protected static readonly ITracing _tracing = TracingManager.GetTracing(typeof(NetworkCluster<TMessage>));
        protected readonly Dictionary<Protocols, IDictionary<ApplicationLevel, IDictionary<ProtocolTypes, KetamaRing>>> _addresses = new Dictionary<Protocols, IDictionary<ApplicationLevel, IDictionary<ProtocolTypes, KetamaRing>>>();

        /// <summary>
        ///     获取所支持的网络协议类型
        /// </summary>
        public ProtocolTypes ProtocolType
        {
            get { return _protocolType; }
        }

        #endregion

        #region Methods.

        /// <summary>
        ///    更新网络缓存信息
        /// </summary>
        /// <param name="level">应用等级</param>
        /// <param name="cache">远程目标终结点信息列表</param>
        /// <param name="protocol">通信协议</param>
        public void UpdateCache(Protocols protocol, ApplicationLevel level, IList<string> cache)
        {
            //prepares kathma ring.
            KetamaRing ring = ((cache == null || cache.Count == 0) ? null : new KetamaRing(cache.Select(v => new KAEHostNode(v)).ToList()));
            lock (_lockObj)
            {
                IDictionary<ProtocolTypes, KetamaRing> thirdDic;
                IDictionary<ApplicationLevel, IDictionary<ProtocolTypes, KetamaRing>> tempDic;
                if (!_addresses.TryGetValue(protocol, out tempDic)) _addresses.Add(protocol, (tempDic = new Dictionary<ApplicationLevel, IDictionary<ProtocolTypes, KetamaRing>>()));;
                if (!tempDic.TryGetValue(level, out thirdDic)) tempDic.Add(level, (thirdDic = new Dictionary<ProtocolTypes, KetamaRing>()));
                thirdDic[_protocolType] = ring;
            }
        }

        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="level">KAE应用等级</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public IServerConnectionAgent<TMessage> GetChannel(Protocols target, ApplicationLevel level, IProtocolStack protocolStack, out string errMsg)
        {
            lock (_lockObj)
            {
                IDictionary<ApplicationLevel, IDictionary<ProtocolTypes, KetamaRing>> firstLevel;
                if (_addresses.TryGetValue(target, out firstLevel))
                {
                    IDictionary<ProtocolTypes, KetamaRing> secondLevel;
                    if (firstLevel.TryGetValue(level, out secondLevel))
                    {
                        KetamaRing ring;
                        if (secondLevel.TryGetValue(_protocolType, out ring))
                        {
                            if (ring == null)
                            {
                                errMsg = string.Format("#Sadly, We have no more network information about what you gave of protocol: {0}-{1}-{2}, Maybe it was been removed or there wasn't any available network resources.", target.ProtocolId, target.ServiceId, target.DetailsId);
                                return null;
                            }
                            KAEHostNode node = ring.GetWorkerNode();
                            IServerConnectionAgent<TMessage> agent = _connectionPool.GetChannel(node.EndPoint, node.RawAddress, protocolStack, _transactionManager);
                            errMsg = (agent != null ? string.Empty : "#Sadly, We cannot connect to remote endpoint: " + node.RawAddress);
                            return agent;
                        }
                    }
                }
                errMsg = string.Format("#Couldn't find any remoting address which it can access it. #Protocol: {0}, #Level: {1}", target, level);
                return null;
            }
        }

        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="level">KAE应用等级</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="balanceFlag">负载位</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public IServerConnectionAgent<TMessage> GetChannel(Protocols target, ApplicationLevel level, IProtocolStack protocolStack, long balanceFlag, out string errMsg)
        {
            lock (_lockObj)
            {
                IDictionary<ApplicationLevel, IDictionary<ProtocolTypes, KetamaRing>> firstLevel;
                if (_addresses.TryGetValue(target, out firstLevel))
                {
                    IDictionary<ProtocolTypes, KetamaRing> secondLevel;
                    if (firstLevel.TryGetValue(level, out secondLevel))
                    {
                        KetamaRing ring;
                        if (secondLevel.TryGetValue(_protocolType, out ring))
                        {
                            if (ring == null)
                            {
                                errMsg = string.Format("#Sadly, We have no more network information about what you gave of protocol: {0}-{1}-{2}, Maybe it was been removed or there wasn't any available network resources.", target.ProtocolId, target.ServiceId, target.DetailsId);
                                return null;
                            }
                            KAEHostNode node = ring.GetWorkerNode(balanceFlag);
                            IServerConnectionAgent<TMessage> agent = _connectionPool.GetChannel(node.EndPoint, node.RawAddress, protocolStack, _transactionManager);
                            errMsg = (agent != null ? string.Empty : "#Sadly, We cannot connect to remote endpoint: " + node.RawAddress);
                            return agent;
                        }
                    }
                }
                errMsg = string.Format("#Couldn't find any remoting address which it can access it. #Protocol: {0}, #Level: {1}", target, level);
                return null;
            }
        }

        #endregion
    }
}