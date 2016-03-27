using System.Runtime.InteropServices;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     页面头
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct PageHead
    {
        #region Members

        /// <summary>
        ///     页面编号
        /// </summary>
        [FieldOffset(0)]
        public uint Id;
        /// <summary>
        ///     已使用的内存片段数量
        /// </summary>
        [FieldOffset(5)]
        public ushort UsedSegmentCount;

        #endregion
    }
}