using System;
using KJFramework.Cache.Containers;
using KJFramework.Cache.Cores;

namespace KJFramework.Cache.Proxy
{
    /// <summary>
    ///     Զ�̻��������Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="K">�������Key����</typeparam>
    /// <typeparam name="V">�����������</typeparam>
    public interface IRemoteCacheProxy<K, V>
    {
        /// <summary>
        ///     ��ȡ��ǰԶ�̻���������Ŀ�����
        /// </summary>
        bool IsAvailable { get; }
        /// <summary>
        ///     ��ȡ�����ñ��ػ�������
        /// </summary>
        ICacheContainer<K, V> LocalContainer { get; set; }
        /// <summary>
        ///     ������ǰ���������еĻ���
        /// </summary>
        void Discard();
        /// <summary>
        ///     ��ѯ����ָ��key�Ļ����Ƿ��Ѿ�����
        /// </summary>
        /// <param name="key">�������Key</param>
        /// <returns>�����Ƿ���ڵ�״̬</returns>
        bool IsExists(K key);
        /// <summary>
        ///     �Ƴ�һ������ָ��key�Ļ������
        /// </summary>
        /// <param name="key">�������Key</param>
        void Remove(K key);
        /// <summary>
        ///     ���һ���µĻ���
        /// </summary>
        /// <param name="key">����Key</param>
        /// <param name="obj">Ҫ����Ե���</param>
        /// <param name="expireTime">����ʱ��</param>
        /// <returns>���ػ������</returns>
        IReadonlyCacheStub<V> Add(K key, V obj, DateTime expireTime);
        /// <summary>
        ///     ��ȡһ������ָ��key�Ļ������
        /// </summary>
        /// <param name="key">�������Key</param>
        /// <returns>���ػ������</returns>
        IReadonlyCacheStub<V> Get(K key);
    }
}