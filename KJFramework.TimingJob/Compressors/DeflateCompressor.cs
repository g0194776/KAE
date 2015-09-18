using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace KJFramework.TimingJob.Compressors
{
    /// <summary>
    ///     Deflate数据压缩器
    /// </summary>
    public class DeflateCompressor : ICompressor
    {
        #region Overrides of Compressor

        /// <summary>
        ///    压缩一段字节数据
        /// </summary>
        /// <param name="data">即将被压缩的数据</param>
        /// <returns>返回压缩后的数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public byte[] Compress(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            using (MemoryStream ms = new MemoryStream())
            {
                using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Compress))
                {
                    ds.Write(data, 0, data.Length);
                    ds.Flush();
                    ds.Close();
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        ///   解压缩一段字节数组
        /// </summary>
        /// <param name="data">即将被解压缩的数据</param>
        /// <param name="index">数据起始位置</param>
        /// <param name="byteCount">使用的数据长度</param>
        /// <returns>返回解压缩后的数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public byte[] Decompress(byte[] data, int index, int byteCount)
        {
            if (data == null) throw new ArgumentNullException("data");
            const int bufferSize = 256;
            byte[] tempArray = new byte[bufferSize];
            List<byte[]> tempList = new List<byte[]>();
            int count, length = 0;
            using (MemoryStream ms = new MemoryStream(data, index, byteCount))
            {
                using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    while ((count = ds.Read(tempArray, 0, bufferSize)) > 0)
                    {
                        if (count == bufferSize)
                        {
                            tempList.Add(tempArray);
                            tempArray = new byte[bufferSize];
                        }
                        else
                        {
                            byte[] temp = new byte[count];
                            Array.Copy(tempArray, 0, temp, 0, count);
                            tempList.Add(temp);
                        }
                        length += count;
                    }
                    byte[] retVal = new byte[length];
                    count = 0;
                    foreach (byte[] temp in tempList)
                    {
                        Array.Copy(temp, 0, retVal, count, temp.Length);
                        count += temp.Length;
                    }
                    return retVal;
                }
            }
        }

        #endregion
    }
}