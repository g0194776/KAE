using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace KJFramework.Compress
{
    /// <summary>
    ///     Deflate压缩器的基础实现，提供了相关的基本操作。
    /// </summary>
    public class DeflateCompressor : Compressor
    {
        #region Overrides of Compressor

        /// <summary>
        ///     压缩一个二进制字节数组
        /// </summary>
        /// <param name="data">被压缩的字节数组</param>
        /// <returns>返回压缩后的数据</returns>
        public override byte[] Compress(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
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
        ///     解压缩一个二进制字节数组
        /// </summary>
        /// <param name="data">被解压缩的字节数组</param>
        /// <returns>返回解压缩后的数据</returns>
        public override byte[] DeCompress(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            const int bufferSize = 256;
            byte[] tempArray = new byte[bufferSize];
            List<byte[]> tempList = new List<byte[]>();
            int count = 0, length = 0;
            using (MemoryStream ms = new MemoryStream(data))
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