using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据片段接口
    /// </summary>
    internal interface IDataSegment
    {
        /// <summary>
        ///     获取或设置一个值，该值标示了当前的数据片段是否已经被使用
        /// </summary>
        bool IsUsed { get; set; }
        /// <summary>
        ///     获取当前数据片段的健康度
        /// </summary>
        float Health { get; }
        /// <summary>
        ///     写入一个数据范围
        /// </summary>
        /// <param name="position">保存位置</param>
        /// <param name="data">数据</param>
        void Write(byte[] data, StorePosition position);
        /// <summary>
        ///     初始化内部信息
        /// </summary>
        /// <param name="mappedFile">内存映射文件句柄</param>
        void Initialize(MemoryMappedFile mappedFile);
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
        IList<byte[]> ReadAll();
        /// <summary>
        ///     整理内部的数据
        /// </summary>
        void Arrange();
        /// <summary>
        ///     计算内存片段剩余容量
        /// </summary>
        /// <returns>返回内部剩余容量</returns>
        uint CalcRemaining();
    }
}