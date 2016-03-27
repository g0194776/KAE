using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using KJFramework.Data.ObjectDB.Controllers;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     ����Ƭ��
    ///     <para>* ����Ƭ�εı�ţ��Ǵ�1��ʼ��</para>
    /// </summary>
    [DebuggerDisplay("IsUsed: {IsUsed}, UsedSize: {_head.UsedSize}, StartOffset: {_startOffset}")]
    internal class DataSegment : IDataSegment
    {
        #region Constructor

        /// <summary>
        ///     ����Ƭ��
        ///     <para>* ����Ƭ�εı�ţ��Ǵ�1��ʼ��</para>
        /// </summary>
        /// <param name="allocator">�ļ��ڴ�������</param>
        /// <param name="segmentId">����Ƭ�α��</param>
        /// <param name="startOffset">��ʼƫ��</param>
        public unsafe DataSegment(IFileMemoryAllocator allocator, ushort segmentId, uint startOffset)
        {
            _allocator = allocator;
            _segmentId = segmentId;
            _headOffset = startOffset;
            _startOffset = (uint) (_headOffset + sizeof (SegmentHead));
        }

        #endregion

        #region Members

        private SegmentHead _head;
        private readonly uint _startOffset;
        private readonly uint _headOffset;
        private readonly ushort _segmentId;
        private readonly IFileMemoryAllocator _allocator;

        #endregion

        #region Implementation of IDataSegment

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ������Ƭ���Ƿ��Ѿ���ʹ��
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        ///     ��ȡ��ǰ����Ƭ�εĽ�����
        /// </summary>
        public float Health { get; private set; }

        /// <summary>
        ///     д��һ�����ݷ�Χ
        /// </summary>
        /// <param name="position">����λ��</param>
        /// <param name="data">����</param>
        public unsafe void Write(byte[] data, StorePosition position)
        {
            IDataRange dataRange = new DataRange(_allocator, _startOffset + position.StartOffset, data);
            dataRange.Save();
            _head.UsedSize += (uint) (data.Length + sizeof (DataRangeHead));
            SaveHead();
        }

        /// <summary>
        ///     ��ȡһ�����ݷ�Χ
        /// </summary>
        /// <param name="offset">��ȡ��ʼƫ��</param>
        /// <returns>�������ݷ�Χ</returns>
        public IDataRange Read(ushort offset)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     ��ȡ��ǰ����Ƭ���ڲ����е�����
        /// </summary>
        /// <returns>�������ݷ�Χ�ļ���</returns>
        public unsafe IList<byte[]> ReadAll()
        {
            List<byte[]> data = new List<byte[]>();
            if (_head.UsedSize == 0) return data;
            int segmentDataLength = (int) (Global.SegmentSize - sizeof (SegmentHead));
            using (MemoryMappedFile mappedFile = _allocator.NewMappedFile())
            using (MemoryMappedViewStream stream = mappedFile.CreateViewStream(_startOffset, segmentDataLength))
            {
                byte[] buffer = new byte[_head.UsedSize];
                stream.Read(buffer, 0, buffer.Length);
                int offset = 0;
                fixed (byte* pByte = buffer)
                {
                    do
                    {
                        DataRangeHead* rangeHead = (DataRangeHead*) (pByte + offset);
                        if (rangeHead->Length == 0U) break;
                        offset += sizeof (DataRangeHead);
                        byte[] actualData = new byte[rangeHead->Length];
                        Buffer.BlockCopy(buffer, offset, actualData, 0, actualData.Length);
                        offset += (int)rangeHead->Length;
                        data.Add(actualData);
                    } while (true);
                }
            }
            return data;
        }

        /// <summary>
        ///     �����ڲ�������
        /// </summary>
        public void Arrange()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     �����ڴ�Ƭ��ʣ������
        /// </summary>
        /// <returns>�����ڲ�ʣ������</returns>
        public uint CalcRemaining()
        {
            return Global.SegmentSize - _head.UsedSize;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     ��ʼ���ڲ���Ϣ
        /// </summary>
        /// <param name="mappedFile">�ڴ�ӳ���ļ����</param>
        public unsafe void Initialize(MemoryMappedFile mappedFile)
        {
            using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(_headOffset, sizeof(SegmentHead)))
                accessor.Read(0U, out _head);
            //reset flag.
            if (_head.UsedSize == 0U) IsUsed = false;
        }

        /// <summary>
        ///     ����ͷ����Ϣ
        /// </summary>
        private unsafe void SaveHead()
        {
            using (MemoryMappedFile mappedFile = _allocator.NewMappedFile())
            using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(_headOffset, sizeof(SegmentHead)))
            {
                accessor.Write(0U, ref _head);
                accessor.Flush();
            }
        }

        #endregion
    }
}