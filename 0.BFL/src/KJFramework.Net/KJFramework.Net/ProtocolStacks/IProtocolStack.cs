using System.Collections.Generic;
using KJFramework.Net.Exception;

namespace KJFramework.Net.ProtocolStacks
{
    /// <summary>
    ///     Э��ջԪ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IProtocolStack
    {
        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <returns>���س�ʼ���Ľ��</returns>
        /// <exception cref="InitializeFailedException">��ʼ��ʧ��</exception>
        bool Initialize();
        /// <summary>
        ///     ����Ԫ����
        /// </summary>
        /// <param name="data">Ԫ����</param>
        /// <returns>�����ܷ������һ����ʾ</returns>
        List<T> Parse<T>(byte[] data);
        /// <summary>
        ///     ����Ԫ����
        /// </summary>
        /// <param name="data">��BUFF����</param>
        /// <param name="offset">����ƫ����</param>
        /// <param name="count">���ó���</param>
        /// <returns>�����ܷ������һ����ʾ</returns>
        List<T> Parse<T>(byte[] data, int offset, int count);
        /// <summary>
        ///     ��һ����Ϣת��Ϊ2������ʽ
        /// </summary>
        /// <param name="message">��Ҫת������Ϣ</param>
        /// <returns>����ת�����2����</returns>
        byte[] ConvertToBytes(object message);
        /// <summary>
        ///     ��һ����Ϣת��Ϊ����ְ�����������
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <param name="maxSize">���Ƭ�������</param>
        /// <returns>����ת�����2���Ƽ���</returns>
        List<byte[]> ConvertMultiMessage(object message, int maxSize);
    }
}