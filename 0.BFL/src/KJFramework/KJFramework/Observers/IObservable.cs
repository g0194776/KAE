using System.Collections.Generic;

namespace KJFramework.Observers
{
    /// <summary>
    ///     可观察的元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="TKey">观察唯一键值</typeparam>
    /// <typeparam name="TValue">观察类型</typeparam>
    public interface IObservable<TKey, TValue>
    {
        /// <summary>
        ///     获取观察者列表
        /// </summary>
        Dictionary<TKey, IObserver<TValue>>  Observers{ get; }
        /// <summary>
        ///     获取一个具有指定KEY的观察者
        /// </summary>
        /// <param name="key">目标KEY</param>
        /// <returns>返回观察者</returns>
        IObserver<TValue> GetObserver(TKey key);
    }
}