using System;
using KJFramework.Cores;
using KJFramework.EventArgs;

namespace KJFramework.Containers
{
    /// <summary>
    ///     ��������Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="K">�������Key����</typeparam>
    /// <typeparam name="V">�����������</typeparam>
    public interface ICacheContainer<K, V> : ILeasable
    {
        /// <summary>
        ///     ��ȡ��ǰ���������ķ�������
        /// </summary>
        string Category { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�Ļ��������Ƿ�ΪԶ�̻���Ĵ�����
        /// </summary>
        bool IsRemotable { get; }
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
        /// <returns>����ɾ�����״̬</returns>
        bool Remove(K key);
        /// <summary>
        ///     ���һ���µĻ���
        /// </summary>
        /// <param name="key">����Key</param>
        /// <param name="obj">Ҫ����Ե���</param>
        /// <returns>���ػ������</returns>
        IReadonlyCacheStub<V> Add(K key, V obj);
        /// <summary>
        ///     ���һ���µĻ���
        /// </summary>
        /// <param name="key">����Key</param>
        /// <param name="obj">Ҫ����Ե���</param>
        /// <param name="timeSpan">����ʱ��</param>
        /// <returns>���ػ������</returns>
        IReadonlyCacheStub<V> Add(K key, V obj, TimeSpan timeSpan);
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
        /// <summary>
        ///     ��������¼�
        /// </summary>
        event EventHandler<ExpiredCacheEventArgs<K, V>> CacheExpired;
    }
}