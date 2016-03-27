using System;

namespace KJFramework.Messages.Helpers
{
    /// <summary>
    ///     记录了框架内设置的值类型默认值
    /// </summary>
    public static class DefaultValue
    {
        /// <summary>
        ///     bool类型默认值
        /// </summary>
        public static readonly bool Boolean = false;
        /// <summary>
        ///     char类型默认值
        /// </summary>
        public static readonly char Char = '\0';
        /// <summary>
        ///     byte类型默认值
        /// </summary>
        public static readonly byte Byte = 0x00;
        /// <summary>
        ///     sbyte类型默认值
        /// </summary>
        public static readonly sbyte SByte = 0x00;
        /// <summary>
        ///     decimal类型默认值
        /// </summary>
        public static readonly decimal Decimal = 0;
        /// <summary>
        ///     short类型默认值
        /// </summary>
        public static readonly short Int16 = 0;
        /// <summary>
        ///     ushort类型默认值
        /// </summary>
        public static readonly ushort UInt16 = 0;
        /// <summary>
        ///     float类型默认值
        /// </summary>
        public static readonly float Float = 0;
        /// <summary>
        ///     int类型默认值
        /// </summary>
        public static readonly int Int32 = 0;
        /// <summary>
        ///     uint类型默认值
        /// </summary>
        public static readonly uint UInt32 = 0U;
        /// <summary>
        ///     ulong类型默认值
        /// </summary>
        public static readonly ulong UInt64 = 0UL;
        /// <summary>
        ///     long类型默认值
        /// </summary>
        public static readonly long Int64 = 0L;
        /// <summary>
        ///     double类型默认值
        /// </summary>
        public static readonly double Double = 0D;
        /// <summary>
        ///     DateTime类型默认值
        /// </summary>
        public static readonly DateTime DateTime = DateTime.MinValue;
        /// <summary>
        ///     IntPtr类型默认值
        /// </summary>
        public static readonly IntPtr IntPtr = IntPtr.Zero;
        /// <summary>
        ///     Guid类型默认值
        /// </summary>
        public static readonly Guid Guid = Guid.Empty;
        /// <summary>
        ///     TimeSpan类型默认值
        /// </summary>
        public static readonly TimeSpan TimeSpan = new TimeSpan(0, 0, 0);
    }
}