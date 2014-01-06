using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.Pools;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction.Clusters
{
    /// <summary>
    ///     按照普通HASH方式的网络群集负载器
    /// </summary>
    public class HashNetworkCluster : INetworkCluster<BaseMessage>
    {
        #region Constructor

        /// <summary>
        ///     按照普通HASH方式的网络群集负载器
        /// </summary>
        private HashNetworkCluster(MessageTransactionManager transactionManager, IntellectObjectSystemConnectionPool connectionPool, Dictionary<string, ServiceCoreConfig[]> addresses, Dictionary<string, int> maxRanges)
        {
            _transactionManager = transactionManager;
            _connectionPool = connectionPool;
            _addresses = addresses;
            _maxRanges = maxRanges;
        }

        #endregion

        #region Members

        private readonly Dictionary<string, int> _maxRanges;
        private readonly IntellectObjectSystemConnectionPool _connectionPool;
        private readonly MessageTransactionManager _transactionManager;
        private readonly Dictionary<string, ServiceCoreConfig[]> _addresses;
        private static readonly MD5 _md5 = new MD5CryptoServiceProvider();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(HashNetworkCluster));

	    #endregion

        #region Implementation of INetworkCluster

        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public IServerConnectionAgent<BaseMessage> GetChannel(string roleId, IProtocolStack<BaseMessage> protocolStack, out string errMsg)
        {
            ServiceCoreConfig[] configs;
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            if (!_addresses.TryGetValue(roleId, out configs))
            {
                errMsg = "#No address can be matched.";
                return null;
            }
            ServiceCoreConfig coreConfig = configs[DateTime.Now.Ticks % configs.Length];
            IServerConnectionAgent<BaseMessage> agent = _connectionPool.GetChannel(coreConfig.Address, roleId, protocolStack, _transactionManager);
            errMsg = agent != null ? string.Empty : "#Sadly, We cannot connect to remote endpoint: " + coreConfig.Address;
            return agent;
        }

        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="balanceFlag">负载位</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public IServerConnectionAgent<BaseMessage> GetChannelBySpecificCondition(string roleId, IProtocolStack<BaseMessage> protocolStack, int balanceFlag, out string errMsg)
        {
            ServiceCoreConfig[] configs;
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            if (!_addresses.TryGetValue(roleId, out configs))
            {
                errMsg = "#No address can be matched.";
                return null;
            }
            ServiceCoreConfig coreConfig = GetConfig(configs, _maxRanges[roleId], balanceFlag);
            IServerConnectionAgent<BaseMessage> agent = _connectionPool.GetChannel(coreConfig.Address, roleId, protocolStack, _transactionManager);
            errMsg = agent != null ? string.Empty : "#Sadly, We cannot connect to remote endpoint: " + coreConfig.Address;
            return agent;
        }

        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="balanceFlag">负载位</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public IServerConnectionAgent<BaseMessage> GetChannelBySpecificCondition(string roleId, IProtocolStack<BaseMessage> protocolStack, long balanceFlag, out string errMsg)
        {
            ServiceCoreConfig[] configs;
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            if (!_addresses.TryGetValue(roleId, out configs))
            {
                errMsg = "#No address can be matched.";
                return null;
            }
            ServiceCoreConfig coreConfig = GetConfig(configs, _maxRanges[roleId], balanceFlag);
            IServerConnectionAgent<BaseMessage> agent = _connectionPool.GetChannel(coreConfig.Address, roleId, protocolStack, _transactionManager);
            errMsg = agent != null ? string.Empty : "#Sadly, We cannot connect to remote endpoint: " + coreConfig.Address;
            return agent;
        }

        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="balanceFlag">负载位</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public IServerConnectionAgent<BaseMessage> GetChannelBySpecificCondition(string roleId, IProtocolStack<BaseMessage> protocolStack, string balanceFlag, out string errMsg)
        {
            ServiceCoreConfig[] configs;
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            if (string.IsNullOrEmpty(balanceFlag)) throw new ArgumentNullException("balanceFlag");
            if (!_addresses.TryGetValue(roleId, out configs))
            {
                errMsg = "#No address can be matched.";
                return null;
            }
            byte[] source = _md5.ComputeHash(Encoding.UTF8.GetBytes(balanceFlag));
            long flag;
            unsafe { fixed (byte* pByte = source) flag = *(long*) (pByte + 4); }
            ServiceCoreConfig coreConfig = GetConfig(configs, _maxRanges[roleId], flag);
            IServerConnectionAgent<BaseMessage> agent = _connectionPool.GetChannel(coreConfig.Address, roleId, protocolStack, _transactionManager);
            errMsg = agent != null ? string.Empty : "#Sadly, We cannot connect to remote endpoint: " + coreConfig.Address;
            return agent;
        }

        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="balanceFlag">负载位</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public IServerConnectionAgent<BaseMessage> GetChannelBySpecificCondition(string roleId, IProtocolStack<BaseMessage> protocolStack, Guid balanceFlag, out string errMsg)
        {
            ServiceCoreConfig[] configs;
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            if (!_addresses.TryGetValue(roleId, out configs))
            {
                errMsg = "#No address can be matched.";
                return null;
            }
            long flag;
            unsafe
            {
                Guid* pByte = &balanceFlag;
                flag = *(long*)((byte*)pByte + 4); 
            }
            ServiceCoreConfig coreConfig = GetConfig(configs, _maxRanges[roleId], flag);
            IServerConnectionAgent<BaseMessage> agent = _connectionPool.GetChannel(coreConfig.Address, roleId, protocolStack, _transactionManager);
            errMsg = agent != null ? string.Empty : "#Sadly, We cannot connect to remote endpoint: " + coreConfig.Address;
            return agent;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     创建一个新的网络集群负载器
        /// </summary>
        /// <param name="transactionManager">事务管理器</param>
        /// <param name="connectionPool">连接池</param>
        /// <param name="addresses">地址集</param>
        /// <param name="maxRanges">范围集</param>
        /// <returns>返回一个新的网络集群负载器</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public static HashNetworkCluster Create(MessageTransactionManager transactionManager, IntellectObjectSystemConnectionPool connectionPool, Dictionary<string, ServiceCoreConfig[]> addresses, Dictionary<string, int> maxRanges)
        {
            if (transactionManager == null) throw new ArgumentNullException("transactionManager");
            if (connectionPool == null) throw new ArgumentNullException("connectionPool");
            if (addresses == null) throw new ArgumentNullException("addresses");
            if (maxRanges == null) throw new ArgumentNullException("maxRanges");
            return new HashNetworkCluster(transactionManager, connectionPool, addresses, maxRanges);
        }

        /// <summary>
        ///     折半法查找指定用户UserId所在的服务地址
        /// </summary>
        /// <param name="configs">某服务的所有覆盖范围</param>
        /// <param name="maxRange">服务最大覆盖范围</param>
        /// <param name="userId">用户Id</param>
        /// <returns>返回指定用户所在的服务地址</returns>
        internal ServiceCoreConfig GetConfig(ServiceCoreConfig[] configs, int maxRange, int userId)
        {
            int range = userId % maxRange;
            ServiceCoreConfig current;
            int lowBound = 0;
            int highBound = configs.Length;
            while (highBound > lowBound)
            {
                current = configs[(lowBound + highBound) / 2];
                if (current.BeginRange <= range && current.EndRange >= range) return current;
                if (current.EndRange < range)
                {
                    lowBound = (lowBound + highBound) / 2;
                    continue;
                }
                if (range < current.BeginRange)
                {
                    highBound = (lowBound + highBound) / 2;
                    continue;
                }
            }
            return null;
        }


        /// <summary>
        ///     折半法查找指定用户UserId所在的服务地址
        /// </summary>
        /// <param name="configs">某服务的所有覆盖范围</param>
        /// <param name="maxRange">服务最大覆盖范围</param>
        /// <param name="mobileNo">手机号</param>
        /// <returns>返回指定用户所在的服务地址</returns>
        internal ServiceCoreConfig GetConfig(ServiceCoreConfig[] configs, int maxRange, long mobileNo)
        {
            int range = (int)(mobileNo % maxRange);
            ServiceCoreConfig current;
            int lowBound = 0;
            int highBound = configs.Length;
            while (highBound > lowBound)
            {
                current = configs[(lowBound + highBound) / 2];
                if (current.BeginRange <= range && current.EndRange >= range) return current;
                if (current.EndRange < range)
                {
                    lowBound = (lowBound + highBound) / 2;
                    continue;
                }
                if (range < current.BeginRange)
                    highBound = (lowBound + highBound) / 2;
            }
            return null;
        }

        #endregion
    }
}