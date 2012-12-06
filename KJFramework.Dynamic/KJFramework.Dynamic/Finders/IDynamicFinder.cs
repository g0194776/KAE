using System;

namespace KJFramework.Dynamic.Finders
{
    /// <summary>
    ///     ��̬���������ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public interface IDynamicFinder<T> : IDisposable
    {
        /// <summary>
        ///     ����һ��·�������еĶ�̬���������
        /// </summary>
        /// <param name="path">����·��</param>
        /// <returns>���س����������ڵ���Ϣ����</returns>
        T Execute(String path);
    }
}