using System;
using KJFramework.Net.Exception;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     ���ܽڵ�Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public interface IFunctionNode<T>
    {
        /// <summary>
        ///     ��ȡ���ñ�ʾ
        /// </summary>
        bool Enable { get; }
        /// <summary>
        ///     ��ȡΨһ��ֵ
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///   ��ȡ�����ø�������
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <returns>���س�ʼ��״̬</returns>
        /// <exception cref="InitializeFailedException">��ʼ��ʧ��</exception>
        bool Initialize();
    }
}