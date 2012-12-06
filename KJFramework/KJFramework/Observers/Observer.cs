using System;
using KJFramework.EventArgs;

namespace KJFramework.Observers
{
    /// <summary>
    ///     观察者抽象父类，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">观察类型</typeparam>
    public abstract class Observer<T> : IObserver<T>
    {
        #region 析构函数

        ~Observer()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        ///     通知
        /// </summary>
        /// <param name="target">目标对象</param>
        public abstract void Notify(T target);

        /// <summary>
        ///     通知事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<T>> Notifyed;
        protected void NotifyedHandler(LightSingleArgEventArgs<T> e)
        {
            EventHandler<LightSingleArgEventArgs<T>> handler = Notifyed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}