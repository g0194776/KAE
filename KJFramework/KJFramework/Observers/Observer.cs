using System;
using KJFramework.EventArgs;

namespace KJFramework.Observers
{
    /// <summary>
    ///     �۲��߳����࣬�ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">�۲�����</typeparam>
    public abstract class Observer<T> : IObserver<T>
    {
        #region ��������

        ~Observer()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        ///     ֪ͨ
        /// </summary>
        /// <param name="target">Ŀ�����</param>
        public abstract void Notify(T target);

        /// <summary>
        ///     ֪ͨ�¼�
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