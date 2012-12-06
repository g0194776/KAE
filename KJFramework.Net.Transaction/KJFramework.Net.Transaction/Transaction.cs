using System;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     ����ṩ����صĻ�������
    /// </summary>
    public class Transaction : ITransaction
    {
        #region Constructor

        /// <summary>
        ///     ����ṩ����صĻ�������
        ///     <para>* ʹ�ô˹��죬�������õ�ǰ��������Զ����ʱ</para>
        /// </summary>
        public Transaction() : this(new Lease(DateTime.MaxValue))
        {
        }

        /// <summary>
        ///     ����ṩ����صĻ�������
        /// </summary>
        /// <param name="lease">��ǰ���������������Լ</param>
        public Transaction(ILease lease)
        {
            if (lease == null) throw new ArgumentNullException("lease");
            if (lease.IsDead) throw new ArgumentException("Illegal lease. #reason: IsDead = true.");
            _id = Guid.NewGuid();
            _lease = lease;
        }

        #endregion

        #region Implementation of ITransaction

        protected readonly Guid _id;
        protected readonly ILease _lease;
        private object _tag;

        /// <summary>
        ///     ��ȡ����Ψһ���
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     ��ȡ�����ø�������
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     ��ȡ��ǰ���������������Լ
        /// </summary>
        /// <returns>��������������Լ</returns>
        public ILease GetLease()
        {
            return _lease;
        }

        #endregion
    }
}