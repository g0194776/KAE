using System;

namespace KJFramework.Compress
{
    /// <summary>
    ///     ѹ���������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class Compressor : ICompressor
    {
        #region ��������

        ~Compressor()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of ICompressor

        /// <summary>
        ///     ѹ��һ���������ֽ�����
        /// </summary>
        /// <param name="data">��ѹ�����ֽ�����</param>
        /// <returns>����ѹ���������</returns>
        public abstract byte[] Compress(byte[] data);
        /// <summary>
        ///     ��ѹ��һ���������ֽ�����
        /// </summary>
        /// <param name="data">����ѹ�����ֽ�����</param>
        /// <returns>���ؽ�ѹ���������</returns>
        public abstract byte[] DeCompress(byte[] data);

        #endregion
    }
}