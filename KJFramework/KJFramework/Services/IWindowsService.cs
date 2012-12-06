using System;

namespace KJFramework.Services
{
    /// <summary>
    ///     WINDOWS����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IWindowsService : IDisposable
    {
        /// <summary>
        ///     ��������
        /// </summary>
        /// <returns>���ؿ�����״̬</returns>
        bool Start();
        /// <summary>
        ///     ֹͣ����
        /// </summary>
        /// <returns>����ֹͣ��״̬</returns>
        bool Stop();
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        String ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ��������ʾ����
        /// </summary>
        String DisplayName { get; set; }
        /// <summary>
        ///     ��ȡ�����÷����ִ���ļ�·��
        /// </summary>
        String FilePath { get; set; }
        /// <summary>
        ///     ��ȡ�����÷����ִ���ļ�Ŀ¼
        /// </summary>
        String DirectoryPath { get; set; }
    }
}