using System;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     事物元接口，提供了相关的基本操作
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        ///     获取事务唯一编号
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     获取当前事务的生命周期租约
        /// </summary>
        /// <returns>返回生命周期租约</returns>
        ILease GetLease();
    }
}