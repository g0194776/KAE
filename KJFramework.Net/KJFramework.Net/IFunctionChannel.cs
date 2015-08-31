using System;

namespace KJFramework.Net
{
    /// <summary>
    ///     ����ͨ��Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IFunctionChannel
    {
        /// <summary>
        ///     ��ȡΨһ��ʶ
        /// </summary>
        Guid Key { get; }
        /// <summary>
        ///     ����ָ�����󣬲����ش����Ľ��
        /// </summary>
        /// <param name="obj">����Ķ���</param>
        /// <param name="isSuccess">�Ƿ���ɹ��ı�ʾ</param>
        /// <returns>�ش����Ľ��</returns>
        object Process(object obj, out bool isSuccess);
    }

    /// <summary>
    ///     ����ͨ��Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">����Ķ�������</typeparam>
    public interface IFunctionChannel<T>
    {
        /// <summary>
        ///     ��ȡΨһ��ʶ
        /// </summary>
        Guid Key { get; }
        /// <summary>
        ///     ����ָ�����󣬲����ش����Ľ��
        /// </summary>
        /// <param name="obj">����Ķ���</param>
        /// <param name="isSuccess">�Ƿ���ɹ��ı�ʾ</param>
        /// <returns>�ش����Ľ��</returns>
        T Process(T obj, out bool isSuccess);
    }
}