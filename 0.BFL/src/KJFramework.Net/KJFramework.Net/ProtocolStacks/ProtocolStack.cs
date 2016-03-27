using System.Collections.Generic;
using KJFramework.Net.Exception;

namespace KJFramework.Net.ProtocolStacks
{
    /// <summary>
    ///   Э��ջ�����࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class ProtocolStack : IProtocolStack
    {
        #region Implementation of IProtocolStack<T>

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <returns>���س�ʼ���Ľ��</returns>
        /// <exception cref="InitializeFailedException">��ʼ��ʧ��</exception>
        public abstract bool Initialize();
        /// <summary>
        ///     ����Ԫ����
        /// </summary>
        /// <param name="data">Ԫ����</param>
        /// <returns>�����ܷ������һ����ʾ</returns>
        public abstract List<T> Parse<T>(byte[] data);
        /// <summary>
        ///     ����Ԫ����
        /// </summary>
        /// <param name="data">��BUFF����</param>
        /// <param name="offset">����ƫ����</param>
        /// <param name="count">���ó���</param>
        /// <returns>�����ܷ������һ����ʾ</returns>
        public abstract List<T> Parse<T>(byte[] data, int offset, int count);
        /// <summary>
        ///     ��һ����Ϣת��Ϊ2������ʽ
        /// </summary>
        /// <param name="message">��Ҫת������Ϣ</param>
        /// <returns>����ת�����2����</returns>
        public abstract byte[] ConvertToBytes(object message);
        /// <summary>
        ///     ��һ����Ϣת��Ϊ����ְ�����������
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <param name="maxSize">���Ƭ�������</param>
        /// <returns>����ת�����2���Ƽ���</returns>
        public virtual List<byte[]> ConvertMultiMessage(object message, int maxSize)
        {
            return null;
        }

        #endregion
    }
}