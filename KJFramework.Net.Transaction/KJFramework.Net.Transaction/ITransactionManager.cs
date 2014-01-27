using System;
using KJFramework.EventArgs;
using KJFramework.Net.Transaction.Identities;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     事务管理器元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="V">事务类型</typeparam>
    public interface ITransactionManager<V>
        where V : ITransaction
    {
        /// <summary>
        ///     获取事务检查的时间间隔
        ///     <para>* 单位: 毫秒</para>
        /// </summary>
        int Interval { get; }
        /// <summary>
        ///     管理一个事务
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <param name="transaction">事务</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        /// <returns>返回添加操作的状态</returns>
        bool Add(TransactionIdentity key, V transaction);
        /// <summary>
        ///     获取一个正在管理中的事务
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <returns>事务</returns>
        V GetTransaction(TransactionIdentity key);
        /// <summary>
        ///     移除一个不需要管理的事务
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        void Remove(TransactionIdentity key);
        /// <summary>
        ///     为一个管理中的事务进行续约操作
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <param name="timeSpan">续约时间</param>
        /// <returns>
        ///     返回续约后的时间
        ///     <para>* 如果返回值 = MIN(DateTime), 则表示续约失败</para>
        /// </returns>
        DateTime Renew(TransactionIdentity key, TimeSpan timeSpan);
        /// <summary>
        ///     尝试获取一个具有指定唯一标示的事务，并且在获取该事务后进行移除操作
        /// </summary>
        /// <param name="key">事务唯一键值</param>
        /// <returns>返回获取到的事务</returns>
        V GetRemove(TransactionIdentity key);
        /// <summary>
        ///     事务过期事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<V>> TransactionExpired;
    }
}