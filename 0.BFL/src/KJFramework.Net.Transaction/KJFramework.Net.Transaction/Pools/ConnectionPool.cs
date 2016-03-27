﻿using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Net;
using KJFramework.Exceptions;
using KJFramework.Net.Extends;

namespace KJFramework.Net.Transaction.Pools
{
    /// <summary>
    ///     连接池
    /// </summary>
    /// <typeparam name="TMessage">消息类型</typeparam>
    public abstract class ConnectionPool<TMessage>
    {
        #region Constructor.

        /// <summary>
        ///    连接池基础构造
        ///    <para>* 使用此构造函数将会初始化一个最少1最大3个的连接池配置</para>
        /// </summary>
        protected ConnectionPool()
            : this(Global.MinimumConnectionCount, Global.MaximumConnectionCount)
        {

        }

        /// <summary>
        ///    连接池构造
        /// </summary>
        /// <param name="min">当前池中每个KEY所需要承载的最小的连接数</param>
        /// <param name="max">当前池中每个KEY所需要承载的最大的连接数</param>
        /// <exception cref="OutOfRangeException">传入的参数值超出预期范围</exception>
        protected ConnectionPool(int min, int max)
        {
            if (min == 0) throw new OutOfRangeException("#The parameter value of #min MUST greater than zero.");
            if (max == 0) throw new OutOfRangeException("#The parameter value of #max MUST greater than zero.");
            if (max < min) throw new OutOfRangeException("#Max value MUST greater or equals than #min value. ");
            _min = min;
            _max = max;
        }

        #endregion

        #region Members

        private readonly int _min;
        private readonly int _max;
        private readonly object _lockObj = new object();
        protected static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ConnectionPool<TMessage>));
        private readonly IDictionary<string, ConnectionSet<TMessage>> _connections = new Dictionary<string, ConnectionSet<TMessage>>();

        #endregion

        #region Methods

        /// <summary>
        ///     获取具有指定唯一标示的消息通信信道
        /// </summary>
        /// <param name="iep">远程终结点地址，如果给予的目标key在当前连接池中并不存在任何连接，则会使用此参数来创建一个连接</param>
        /// <param name="key">连接标示</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="transactionManager">网络事务管理器</param>
        /// <returns>返回一个消息通信信道</returns>
        public virtual IServerConnectionAgent<TMessage> GetChannel(string iep, string key, IProtocolStack protocolStack, object transactionManager)
        {
            return GetChannel(iep.ConvertToIPEndPoint(), key, protocolStack, transactionManager);
        }

        /// <summary>
        ///     获取具有指定唯一标示的消息通信信道
        /// </summary>
        /// <param name="iep">远程终结点地址，如果给予的目标key在当前连接池中并不存在任何连接，则会使用此参数来创建一个连接</param>
        /// <param name="key">连接标示</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="transactionManager">网络事务管理器</param>
        /// <returns>返回一个消息通信信道</returns>
        public virtual IServerConnectionAgent<TMessage> GetChannel(IPEndPoint iep, string key, IProtocolStack protocolStack, object transactionManager)
        {
            try
            {
                lock (_lockObj)
                {
                    ConnectionSet<TMessage> connectionSet;
                    if (!_connections.TryGetValue(key, out connectionSet))
                    {
                        connectionSet = new RamdomConnectionSet<TMessage>(_min, _max, new Tuple<IPEndPoint, IProtocolStack, object>(iep, protocolStack, transactionManager), CreateAgent);
                        connectionSet.Tag = key;
                        _connections.Add(key, connectionSet);
                    }
                    return connectionSet.GetConnection();
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return null;
            }
        }

        /// <summary>
        ///     移除具有指定唯一标示的消息通信信道
        /// </summary>
        /// <param name="key">连接标示</param>
        /// <returns>返回移除后的状态</returns>
        public virtual bool Remove(string key)
        {
            lock(_lockObj) return _connections.Remove(key);
        }

        /// <summary>
        ///    创建一个新的服务器端连接代理器，并将其注册到当前的连接池中
        /// </summary>
        /// <param name="iep">要创建连接的远程终结点地址</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="transactionManager">网络事务管理器</param>
        /// <returns>返回已经创建好的服务器端连接代理器</returns>
        protected abstract IServerConnectionAgent<TMessage> CreateAgent(IPEndPoint iep, IProtocolStack protocolStack, object transactionManager);

        #endregion
    }
}