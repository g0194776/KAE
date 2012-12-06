using System;
using KJFramework.EventArgs;

namespace KJFramework.Net.Cloud.Processors
{
    /// <summary>
    ///     ���ܴ�����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public interface IFunctionProcessor<T> : IDisposable
    {
        /// <summary>
        ///   ��ȡ�����ø�������
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     ��ȡΨһ��ʾ
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     ����һ��������Ϣ
        /// </summary>
        /// <param name="id">����ͨ����ʾ</param>
        /// <param name="message">������Ϣ</param>
        /// <returns>
        ///     ���ػ�����Ϣ
        ///     <para>* �������Ϊnull, ��֤��û�з�����Ϣ��</para>
        /// </returns>
        T Process(Guid id, T message);
        /// <summary>
        ///     ����������Ϣ�ɹ��¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> ProcessSuccessfully;
        /// <summary>
        ///     ����������Ϣʧ���¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> ProcessFailed;
    }
}