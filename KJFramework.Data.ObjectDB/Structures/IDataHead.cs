namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据头接口
    /// </summary>
    internal interface IDataHead
    {
        #region Members

        /// <summary>
        ///     获取或设置数据大小
        /// </summary>
        ushort Size { get; set; }
        /// <summary>
        ///     获取或设置数据存储时间
        /// </summary>
        ulong UpdateTime { get; set; }

        #endregion
    }
}