using System;
using System.Collections.Generic;
using KJFramework.Buffers;
using KJFramework.Tracing;

namespace KJFramework.Net.Buffers
{
    /// <summary>
    ///   ���ջ�����
    /// </summary>
    public class ReceiveBuffer : ByteArrayBuffer
    {
        #region ���캯��

        /// <summary>
        ///   ���ջ�����
        /// </summary>
        /// <param name="bufferSize">��������С</param>
        public ReceiveBuffer(int bufferSize)
            : base(bufferSize)
        { }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ReceiveBuffer));

        #endregion

        #region Overrides of ByteArrayBuffer

        /// <summary>
        ///   �������û�ʹ�õķ���������ʹ���Լ��ķ�ʽ��ȡ���õ�����
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