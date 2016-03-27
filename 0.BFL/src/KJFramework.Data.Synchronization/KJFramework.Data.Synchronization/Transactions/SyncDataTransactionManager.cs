using KJFramework.Messages.Contracts;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using KJFramework.Net;
using KJFramework.Net.Identities;

namespace KJFramework.Data.Synchronization.Transactions
{
    /// <summary>
    ///     同步数据事务管理器
    /// </summary>
    internal class SyncDataTransactionManager
    {
        #region Constructor

        /// <summary>
        ///     同步数据事务管理器
        /// </summary>
        private SyncDataTransactionManager()
        {
            _chkThread = new Thread(ChkProc) {IsBackground = true, Name = "SyncDataFramework::ChkThread"};
            _chkThread.Start();
        }

        #endregion

        #region Members

        private Thread _chkThread;
        public static readonly SyncDataTransactionManager Instance = new SyncDataTransactionManager();
        private static readonly ConcurrentDictionary<TransactionIdentity, SyncDataTransaction> _trans = new ConcurrentDictionary<TransactionIdentity, SyncDataTransaction>(new TransactionIdentityComparer());

        #endregion

        #region Implementation of ISyncDataTransactionManager

        /// <summary>
        ///     创建一个新的事务
        ///     <para>* 使用此方法创建的事物，唯一标识将会使用本地的通信地址</para>
        /// </summary>
        /// <param name="channel">事务内部的通信信道</param>
        /// <returns>返回创建后的新事物</returns>
        public SyncDataTransaction Create(IMessageTransportChannel<MetadataContainer> channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            SyncDataTransaction tran = new SyncDataTransaction(channel) { Identity = IdentityHelper.Create(channel.LocalEndPoint, channel.ChannelType) };
            return _trans.TryAdd(tran.Identity, tran) ? tran : null;
        }

        /// <summary>
        ///     创建一个新的事务
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="channel">事务内部的通信信道</param>
        /// <returns>返回创建后的新事物</returns>
        public SyncDataTransaction Create(TransactionIdentity identity, IMessageTransportChannel<MetadataContainer> channel)
        {
            if (identity == null) throw new ArgumentNullException("identity");
            if (channel == null) throw new ArgumentNullException("channel");
            SyncDataTransaction tran;
            if (_trans.TryGetValue(identity, out tran)) return null;
            tran = new SyncDataTransaction(channel) { Identity = identity };
            return _trans.TryAdd(tran.Identity, tran) ? tran : null;
        }

        /// <summary>
        ///     移除一个事务
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        public void Remove(TransactionIdentity identity)
        {
            if (identity == null) throw new ArgumentNullException("identity");
            SyncDataTransaction tran;
            _trans.TryRemove(identity, out tran);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     内部事务检查函数
        /// </summary>
        private void ChkProc()
        {
            while (true)
            {
                Thread.Sleep(Global.TranChkInterval);
                IList<TransactionIdentity> failedList = new List<TransactionIdentity>();
                foreach (var pair in _trans) if (pair.Value.GetLease().IsDead) failedList.Add(pair.Key);
                if(failedList.Count > 0)
                {
                    SyncDataTransaction transaction;
                    foreach (TransactionIdentity identity in failedList)
                        if (_trans.TryRemove(identity, out transaction)) transaction.SetTimeout();
                }
            }
        }

        /// <summary>
        ///     激活一个事务，并尝试处理该事务的响应消息流程
        /// </summary>
        /// <param name="identity">事务唯一标示</param>
        /// <param name="response">响应消息</param>
        public void Active(TransactionIdentity identity, MetadataContainer response)
        {
            SyncDataTransaction transaction;
            if (!_trans.TryRemove(identity, out transaction)) return;
            transaction.SetResponse(response);
        }

        #endregion
    }
}