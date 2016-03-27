using System.Collections.Generic;

namespace KJFramework.Observers
{
    /// <summary>
    ///     �ɹ۲��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="TKey">�۲�Ψһ��ֵ</typeparam>
    /// <typeparam name="TValue">�۲�����</typeparam>
    public interface IObservable<TKey, TValue>
    {
        /// <summary>
        ///     ��ȡ�۲����б�
        /// </summary>
        Dictionary<TKey, IObserver<TValue>>  Observers{ get; }
        /// <summary>
        ///     ��ȡһ������ָ��KEY�Ĺ۲���
        /// </summary>
        /// <param name="key">Ŀ��KEY</param>
        /// <returns>���ع۲���</returns>
        IObserver<TValue> GetObserver(TKey key);
    }
}