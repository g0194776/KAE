using System;
using System.Runtime.InteropServices;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据区域的头
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct DataRangeHead
    {
        #region Members

        /// <summary>
        ///     真实数据长度
        /// </summary>
        [FieldOffset(0)]
        public uint Length;
        /// <summary>
        ///     最后更新时间
        /// </summary>
        [FieldOffset(5)]
        public DateTime LastUpdateTime;

        #endregion
    }
}