using System;
using System.IO.MemoryMappedFiles;
using KJFramework.Data.ObjectDB.Controllers;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     数据范围 
    /// </summary>
    internal class DataRange : IDataRange
    {
        #region Constructor

        /// <summary>
        ///     数据范围 
        /// </summary>
        /// <param name="allocator">文件内存申请器</param>
        /// <param name="offset">要保存的内存偏移</param>
        /// <param name="data">要保存的数据</param>
        public DataRange(IFileMemoryAllocator allocator, uint offset, byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            _allocator = allocator;
            _offset = offset;
            _data = data;
            _head.Length = (uint) data.Length;
            _head.LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        ///     数据范围 
        /// </summary>
        /// <param name="allocator">文件内存申请器</param>
        /// <param name="offset">当前数据范围在内存中的起始偏移</param>
        public DataRange(IFileMemoryAllocator allocator, uint offset)
        {
            _allocator = allocator;
            _offset = offset;
            Initialize();
        }

        #endregion

        #region Members

        private uint _offset;
        private DataRangeHead _head;
        private readonly IFileMemoryAllocator _allocator;
        private readonly byte[] _data;

        #endregion

        #region Methods

        /// <summary>
        ///     从文件内存中初始化数据范围内部的一些信息
        /// </summary>
        private unsafe void Initialize()
        {
            using (MemoryMappedFile mappedFile = _allocator.NewMappedFile())
            using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(_offset, sizeof(DataRangeHead)))
                accessor.Read(0U, out _head);
        }

        /// <summary>
        ///     从内存中读取数据并返回
        /// </summary>
        /// <returns></returns>
        private byte[] ReadDataFromMemory()
        {
            return null;
        }

        #endregion

        #region Implementation of IDataRange

        /// <summary>
        ///     获取内部包含的真实数据
        /// </summary>
        /// <returns>返回内部包含的真实数据</returns>
        public byte[] GetData()
        {
            return _data ?? ReadDataFromMemory();
        }

        /// <summary>
        ///     获取内部数据的真实长度
        /// </summary>
        public uint Length
        {
            get { return _head.Length; }
        }

        /// <summary>
        ///     保存当前内部数据
        /// </summary>
        public unsafe void Save()
        {
            using (MemoryMappedFile mappedFile = _allocator.NewMappedFile())
            using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(_offset, _data.Length + sizeof(DataRangeHead)))
            {
                //write data range head.
                accessor.Write(0U, ref _head);
                //write actual data.
                accessor.WriteArray(sizeof (DataRangeHead), _data, 0, _data.Length);
                accessor.Flush();
            }
        }

        #endregion
    }
}