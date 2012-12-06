using System;
using KJFramework.EventArgs;

namespace KJFramework.Observers
{
    /// <summary>
    ///     �۲���Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">�۲�����</typeparam>
    public interface IObserver<T> : IDisposable
    {
        /// <summary>
        ///     ֪ͨ
        /// </summary>
        /// <param name="target">Ŀ�����</param>
        void Notify(T target);
        /// <summary>
        ///     ֪ͨ�¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> Notifyed;
    }
}