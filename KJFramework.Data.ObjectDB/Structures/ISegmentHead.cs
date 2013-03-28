namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据片段头接口
    /// </summary>
    internal interface ISegmentHead
    {
        /// <summary>
        ///     获取当前数据片段的总长度
        /// </summary>
        ushort TotalSize { get; }
        /// <summary>
        ///     获取当前数据片段已使用的长度
        /// </summary>
        ushort UsedSize { get; }
    }
}