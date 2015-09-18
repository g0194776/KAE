using System;
using KJFramework.Compress;
using KJFramework.TimingJob.Enums;

namespace KJFramework.TimingJob.Encoders
{
    /// <summary>
    ///    数据编码器
    /// </summary>
    public class DefaultBinaryDataEncoder : IDataEncoder
    {
        #region Constructor.

        /// <summary>
        ///    数据编码器
        /// </summary>
        /// <param name="maximumUnZipSize">
        ///    最大允许的压缩前大小
        ///    <para>* 如果数据超过了这个大小将会自动被压缩</para>
        /// </param>
        public DefaultBinaryDataEncoder(int maximumUnZipSize)
        {
            _maximumUnZipSize = maximumUnZipSize;
        }

        #endregion

        #region Members.

        private readonly int _maximumUnZipSize;
        private static ICompressor _compressor = new DeflateCompressor();

        #endregion

        #region Methods.

        /// <summary>
        ///    将一段二进制数据进行编码
        /// </summary>
        /// <param name="data">需要编码的二进制数据</param>
        /// <returns>返回编码后的二进制数据</returns>
        public byte[] Encode(byte[] data)
        {
            if (data == null) return null;
            byte[] newData;
            bool ziped = false;
            if (data.Length > _maximumUnZipSize)
            {
                ziped = true;
                data = _compressor.Compress(data);
            }
            newData = new byte[data.Length + 2];
            newData[0] = (byte)(ziped ? 0x01 : 0x00);
            newData[1] = (byte)(ziped ? CompressTypes.Deflate : 0x00);
            Buffer.BlockCopy(data, 0, newData, 2, data.Length);
            return newData;
        }

        #endregion
    }
}