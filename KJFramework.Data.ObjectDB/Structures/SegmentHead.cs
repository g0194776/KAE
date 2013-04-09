using System.Runtime.InteropServices;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据片段头
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct SegmentHead
    {
        /// <summary>
        ///     已使用的长度
        /// </summary>
        [FieldOffset(0)]
        public uint UsedSize;
    }
}