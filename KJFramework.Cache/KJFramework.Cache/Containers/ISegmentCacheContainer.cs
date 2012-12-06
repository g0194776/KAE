using System;
using KJFramework.EventArgs;

namespace KJFramework.Cache.Containers
{
    /// <summary>
    ///     Ƭ��ʽ��������Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">Ψһ��ʶ����</typeparam>
    public interface ISegmentCacheContainer<T>
    {
        /// <summary>
        ///     ��ȡ�������
        /// </summary>
        int Capacity { get; }
        /// <summary>
        ///     ���һ������
        /// </summary>
        /// <param name="key">����Ψһ��ʶ</param>
        /// <param name="obj">��������</param>
        /// <returns>������Ӻ�ı�ʾ</returns>
        bool Add(T key, byte[] obj);
        /// <summary>
        ///     ���һ������
        /// </summary>
        /// <param name="key">����Ψһ��ʶ</param>
        /// <param name="obj">��������</param>
        /// <param name="timeSpan">����ʱ��</param>
        /// <returns>������Ӻ�ı�ʾ</returns>
        bool Add(T key, byte[] obj, TimeSpan timeSpan);
        /// <summary>
        ///     ���һ������
        /// </summary>
        /// <param name="key">����Ψһ��ʶ</param>
        /// <param name="obj">��������</param>
        /// <param name="expireTime">����ʱ��</param>
        /// <returns>������Ӻ�ı�ʾ</returns>
        bool Add(T key, byte[] obj, DateTime expireTime);
        /// <summary>
        ///     ��ȡ����ָ��Ψһ��ʶ�Ļ�������
        /// </summary>
        /// <param name="key">����Ψһ��ʶ</param>
        /// <returns>���ػ�������</returns>
        byte[] Get(T key);
        /// <summary>
        ///     ��鵱ǰ�Ƿ���ھ���ָ��Ψһ��ʶ�Ļ���
        /// </summary>
        /// <param name="key">����Ψһ��ʶ</param>
        /// <returns>�����Ƿ���ڵı�ʶ</returns>
        bool IsExists(T key);
        /// <summary>
        ///     �Ƴ�����ָ��Ψһ��ʾ�Ļ���
        /// </summary>
        /// <param name="key">����Ψһ��ʶ</param>
        void Remove(T key);
        /// <summary>
        ///     �����¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> Expired;
    }
}