using System;
using KJFramework.TimingJob.Compressors;
using KJFramework.TimingJob.Enums;

namespace KJFramework.TimingJob.Decoders
{
    /// <summary>
    ///    二进制数据解码器
    /// </summary>
    public class DefaultBinaryDataDecoder : IDataDecoder
    {
        #region Members.

        private static ICompressor _compressor = new DeflateCompressor();

        #endregion

        #region Methods.

        /// <summary>
        ///    将一段二进制数据按照指定的格式进行解码并返回内部的真正数据
        ///    <para>* 数据解析格式为: ZIP Flag(1 byte) + ZIP Algorithm(1 byte) + Data(N bytes)</para>
        /// </summary>
        /// <param name="data">需要解码的二进制数据</param>
        /// <returns>返回内部真正的数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="FormatException">数据长度或者格式不符合解码要求</exception>
        public byte[] Decode(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            if(data.Length < 3) throw new FormatException("#Illegal data length.");
            byte[] realData;
            if (data[0] == 0)
            {
                realData = new byte[data.Length - 2];
                Buffer.BlockCopy(data, 2, realData, 0, realData.Length);
                return realData;
            }
            switch ((CompressTypes) data[1])
            {
                case CompressTypes.Deflate:
	            {
					return _compressor.Decompress(data, 2, data.Length - 2);
	            }
                default: throw new FormatException("Unsupported ZIP algorithm!");
            }
        }


        #endregion
    }
}