using System.Collections.Generic;
using KJFramework.Cache.Containers;
using KJFramework.Cache.Proxy;

namespace KJFramework.Cache
{
    /// <summary>
    ///     ����������Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface ICacheTenant
    {
        /// <summary>
        ///     ��������ָ������Ļ�������
        /// </summary>
        /// <param name="category">��������</param>
        void Discard(string category);
        /// <summary>
        ///     ��ȡ����ָ���������ƵĻ�������
        /// </summary>
        /// <typeparam name="T">������������</typeparam>
        /// <param name="category">��������</param>
        /// <returns>���ػ�������</returns>
        T Get<T>(string category);
        /// <summary>
        ///     ���һ���µĹ�̬��������
        /// </summary>
        /// <typeparam name="T">�����������</typeparam>
        /// <param name="category">��������</param>
        /// <param name="capacity">�������</param>
        /// <returns>���ػ�������</returns>
        IFixedCacheContainer<T> Rent<T>(string category, int capacity) where T : IClearable, new();
        /// <summary>
        ///     ���һ���µı��ػ�������
        /// </summary>
        /// <typeparam name="K">����key����</typeparam>
        /// <typeparam name="V">�����������</typeparam>
        /// <param name="category">��������</param>
        /// <returns>���ر��ػ�������</returns>
        ICacheContainer<K, V> Rent<K, V>(string category);
        /// <summary>
        ///     ���һ���µı��ػ�������
        /// </summary>
        /// <typeparam name="K">����key����</typeparam>
        /// <typeparam name="V">�����������</typeparam>
        /// <param name="category">��������</param>
        /// <param name="comparer">�Ƚ��� </param>
        /// <returns>���ر��ػ�������</returns>
        ICacheContainer<K, V> Rent<K, V>(string category, IEqualityComparer<K> comparer);
        /// <summary>
        ///     ���һ���µ�Զ�̻�������
        /// </summary>
        /// <typeparam name="K">����key����</typeparam>
        /// <typeparam name="V">�����������</typeparam>
        /// <param name="category">��������</param>
        /// <param name="proxy">Զ�̻��������</param>
        /// <returns>����Զ�̻�������</returns>
        ICacheContainer<K, V> Rent<K, V>(string category, IRemoteCacheProxy<K, V> proxy);
    }
}