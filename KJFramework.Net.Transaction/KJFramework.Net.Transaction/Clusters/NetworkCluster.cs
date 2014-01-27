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
    ///     ������ͨHASH��ʽ������Ⱥ��������
    /// </summary>
    public class HashNetworkCluster : INetworkCluster<BaseMessage>
    {
        #region Constructor

        /// <summary>
        ///     ������ͨHASH��ʽ������Ⱥ��������
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
        ///     ���ݵ�ǰ�����������ȡһ��ͨ���ŵ�
        /// </summary>
        /// <param name="roleId">��ɫ���</param>
        /// <param name="protocolStack">Э��ջ</param>
        /// <param name="errMsg">������Ϣ</param>
        /// <returns>���ָ��������ͨ���ŵ������ڣ���ᴴ����������</returns>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
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
        ///     ���ݵ�ǰ�����������ȡһ��ͨ���ŵ�
        /// </summary>
        /// <param name="roleId">��ɫ���</param>
        /// <param name="protocolStack">Э��ջ</param>
        /// <param name="balanceFlag">����λ</param>
        /// <param name="errMsg">������Ϣ</param>
        /// <returns>���ָ��������ͨ���ŵ������ڣ���ᴴ����������</returns>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
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
        ///     ���ݵ�ǰ�����������ȡһ��ͨ���ŵ�
        /// </summary>
        /// <param name="roleId">��ɫ���</param>
        /// <param name="protocolStack">Э��ջ</param>
        /// <param name="balanceFlag">����λ</param>
        /// <param name="errMsg">������Ϣ</param>
        /// <returns>���ָ��������ͨ���ŵ������ڣ���ᴴ����������</returns>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
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
        ///     ���ݵ�ǰ�����������ȡһ��ͨ���ŵ�
        /// </summary>
        /// <param name="roleId">��ɫ���</param>
        /// <param name="protocolStack">Э��ջ</param>
        /// <param name="balanceFlag">����λ</param>
        /// <param name="errMsg">������Ϣ</param>
        /// <returns>���ָ��������ͨ���ŵ������ڣ���ᴴ����������</returns>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
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
        ///     ���ݵ�ǰ�����������ȡһ��ͨ���ŵ�
        /// </summary>
        /// <param name="roleId">��ɫ���</param>
        /// <param name="protocolStack">Э��ջ</param>
        /// <param name="balanceFlag">����λ</param>
        /// <param name="errMsg">������Ϣ</param>
        /// <returns>���ָ��������ͨ���ŵ������ڣ���ᴴ����������</returns>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
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
        ///     ����һ���µ����缯Ⱥ������
        /// </summary>
        /// <param name="transactionManager">���������</param>
        /// <param name="connectionPool">���ӳ�</param>
        /// <param name="addresses">��ַ��</param>
        /// <param name="maxRanges">��Χ��</param>
        /// <returns>����һ���µ����缯Ⱥ������</returns>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
        public static HashNetworkCluster Create(MessageTransactionManager transactionManager, IntellectObjectSystemConnectionPool connectionPool, Dictionary<string, ServiceCoreConfig[]> addresses, Dictionary<string, int> maxRanges)
        {
            if (transactionManager == null) throw new ArgumentNullException("transactionManager");
            if (connectionPool == null) throw new ArgumentNullException("connectionPool");
            if (addresses == null) throw new ArgumentNullException("addresses");
            if (maxRanges == null) throw new ArgumentNullException("maxRanges");
            return new HashNetworkCluster(transactionManager, connectionPool, addresses, maxRanges);
        }

        /// <summary>
        ///     �۰뷨����ָ���û�UserId���ڵķ����ַ
        /// </summary>
        /// <param name="configs">ĳ��������и��Ƿ�Χ</param>
        /// <param name="maxRange">������󸲸Ƿ�Χ</param>
        /// <param name="userId">�û�Id</param>
        /// <returns>����ָ���û����ڵķ����ַ</returns>
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
        ///     �۰뷨����ָ���û�UserId���ڵķ����ַ
        /// </summary>
        /// <param name="configs">ĳ��������и��Ƿ�Χ</param>
        /// <param name="maxRange">������󸲸Ƿ�Χ</param>
        /// <param name="mobileNo">�ֻ���</param>
        /// <returns>����ָ���û����ڵķ����ַ</returns>
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