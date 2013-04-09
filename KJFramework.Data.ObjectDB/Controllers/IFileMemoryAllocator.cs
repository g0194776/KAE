using System.IO.MemoryMappedFiles;

namespace KJFramework.Data.ObjectDB.Controllers
{
    /// <summary>
    ///     文件内存申请器接口
    /// </summary>
    internal interface IFileMemoryAllocator
    {
        /// <summary>
        ///     增加指定文件大小
        /// </summary>
        /// <param name="size">文件大小</param>
        void Alloc(ulong size);
        /// <summary>
        ///     创建一个新的内存映射文件句柄
        /// </summary>
        /// <returns>返回一个新的内存映射句柄</returns>
        MemoryMappedFile NewMappedFile();
        /// <summary>
        ///     刷新数据到磁盘
        /// </summary>
        void Flush();
    }
}