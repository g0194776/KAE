namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据片段接口
    /// </summary>
    internal interface IDataSegment
    {
        /// <summary>
        ///     获取数据片段头
        /// </summary>
        ISegmentHead Head { get; }
        /// <summary>
        ///     获取当前数据片段的健康度
        /// </summary>
        float Health { get; }
        /// <summary>
        ///     写入一个数据范围
        /// </summary>
        /// <param name="dataRage">数据范围</param>
        void Write(IDataRange dataRage);
        /// <summary>
        ///     读取一个数据范围
        /// </summary>
        /// <param name="offset">读取起始偏移</param>
        /// <returns>返回数据范围</returns>
        IDataRange Read(ushort offset);
        /// <summary>
        ///     读取当前数据片段内部所有的数据
        /// </summary>
        /// <returns>返回数据范围的集合</returns>
        IDataRange[] ReadAll();
        /// <summary>
        ///     整理内部的数据
        /// </summary>
        void Arrange();
    }
}