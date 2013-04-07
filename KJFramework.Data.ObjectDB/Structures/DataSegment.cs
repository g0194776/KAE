using System.IO.MemoryMappedFiles;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据片段
    ///     <para>* 数据片段的编号，是从1开始的</para>
    /// </summary>
    internal class DataSegment : IDataSegment
    {
        #region Constructor

        /// <summary>
        ///     数据片段
        ///     <para>* 数据片段的编号，是从1开始的</para>
        /// </summary>
        /// <param name="mappFile">内存映射文件句柄</param>
        /// <param name="segmentId">数据片段编号</param>
        /// <param name="startOffset">起始偏移</param>
        public DataSegment(MemoryMappedFile mappFile, ushort segmentId, uint startOffset)
        {
            _mappFile = mappFile;
            _segmentId = segmentId;
            _startOffset = startOffset*segmentId;
        }

        #endregion

        #region Members

        private readonly uint _startOffset;
        private readonly ushort _segmentId;
        private readonly MemoryMappedFile _mappFile;

        #endregion

        #region Implementation of IDataSegment

        /// <summary>
        ///     获取数据片段头
        /// </summary>
        public ISegmentHead Head { get; private set; }

        /// <summary>
        ///     获取当前数据片段的健康度
        /// </summary>
        public float Health { get; private set; }

        /// <summary>
        ///     写入一个数据范围
        /// </summary>
        /// <param name="dataRage">数据范围</param>
        public void Write(IDataRange dataRage)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     读取一个数据范围
        /// </summary>
        /// <param name="offset">读取起始偏移</param>
        /// <returns>返回数据范围</returns>
        public IDataRange Read(ushort offset)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     读取当前数据片段内部所有的数据
        /// </summary>
        /// <returns>返回数据范围的集合</returns>
        public IDataRange[] ReadAll()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     整理内部的数据
        /// </summary>
        public void Arrange()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}