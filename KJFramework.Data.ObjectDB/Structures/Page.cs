using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using KJFramework.Data.ObjectDB.Controllers;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     ����ҳ��
    ///     <para>* ҳ��ı�ţ��Ǵ�1��ʼ��</para>
    /// </summary>
    internal class Page : IPage
    {
        #region Constructor

        /// <summary>
        ///     ����ҳ��
        /// </summary>
        /// <param name="allocator">�ļ��ڴ�������</param>
        /// <param name="fileId">�������ļ����</param>
        /// <param name="pageId">ҳ����</param>
        /// <param name="isNew">�Ƿ����½���ҳ��</param>
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
        ///     ��ȡ��ǰҳ��Ψһ���
        /// </summary>
        public uint Id
        {
            get { return _head.Id; }
            private set { _head.Id = value; }
        }

        /// <summary>
        ///     ��ȡ��ʹ�õ��ڴ�Ƭ������
        /// </summary>
        public ushort UsedSegmentsCount
        {
            get { return _head.UsedSegmentCount; }
            private set { _head.UsedSegmentCount = value; }
        }

        /// <summary>
        ///     ��ѯ��ǰҳ���Ƿ��ܹ�����ָ����С������
        /// </summary>
        /// <param name="size">���ݴ�С</param>
        /// <param name="remaining">ʣ���С</param>
        /// <returns>����һ����ѯ��Ľ��</returns>
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
        ///     ��ָ�����ݴ��뵱ǰ������
        /// </summary>
        /// <param name="data">Ҫ�洢������</param>
        /// <param name="position">�洢λ��</param>
        /// <returns>���ش洢���</returns>
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
        ///     ��ȡ�ڲ����е��ѱ�������
        /// </summary>
        /// <returns>�����ڲ������ѱ������ݣ����û���κα���������򷵻�null.</returns>
        public IList<byte[]> GetAllData()
        {
            if (_head.UsedSegmentCount == 0) return null;
            List<byte[]> data = new List<byte[]>();
            for (int i = 0; i < _head.UsedSegmentCount; i++) data.AddRange(_segments[i].ReadAll());
            return data;
        }

        /// <summary>
        ///     ����ҳ��ʣ������
        /// </summary>
        /// <returns>�����ڲ�ʣ������</returns>
        public uint CalcRemaining()
        {
            return 0U;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     ��ʼ����ǰҳ���е������ڴ�Ƭ������
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
        ///     ����ͷ����Ϣ
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