using System;

namespace KJFramework.Compress
{
    /// <summary>
    ///     压缩器元接口，提供了相关的基本操作。
    /// </summary>
    public interface ICompressor : IDisposable
    {
        /// <summary>
        ///     压缩一个二进制字节数组
        /// </summary>
        /// <param name="data">被压缩的字节数组</param>
        /// <returns>返回压缩后的数据</returns>
        byte[] Compress(byte[] data);
        /// <summary>
        ///     解压缩一个二进制字节数组
        /// </summary>
        /// <param name="data">被解压缩的字节数组</param>
        /// <returns>返回解压缩后的数据</returns>
        byte[] DeCompress(byte[] data); 
    }
}