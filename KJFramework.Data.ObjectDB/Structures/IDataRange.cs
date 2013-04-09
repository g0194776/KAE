namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据范围接口
    /// </summary>
    internal interface IDataRange
    {
        /// <summary>
        ///     获取内部包含的真实数据
        /// </summary>
        /// <returns>返回内部包含的真实数据</returns>
        byte[] GetData();
        /// <summary>
        ///     获取内部数据的真实长度
        /// </summary>
        uint Length { get; }
        /// <summary>
        ///     保存当前内部数据
        /// </summary>
        void Save();
    }
}