namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据范围接口
    /// </summary>
    internal interface IDataRange
    {
        /// <summary>
        ///     获取数据头
        /// </summary>
        IDataHead Head { get; }
        /// <summary>
        ///     获取数据的开始位置
        /// </summary>
        uint StartPosition { get; }
    }
}