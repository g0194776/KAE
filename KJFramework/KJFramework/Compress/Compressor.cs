using System;

namespace KJFramework.Compress
{
    /// <summary>
    ///     压缩器抽象父类，提供了相关的基本操作。
    /// </summary>
    public abstract class Compressor : ICompressor
    {
        #region 析构函数

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
        ///     压缩一个二进制字节数组
        /// </summary>
        /// <param name="data">被压缩的字节数组</param>
        /// <returns>返回压缩后的数据</returns>
        public abstract byte[] Compress(byte[] data);
        /// <summary>
        ///     解压缩一个二进制字节数组
        /// </summary>
        /// <param name="data">被解压缩的字节数组</param>
        /// <returns>返回解压缩后的数据</returns>
        public abstract byte[] DeCompress(byte[] data);

        #endregion
    }
}