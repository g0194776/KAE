using System;

namespace KJFramework.Compress
{
    /// <summary>
    ///     ѹ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ICompressor : IDisposable
    {
        /// <summary>
        ///     ѹ��һ���������ֽ�����
        /// </summary>
        /// <param name="data">��ѹ�����ֽ�����</param>
        /// <returns>����ѹ���������</returns>
        byte[] Compress(byte[] data);
        /// <summary>
        ///     ��ѹ��һ���������ֽ�����
        /// </summary>
        /// <param name="data">����ѹ�����ֽ�����</param>
        /// <returns>���ؽ�ѹ���������</returns>
        byte[] DeCompress(byte[] data); 
    }
}