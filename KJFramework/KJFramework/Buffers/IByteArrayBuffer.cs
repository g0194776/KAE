using System.Collections.Generic;

namespace KJFramework.Buffers
{
    /// <summary>
    ///   �ֽ����黺�������������ڽ�����Ϣ��ʱ������յ����ֽ�����
    /// </summary>
    public interface IByteArrayBuffer
    {
        /// <summary>
        ///   ��ȡ��������С
        ///   <para>* �������Ĵ�СӦ������Ϊ������������ * ������</para>
        /// </summary>
        int BufferSize { get; }
        /// <summary>
        ///   ��ӻ���
        /// </summary>
        /// <param name="data">���յ�������</param>
        /// <returns>������ȡ�������</returns>
        List<byte[]> Add(byte[] data);
        /// <summary>
        ///   ��ջ�����
        /// </summary>
        void Clear();
        /// <summary>
        ///   ��ȡ������һ��ֵ����ֵ��ʾ�����������������ʱ���Ƿ��Զ����û�����
        /// </summary>
        bool AutoClear { get; set; }
    }
}