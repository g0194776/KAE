using System;

namespace KJFramework.TimingJob.Compressors
{
    /// <summary>
    ///    数据压缩器接口
    /// </summary>
    public interface ICompressor
    {
        /// <summary>
        ///    压缩一段字节数据
        /// </summary>
        /// <param name="data">即将被压缩的数据</param>
        /// <returns>返回压缩后的数据</returns>
        byte[] Compress(byte[] data);
        /// <summary>
        ///   解压缩一段字节数组
        /// </summary>
        /// <param name="data">即将被解压缩的数据</param>
        /// <param name="index">数据起始位置</param>
        /// <param name="byteCount">使用的数据长度</param>
        /// <returns>返回解压缩后的数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        byte[] Decompress(byte[] data, int index, int byteCount); 
    }
}