using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using KJFramework.Data.ObjectDB.Controllers;

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
        /// <param name="allocator">文件内存申请器</param>
        /// <param name="fileId">所属的文件编号</param>
        /// <param name="pageId">页面编号</param>
        /// <param name="isNew">是否是新建的页面</param>
        public unsafe Page(IFileMemoryAllocator allocator, ushort fileId, uint pageId, bool isNew)
        {
            Id = pageId;
            _allocator = allocator;
            _fileId = fileId;
            _pageHeadOffset = Global.HeaderBoundary + Global.ServerPageSize*(pageId - 1);
            _startOffset = _pageHeadOffset + (uint) sizeof (PageHead);
            _segments = new IDataSegment[Global.SegmentsPerPage];
            if (isNew)
            {
                for (ushort i = 1; i <= Global.SegmentsPerPage; i++)
                    _segments[i - 1] = new DataSegment(_allocator, i, (uint) (_startOffset + ((i - 1)*Global.SegmentSize)));
                using (MemoryMappedFile mappedFile = allocator.NewMappedFile())
                using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(_pageHeadOffset, sizeof(PageHead)))
                {
                    accessor.Write(0L, ref _head);
                    accessor.Flush();
                }
            }
            else Initialize();
        }

        #endregion

        #region Members

        private readonly ushort _fileId;
        private readonly uint _startOffset;
        private readonly uint _pageHeadOffset;
        private readonly IDataSegment[] _segments;
        private readonly IFileMemoryAllocator _allocator;
        private PageHead _head;

        #endregion

        #region Implementation of IPage

        /// <summary>
        ///     获取当前页面唯一编号
        /// </summary>
        public uint Id
        {
            get { return _head.Id; }
            private set { _head.Id = value; }
        }

        /// <summary>
        ///     获取已使用的内存片段数量
        /// </summary>
        public ushort UsedSegmentsCount
        {
            get { return _head.UsedSegmentCount; }
            private set { _head.UsedSegmentCount = value; }
        }

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
                uint remainingSize = segment.CalcRemaining();
                if (remainingSize >= size)
                {
                    remaining = new StorePosition { FileId = _fileId, PageId = Id, SegmentId = segmentId, StartOffset = Global.SegmentSize - remainingSize };
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
            IDataSegment segment = _segments[position.SegmentId];
            segment.Write(data, position);
            if(!segment.IsUsed)
            {
                segment.IsUsed = true;
                _head.UsedSegmentCount++;
                SaveHead();
            }
            return true;
        }

        /// <summary>
        ///     获取内部所有的已保存数据
        /// </summary>
        /// <returns>返回内部所有已保存数据，如果没有任何保存的数据则返回null.</returns>
        public IList<byte[]> GetAllData()
        {
            if (_head.UsedSegmentCount == 0) return null;
            List<byte[]> data = new List<byte[]>();
            for (int i = 0; i < _head.UsedSegmentCount; i++) data.AddRange(_segments[i].ReadAll());
            return data;
        }

        /// <summary>
        ///     计算页面剩余容量
        /// </summary>
        /// <returns>返回内部剩余容量</returns>
        public uint CalcRemaining()
        {
            return 0U;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     初始化当前页面中的所有内存片段数据
        /// </summary>
        private unsafe void Initialize()
        {
            using (MemoryMappedFile mappedFile = _allocator.NewMappedFile())
            using (MemoryMappedViewStream stream = mappedFile.CreateViewStream(_pageHeadOffset, Global.ServerPageSize))
            {
                byte[] data = new byte[Global.ServerPageSize];
                //read data from memory buffer.
                stream.Read(data, 0, data.Length);
                //read page head.
                fixed (byte* pData = data) _head = *(PageHead*)pData;
                for (ushort i = 1; i <= Global.SegmentsPerPage; i++)
                {
                    DataSegment segment = new DataSegment(_allocator, i, (uint)(_startOffset + ((i - 1) * Global.SegmentSize))) { IsUsed = (_head.UsedSegmentCount != 0 && _head.UsedSegmentCount >= i) };
                    if (segment.IsUsed) segment.Initialize(mappedFile);
                    _segments[i - 1] = segment;
                }
            }
        }

        /// <summary>
        ///     保存头部信息
        /// </summary>
        private unsafe void SaveHead()
        {
            using (MemoryMappedFile mappedFile = _allocator.NewMappedFile())
            using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(_pageHeadOffset, sizeof(PageHead)))
            {
                accessor.Write(0U, ref _head);
                accessor.Flush();
            }
        }

        #endregion
    }
}