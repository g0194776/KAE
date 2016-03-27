using KJFramework.Messages.Contracts;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using KJFramework.Net.Identities;

namespace KJFramework.Net.Transaction.Managers
{
    /// <summary>
    ///     消息事务管理器，提供了相关的基本操作
    /// </summary>
    public class MetadataTransactionManager : TransactionManager<MetadataContainer>
    {
        #region Constructor

        /// <summary>
        ///     消息事务管理器，提供了相关的基本操作
        ///     * 默认时间：从配置文件中读取.
        /// </summary>
        /// <param name="comparer">比较器</param>
        public MetadataTransactionManager(IEqualityComparer<TransactionIdentity> comparer)
            : this(comparer, Global.TransactionCheckInterval)
        {
        }

        /// <summary>
        ///     消息事务管理器，提供了相关的基本操作
        ///     * 默认时间：30s.
        /// </summary>
        /// <param name="interval">事务检查时间间隔</param>
        /// <param name="comparer">比较器</param>
        public MetadataTransactionManager(IEqualityComparer<TransactionIdentity> comparer, int interval = 30000)
            : base(interval, comparer)
        {
        }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(MetadataTransactionManager));

        #endregion

        #region Methods

        /// <summary>
        ///     创建一个新的消息事务，并将其加入到当前的事务列表中
        ///     <para>* 事务超时时间被设置为KJFramework.Message节点的配置。</para>
        ///     <para>* 默认为: 30S</para>
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="channel">消息通信信道</param>
        /// <returns>返回一个新的消息事务</returns>
        /// <exception cref="ArgumentNullException">通信信道不能为空</exception>
        public MetadataMessageTransaction Create(TransactionIdentity identity, IMessageTransportChannel<MetadataContainer> channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            MetadataMessageTransaction transaction = new MetadataMessageTransaction(new Lease(DateTime.Now.Add(Global.TransactionTimeout)), channel) { TransactionManager = this, Identity = identity };
            return Add(identity, transaction) ? transaction : null;
        }

        /// <summary>
        ///     创建一个新的消息事务，并将其加入到当前的事务列表中
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="channel">消息通信信道</param>
        /// <param name="timeout">事务超时时间</param>
        /// <returns>返回一个新的消息事务</returns>
        /// <exception cref="ArgumentNullException">通信信道不能为空</exception>
        public MetadataMessageTransaction Create(TransactionIdentity identity, IMessageTransportChannel<MetadataContainer> channel, TimeSpan timeout)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            MetadataMessageTransaction transaction = new MetadataMessageTransaction(new Lease(DateTime.Now.Add(timeout)), channel) { TransactionManager = this, Identity = identity };
            return Add(identity, transaction) ? transaction : null;
        }

        #endregion
    }
}