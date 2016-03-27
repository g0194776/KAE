namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     位置信息数据
    /// </summary>
    internal struct PositionData
    {
        #region Members

        /// <summary>
        ///     文件编号
        /// </summary>
        public byte FileId;
        /// <summary>
        ///     当前编号文件下的起始页编号
        /// </summary>
        public ushort StartPageId;
        /// <summary>
        ///     使用的页数量
        /// </summary>
        public ushort PageCount;

        #endregion
    }
}