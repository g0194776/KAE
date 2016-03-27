using System;
using KJFramework.Services;

namespace KJFramework.Installers
{
    /// <summary>
    ///     ����װ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">��װ����</typeparam>
    public interface IServiceInstaller<T> : IDisposable
    {
        /// <summary>
        ///     ��װ����
        /// </summary>
        /// <param name="obj">��װ����</param>
        /// <returns>���ذ�װ��״̬</returns>
        IWindowsService Install(T obj);
        /// <summary>
        ///     ж�ط���
        /// </summary>
        /// <param name="name">������</param>
        /// <returns>����д��״̬</returns>
        bool UnInstall(String name);
    }
}