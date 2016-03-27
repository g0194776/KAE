using KJFramework.Messages.Types;

namespace KJFramework.Messages.Helpers
{
    /// <summary>
    ///     类型转换相关帮助器
    /// </summary>
    public static class BitConvertHelper
    {
        #region Method

        /// <summary>Returns the specified 32-bit signed integer value as an array of bytes.</summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="memory">a memory will be fill.</param>
        /// <param name="offset">fill begin offset.</param>
        public unsafe static void GetBytes(int value, byte[] memory, int offset)
        {
            fixed (byte* ptr = &memory[offset]) *(int*)ptr = value;
        }


        /// <summary>Returns the specified 64-bit signed integer value as an array of bytes.</summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="memory">a memory will be fill.</param>
        /// <param name="offset">fill begin offset.</param>
        public unsafe static void GetBytes(long value, byte[] memory, int offset)
        {
            fixed (byte* ptr = &memory[offset]) *(long*)ptr = value;
        }

        /// <summary>Returns the specified 16-bit signed integer value as an array of bytes.</summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="memory">a memory will be fill.</param>
        /// <param name="offset">fill begin offset.</param>
        public unsafe static void GetBytes(short value, byte[] memory, int offset)
        {
            fixed (byte* ptr = &memory[offset]) *(short*)ptr = value;
        }

        /// <summary>
        ///     将一个BitFlag转换为内部的字节值
        /// </summary>
        /// <param name="bits">BitFlag</param>
        /// <returns>返回内部的字节值</returns>
        internal static byte ConvertToByte(BitFlag bits)
        {
            byte result = 0;
            for (byte i = 0; i < 8; i++)
                if (bits[i]) result |= (byte)(1 << i);
            return result;
        }

        #endregion
    }
}