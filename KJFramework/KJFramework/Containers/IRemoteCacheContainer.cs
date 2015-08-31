using KJFramework.Proxy;

namespace KJFramework.Containers
{
    /// <summary>
    ///     Զ�̻�������Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="K">�������Key����</typeparam>
    /// <typeparam name="V">�����������</typeparam>
    public interface IRemoteCacheContainer<K, V> : ICacheContainer<K, V>
    {
        /// <summary>
        ///     ��ȡԶ�̻��������
        /// </summary>
        IRemoteCacheProxy<K, V> Proxy { get; }
    }
}