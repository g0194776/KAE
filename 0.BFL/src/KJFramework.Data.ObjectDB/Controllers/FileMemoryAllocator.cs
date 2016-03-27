using System.IO;
using System.IO.MemoryMappedFiles;

namespace KJFramework.Data.ObjectDB.Controllers
{
    /// <summary>
    ///     文件内存申请器
    /// </summary>
    internal class FileMemoryAllocator : IFileMemoryAllocator
    {
        #region Members

        private FileStream _stream;

        #endregion

        #region Constructor

        /// <summary>
        ///     文件内存申请器
        /// </summary>
        /// <param name="stream">IO流</param>
        public FileMemoryAllocator(FileStream stream)
        {
            _stream = stream;
        }

        #endregion

        #region Implementation of IFileMemoryAllocator

        /// <summary>
        ///     增加指定文件大小
        /// </summary>
        /// <param name="size">文件大小</param>
        public void Alloc(ulong size)
        {
            _stream.SetLength(_stream.Length + (long) size);
            _stream.Flush(true);
        }

        /// <summary>
        ///     创建一个新的内存映射文件句柄
        /// </summary>
        /// <returns>返回一个新的内存映射句柄</returns>
        public MemoryMappedFile NewMappedFile()
        {
            return MemoryMappedFile.CreateFromFile(_stream,
                                                   string.Format("MappedFile::{0}", "TestFile"),
                                                   _stream.Length,
                                                   MemoryMappedFileAccess.ReadWrite, null,
                                                   HandleInheritability.None, true);
        }

        /// <summary>
        ///     刷新数据到磁盘
        /// </summary>
        public void Flush()
        {
            _stream.Flush(true);
        }

        #endregion
    }
}