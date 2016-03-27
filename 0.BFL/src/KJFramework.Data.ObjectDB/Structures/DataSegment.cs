using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using KJFramework.Data.ObjectDB.Controllers;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据片段
    ///     <para>* 数据片段的编号，是从1开始的</para>
    /// </summary>
    [DebuggerDisplay("IsUsed: {IsUsed}, UsedSize: {_head.UsedSize}, StartOffset: {_startOffset}")]
    internal class DataSegment : IDataSegment
    {
        #region Constructor

        /// <summary>
        ///     数据片段
        ///     <para>* 数据片段的编号，是从1开始的</para>
        /// </summary>
        /// <param name="allocator">文件内存申请器</param>
        /// <param name="segmentId">数据片段编号</param>
        /// <param name="startOffset">起始偏移</param>
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
        ///     获取或设置一个值，该值标示了当前的数据片段是否已经被使用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        ///     获取当前数据片段的健康度
        /// </summary>
        public float Health { get; private set; }

        /// <summary>
        ///     写入一个数据范围
        /// </summary>
        /// <param name="position">保存位置</param>
        /// <param name="data">数据</param>
        public unsafe void Write(byte[] data, StorePosition position)
        {
            IDataRange dataRange = new DataRange(_allocator, _startOffset + position.StartOffset, data);
            dataRange.Save();
            _head.UsedSize += (uint) (data.Length + sizeof (DataRangeHead));
            SaveHead();
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
        ///     整理内部的数据
        /// </summary>
        public void Arrange()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     计算内存片段剩余容量
        /// </summary>
        /// <returns>返回内部剩余容量</returns>
        public uint CalcRemaining()
        {
            return Global.SegmentSize - _head.UsedSize;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     初始化内部信息
        /// </summary>
        /// <param name="mappedFile">内存映射文件句柄</param>
        public unsafe void Initialize(MemoryMappedFile mappedFile)
        {
            using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(_headOffset, sizeof(SegmentHead)))
                accessor.Read(0U, out _head);
            //reset flag.
            if (_head.UsedSize == 0U) IsUsed = false;
        }

        /// <summary>
        ///     保存头部信息
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