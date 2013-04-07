namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     页面接口
    ///     <para>* 页面的编号，是从1开始的</para>
    /// </summary>
    internal interface IPage
    {
        /// <summary>
        ///     获取当前页面唯一编号
        /// </summary>
        uint Id { get; }
        /// <summary>
        ///     获取已使用的内存片段数量
        /// </summary>
        ushort UsedSegmentsCount { get; }
        /// <summary>
        ///     查询当前页面是否能够容纳指定大小的数据
        /// </summary>
        /// <param name="size">数据大小</param>
        /// <param name="remaining">剩余大小</param>
        /// <returns>返回一个查询后的结果</returns>
        bool EnsureSize(uint size, out StorePosition remaining);
        /// <summary>
        ///     将指定数据存入当前月面中
        /// </summary>
        /// <param name="data">要存储的数据</param>
        /// <param name="position">存储位置</param>
        /// <returns>返回存储结果</returns>
        bool Store(byte[] data, StorePosition position);
    }
}