using System;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     事物，提供了相关的基本操作
    /// </summary>
    public class Transaction : ITransaction
    {
        #region Constructor

        /// <summary>
        ///     事物，提供了相关的基本操作
        ///     <para>* 使用此构造，将会设置当前的事务永远不超时</para>
        /// </summary>
        public Transaction() : this(new Lease(DateTime.MaxValue))
        {
        }

        /// <summary>
        ///     事物，提供了相关的基本操作
        /// </summary>
        /// <param name="lease">当前事务的生命周期租约</param>
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
        ///     获取事务唯一编号
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     获取当前事务的生命周期租约
        /// </summary>
        /// <returns>返回生命周期租约</returns>
        public ILease GetLease()
        {
            return _lease;
        }

        #endregion
    }
}