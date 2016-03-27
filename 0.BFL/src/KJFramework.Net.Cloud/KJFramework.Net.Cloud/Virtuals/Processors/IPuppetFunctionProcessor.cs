using System;

namespace KJFramework.Net.Cloud.Virtuals.Processors
{
    /// <summary>
    ///     ���ܹ��ܴ�����Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IPuppetFunctionProcessor
    {
        /// <summary>
        ///     ��ȡΨһ���
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="target">��������</param>
        /// <returns>���س�ʼ����״̬</returns>
        bool Initialize<T>(T target);
        /// <summary>
        ///     �ͷŵ�ǰ�Ŀ��ܹ��ܴ���
        /// </summary>
        void Release();
    }
}