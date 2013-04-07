namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     此结构用于存储数据将要保存到的位置信息
    /// </summary>
    internal struct StorePosition
    {
        /// <summary>
        ///     文件编号
        /// </summary>
        public ushort FileId;
        /// <summary>
        ///     页面编号
        /// </summary>
        public uint PageId;
        /// <summary>
        ///     数据段编号
        /// </summary>
        public ushort SegmentId;
        /// <summary>
        ///     其实数据写入偏移
        /// </summary>
        public uint StartOffset;
    }
}