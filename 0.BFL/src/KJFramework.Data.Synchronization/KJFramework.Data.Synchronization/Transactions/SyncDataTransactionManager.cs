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
    ///     ͬ���������������
    /// </summary>
    internal class SyncDataTransactionManager
    {
        #region Constructor

        /// <summary>
        ///     ͬ���������������
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
        ///     ����һ���µ�����
        ///     <para>* ʹ�ô˷������������Ψһ��ʶ����ʹ�ñ��ص�ͨ�ŵ�ַ</para>
        /// </summary>
        /// <param name="channel">�����ڲ���ͨ���ŵ�</param>
        /// <returns>���ش������������</returns>
        public SyncDataTransaction Create(IMessageTransportChannel<MetadataContainer> channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            SyncDataTransaction tran = new SyncDataTransaction(channel) { Identity = IdentityHelper.Create(channel.LocalEndPoint, channel.ChannelType) };
            return _trans.TryAdd(tran.Identity, tran) ? tran : null;
        }

        /// <summary>
        ///     ����һ���µ�����
        /// </summary>
        /// <param name="identity">����Ψһ��ʾ</param>
        /// <param name="channel">�����ڲ���ͨ���ŵ�</param>
        /// <returns>���ش������������</returns>
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
        ///     �Ƴ�һ������
        /// </summary>
        /// <param name="identity">����Ψһ��ʾ</param>
        public void Remove(TransactionIdentity identity)
        {
            if (identity == null) throw new ArgumentNullException("identity");
            SyncDataTransaction tran;
            _trans.TryRemove(identity, out tran);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     �ڲ������麯��
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
        ///     ����һ�����񣬲����Դ�����������Ӧ��Ϣ����
        /// </summary>
        /// <param name="identity">����Ψһ��ʾ</param>
        /// <param name="response">��Ӧ��Ϣ</param>
        public void Active(TransactionIdentity identity, MetadataContainer response)
        {
            SyncDataTransaction transaction;
            if (!_trans.TryRemove(identity, out transaction)) return;
            transaction.SetResponse(response);
        }

        #endregion
    }
}