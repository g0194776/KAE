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
        /// <param name="addresses">地址集</param>
        public NetworkCluster(ITransactionManager<TMessage> transactionManager, ConnectionPool<TMessage> connectionPool)
        {
            _transactionManager = transactionManager;
            _connectionPool = connectionPool;
        }

        #endregion

        #region Members.

        protected readonly object _lockObj = new object();
        protected readonly ConnectionPool<TMessage> _connectionPool;
        protected readonly ITransactionManager<TMessage> _transactionManager;
        protected static readonly ITracing _tracing = TracingManager.GetTracing(typeof(NetworkCluster<TMessage>));
        protected readonly Dictionary<Protocols, IDictionary<ApplicationLevel, KetamaRing>> _addresses = new Dictionary<Protocols, IDictionary<ApplicationLevel, KetamaRing>>();

        #endregion

        #region Methods.

        /// <summary>
        ///    更新网络缓存信息
        /// </summary>
        /// <param name="cache">网络信息</param>
        public void UpdateCache(Dictionary<string, List<string>> cache)
        {
            lock (_lockObj)
            {
                foreach (KeyValuePair<string, List<string>> pair in cache)
                {
                    string[] contents = pair.Key.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] ids = contents[0].Replace("(", "").Replace(")", "").Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    ApplicationLevel level;
                    if (!Enum.TryParse(contents[2], out level))
                    {
                        _tracing.Error("#Couldn't parse targeted value to the Application Level. Value: {0}." + contents[2]);
                        continue;
                    }
                    Protocols identity = new Protocols
                    {
                        ProtocolId = byte.Parse(ids[0]),
                        ServiceId = byte.Parse(ids[1]),
                        DetailsId = byte.Parse(ids[2])
                    };
                    //prepares kathma ring.
                    KetamaRing ring = new KetamaRing(pair.Value.Select(v => new KAEHostNode(v)).ToList());
                    lock (_lockObj)
                    {
                        IDictionary<ApplicationLevel, KetamaRing> tempDic;
                        if (!_addresses.TryGetValue(identity, out tempDic)) _addresses.Add(identity, (tempDic = new Dictionary<ApplicationLevel, KetamaRing>()));
                        tempDic[level] = ring;
                    }
                }
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
                IDictionary<ApplicationLevel, KetamaRing> firstLevel;
                if (_addresses.TryGetValue(target, out firstLevel))
                {
                    KetamaRing ring;
                    if (firstLevel.TryGetValue(level, out ring))
                    {
                        KAEHostNode node = ring.GetWorkerNode();
                        IServerConnectionAgent<TMessage> agent = _connectionPool.GetChannel(node.EndPoint, node.RawAddress, protocolStack, _transactionManager);
                        errMsg = (agent != null ? string.Empty : "#Sadly, We cannot connect to remote endpoint: " + node.RawAddress);
                        return agent;
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
                IDictionary<ApplicationLevel, KetamaRing> firstLevel;
                if (_addresses.TryGetValue(target, out firstLevel))
                {
                    KetamaRing ring;
                    if (firstLevel.TryGetValue(level, out ring))
                    {
                        KAEHostNode node = ring.GetWorkerNode(balanceFlag);
                        IServerConnectionAgent<TMessage> agent = _connectionPool.GetChannel(node.EndPoint, node.RawAddress, protocolStack, _transactionManager);
                        errMsg = (agent != null ? string.Empty : "#Sadly, We cannot connect to remote endpoint: " + node.RawAddress);
                        return agent;
                    }
                }
                errMsg = string.Format("#Couldn't find any remoting address which it can access it. #Protocol: {0}, #Level: {1}", target, level);
                return null;
            }
        }

        #endregion
    }
}