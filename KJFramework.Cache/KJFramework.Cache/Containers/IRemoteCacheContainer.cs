using System;
using KJFramework.Cache.Cores;
using KJFramework.Cache.Proxy;

namespace KJFramework.Cache.Containers
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