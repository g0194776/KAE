using System;
using KJFramework.Net.Cloud.Exceptions;
using KJFramework.Net.Cloud.Processors;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     ������Ϣ�������ƵĹ��ܽڵ�
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public interface IMessageFunctionNode<T> : IFunctionNode<T>
    {
        /// <summary>
        ///     ��ȡ����������
        /// </summary>
        int ProcessorCount { get; }/// <summary>
        ///     ̽��һ����Ϣ�Ƿ��ܱ���ǰ������������
        /// </summary>
        /// <param name="id">����ͨ����ʾ</param>
        /// <param name="message">̽�����Ϣ</param>
        /// <returns>����һ������֧�ֵĴ�����</returns>
        IFunctionProcessor<T> CanProcess(Guid id, T message);
        /// <summary>
        ///     ��ȡ����ָ����ʾ�Ĺ��ܴ�����
        /// </summary>
        /// <param name="id">���ܴ�������ʾ</param>
        /// <returns>���ع��ܴ�����</returns>
        IFunctionProcessor<T> GetProcessor(Guid id);
        /// <summary>
        ///     ����һ����Ϣ
        /// </summary>
        /// <param name="id">����ͨ����ʾ</param>
        /// <param name="message">Ҫ�������Ϣ</param>
        /// <returns>
        ///     ���ط�����Ϣ
        ///     <para>* �������null, ����Ϊû�з�����Ϣ��</para>
        /// </returns>
        /// <exception cref="NotSupportedProcessException">��֧�ֵĲ���</exception>
        /// <exception cref="FunctionProcessorNotEnableException">���ܴ�����δ����</exception>
        T Process(Guid id, T message);
    }
}