using System.IO.MemoryMappedFiles;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据页面
    ///     <para>* 页面的编号，是从1开始的</para>
    /// </summary>
    internal class Page : IPage
    {
        #region Constructor

        /// <summary>
        ///     数据页面
        /// </summary>
        /// <param name="fileId">所属的文件编号</param>
        /// <param name="pageId">页面编号</param>
        /// <param name="mappFile">内存映射文件句柄</param>
        /// <param name="isNew">是否是新建的页面</param>
        public Page(ushort fileId, uint pageId, MemoryMappedFile mappFile, bool isNew)
        {
            Id = pageId;
            _fileId = fileId;
            _mappFile = mappFile;
            _startOffset = Global.HeaderBoundary + Global.ServerPageSize * pageId;
            _segments = new IDataSegment[Global.SegmentsPerPage];
            if (isNew)
            {
                for (ushort i = 1; i <= Global.SegmentsPerPage; i++)
                    _segments[i - 1] = new DataSegment(_mappFile, i, _startOffset + i*Global.SegmentSize);
            }
            else Initialize();
        }

        #endregion

        #region Members

        private readonly ushort _fileId;
        private readonly uint _startOffset;
        private readonly MemoryMappedFile _mappFile;
        private readonly IDataSegment[] _segments;

        #endregion

        #region Implementation of IPage

        /// <summary>
        ///     获取当前页面唯一编号
        /// </summary>
        public uint Id { get; private set; }
        /// <summary>
        ///     获取已使用的内存片段数量
        /// </summary>
        public ushort UsedSegmentsCount { get; private set; }

        /// <summary>
        ///     查询当前页面是否能够容纳指定大小的数据
        /// </summary>
        /// <param name="size">数据大小</param>
        /// <param name="remaining">剩余大小</param>
        /// <returns>返回一个查询后的结果</returns>
        public bool EnsureSize(uint size, out StorePosition remaining)
        {
            ushort usedSegmentsCount = UsedSegmentsCount;
            if(usedSegmentsCount <= Global.SegmentsPerPage)
            {
                ushort segmentId = (ushort) (usedSegmentsCount == 0 ? 0 : usedSegmentsCount - 1);
                IDataSegment segment = _segments[segmentId];
                ushort remainingSize = (ushort) (segment.Head.TotalSize - segment.Head.UsedSize);
                if (remainingSize >= size)
                {
                    remaining = new StorePosition { FileId = _fileId, PageId = Id, SegmentId = segmentId, StartOffset = remainingSize };
                    return true;
                }
                if (usedSegmentsCount < Global.SegmentsPerPage)
                {
                    remaining = new StorePosition { FileId = _fileId, PageId = Id, SegmentId = (ushort) (segmentId + 1), StartOffset = 0 };
                    return true;
                }
            }
            remaining = new StorePosition();
            return false;
        }

        /// <summary>
        ///     将指定数据存入当前月面中
        /// </summary>
        /// <param name="data">要存储的数据</param>
        /// <param name="position">存储位置</param>
        /// <returns>返回存储结果</returns>
        public bool Store(byte[] data, StorePosition position)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     初始化当前页面中的所有内存片段数据
        /// </summary>
        private void Initialize()
        {
            using (MemoryMappedViewStream stream = _mappFile.CreateViewStream(_startOffset, _startOffset+Global.SegmentsPerPage*Global.SegmentSize))
            {
                byte[] data = new byte[Global.SegmentsPerPage*Global.SegmentSize];
                //read data from memory buffer.
                stream.Read(data, 0, data.Length);
                for (ushort i = 1; i <= Global.SegmentsPerPage; i++)
                    _segments[i - 1] = new DataSegment(_mappFile, i, _startOffset + i * Global.SegmentSize);
            }
        }

        #endregion
    }
}