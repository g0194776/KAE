using System;
using System.Collections.Generic;
using KJFramework.Net.Exception;

namespace KJFramework.Net.ProtocolStacks
{
    /// <summary>
    ///   Э��ջ�����࣬�ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ�����͡�</typeparam>
    public abstract class ProtocolStack<T> : IProtocolStack<T>
    {
        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

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
        public abstract List<T> Parse(byte[] data);

        /// <summary>
        ///     ����Ԫ����
        /// </summary>
        /// <param name="data">��BUFF����</param>
        /// <param name="offset">����ƫ����</param>
        /// <param name="count">���ó���</param>
        /// <returns>�����ܷ������һ����ʾ</returns>
        public virtual List<T> Parse(byte[] data, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     ��һ����Ϣת��Ϊ2������ʽ
        /// </summary>
        /// <param name="message">��Ҫת������Ϣ</param>
        /// <returns>����ת�����2����</returns>
        public abstract byte[] ConvertToBytes(T message);
        /// <summary>
        ///     ��һ����Ϣת��Ϊ����ְ�����������
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <param name="maxSize">���Ƭ�������</param>
        /// <returns>����ת�����2���Ƽ���</returns>
        public virtual List<byte[]> ConvertMultiMessage(T message, int maxSize)
        {
            return null;
        }

        #endregion
    }
}