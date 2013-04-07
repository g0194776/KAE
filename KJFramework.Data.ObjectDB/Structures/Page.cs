using System.IO.MemoryMappedFiles;

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
        /// <param name="fileId">�������ļ����</param>
        /// <param name="pageId">ҳ����</param>
        /// <param name="mappFile">�ڴ�ӳ���ļ����</param>
        /// <param name="isNew">�Ƿ����½���ҳ��</param>
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
        ///     ��ȡ��ǰҳ��Ψһ���
        /// </summary>
        public uint Id { get; private set; }
        /// <summary>
        ///     ��ȡ��ʹ�õ��ڴ�Ƭ������
        /// </summary>
        public ushort UsedSegmentsCount { get; private set; }

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
        ///     ��ָ�����ݴ��뵱ǰ������
        /// </summary>
        /// <param name="data">Ҫ�洢������</param>
        /// <param name="position">�洢λ��</param>
        /// <returns>���ش洢���</returns>
        public bool Store(byte[] data, StorePosition position)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     ��ʼ����ǰҳ���е������ڴ�Ƭ������
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