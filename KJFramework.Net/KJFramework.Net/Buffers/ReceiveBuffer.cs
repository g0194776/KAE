using System;
using System.Collections.Generic;
using KJFramework.Buffers;
using KJFramework.Tracing;

namespace KJFramework.Net.Buffers
{
    /// <summary>
    ///   接收缓冲区
    /// </summary>
    public class ReceiveBuffer : ByteArrayBuffer
    {
        #region 构造函数

        /// <summary>
        ///   接收缓冲区
        /// </summary>
        /// <param name="bufferSize">缓冲区大小</param>
        public ReceiveBuffer(int bufferSize)
            : base(bufferSize)
        { }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ReceiveBuffer));

        #endregion

        #region Overrides of ByteArrayBuffer

        /// <summary>
        ///   第三方用户使用的方法，意在使用自己的方式提取有用的数据
        /// </summary>
        /// <returns/>
        protected override List<byte[]> PickupData(ref int nextOffset, int offset)
        {
            int totalRecLength = offset;
            try
            {
                List<byte[]> bytes = new List<byte[]>();
                while (nextOffset < offset)
                {
                    int length = BitConverter.ToInt32(_buffer, nextOffset);
                    //not long enough.
                    if (totalRecLength - length < 0) return bytes;
                    byte[] newData = new byte[length];
                    System.Buffer.BlockCopy(_buffer, nextOffset, newData, 0, length);
                    nextOffset = nextOffset + length;
                    bytes.Add(newData);
                    totalRecLength -= length;
                }
                return bytes;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return null;
            }
        }

        #endregion
    }
}