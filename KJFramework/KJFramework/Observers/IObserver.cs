using System;
using KJFramework.EventArgs;

namespace KJFramework.Observers
{
    /// <summary>
    ///     观察者元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">观察类型</typeparam>
    public interface IObserver<T> : IDisposable
    {
        /// <summary>
        ///     通知
        /// </summary>
        /// <param name="target">目标对象</param>
        void Notify(T target);
        /// <summary>
        ///     通知事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> Notifyed;
    }
}