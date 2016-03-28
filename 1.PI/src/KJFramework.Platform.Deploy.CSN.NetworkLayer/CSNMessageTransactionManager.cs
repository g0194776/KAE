using System;
using System.Collections.Generic;
using KJFramework.Net;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Tracing;

namespace KJFramework.Platform.Deploy.CSN.NetworkLayer
{
    /// <summary>
    ///     消息事务管理器，提供了相关的基本操作
    /// </summary>
    public class CSNMessageTransactionManager : TransactionManager<BaseMessage>
    {
        #region Constructor

        /// <summary>
        ///     消息事务管理器，提供了相关的基本操作
        ///     * 默认时间：从配置文件中读取.
        /// </summary>
        /// <param name="comparer">比较器</param>
        public CSNMessageTransactionManager(IEqualityComparer<TransactionIdentity> comparer)
            : this(comparer, CSNGlobal.TransactionCheckInterval)
        {
        }

        /// <summary>
        ///     消息事务管理器，提供了相关的基本操作
        ///     * 默认时间：30s.
        /// </summary>
        /// <param name="interval">事务检查时间间隔</param>
        /// <param name="comparer">比较器</param>
        public CSNMessageTransactionManager(IEqualityComparer<TransactionIdentity> comparer, int interval = 30000)
            : base(interval, comparer)
        {
        }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(CSNMessageTransactionManager));

        #endregion

        #region Methods

        /// <summary>
        ///     创建一个新的消息事务，并将其加入到当前的事务列表中
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="channel">消息通信信道</param>
        /// <returns>返回一个新的消息事务</returns>
        /// <exception cref="ArgumentNullException">通信信道不能为空</exception>
        public CSNBusinessMessageTransaction Create(TransactionIdentity identity, IMessageTransportChannel<BaseMessage> channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            CSNBusinessMessageTransaction transaction = new CSNBusinessMessageTransaction(new Lease(DateTime.MaxValue), channel) { TransactionManager = this, Identity = identity };
            return (Add(identity, transaction) ? transaction : null);
        }

        #endregion
    }
}