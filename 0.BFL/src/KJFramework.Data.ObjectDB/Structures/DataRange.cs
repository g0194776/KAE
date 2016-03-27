using System;
using System.IO.MemoryMappedFiles;
using KJFramework.Data.ObjectDB.Controllers;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     ���ݷ�Χ 
    /// </summary>
    internal class DataRange : IDataRange
    {
        #region Constructor

        /// <summary>
        ///     ���ݷ�Χ 
        /// </summary>
        /// <param name="allocator">�ļ��ڴ�������</param>
        /// <param name="offset">Ҫ������ڴ�ƫ��</param>
        /// <param name="data">Ҫ���������</param>
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
        ///     ���ݷ�Χ 
        /// </summary>
        /// <param name="allocator">�ļ��ڴ�������</param>
        /// <param name="offset">��ǰ���ݷ�Χ���ڴ��е���ʼƫ��</param>
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
        ///     ���ļ��ڴ��г�ʼ�����ݷ�Χ�ڲ���һЩ��Ϣ
        /// </summary>
        private unsafe void Initialize()
        {
            using (MemoryMappedFile mappedFile = _allocator.NewMappedFile())
            using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(_offset, sizeof(DataRangeHead)))
                accessor.Read(0U, out _head);
        }

        /// <summary>
        ///     ���ڴ��ж�ȡ���ݲ�����
        /// </summary>
        /// <returns></returns>
        private byte[] ReadDataFromMemory()
        {
            return null;
        }

        #endregion

        #region Implementation of IDataRange

        /// <summary>
        ///     ��ȡ�ڲ���������ʵ����
        /// </summary>
        /// <returns>�����ڲ���������ʵ����</returns>
        public byte[] GetData()
        {
            return _data ?? ReadDataFromMemory();
        }

        /// <summary>
        ///     ��ȡ�ڲ����ݵ���ʵ����
        /// </summary>
        public uint Length
        {
            get { return _head.Length; }
        }

        /// <summary>
        ///     ���浱ǰ�ڲ�����
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