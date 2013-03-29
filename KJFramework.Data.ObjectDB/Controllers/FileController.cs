using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using KJFramework.Data.ObjectDB.Helpers;
using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB.Controllers
{
    /// <summary>
    ///     文件控制器
    /// </summary>
    internal class FileController : IFileController
    {
        #region Members

        private readonly string _filename;
        private readonly IIndexTable _indexTable;
        private readonly MemoryMappedFile _mappFile;
        private readonly IFileHeaderController _fileHeaderController;
        private readonly IPageController _pageController;

        #endregion

        #region Constructor

        /// <summary>
        ///     文件控制器
        /// </summary>
        /// <param name="filename">文件地址全路径</param>
        public FileController(string filename)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException("filename");
            _filename = filename;
            if (!File.Exists(filename))
            {
                _indexTable = DBFileHelper.CreateNew(filename);
                _mappFile = MemoryMappedFile.CreateFromFile(filename);
            }
            else
            {
                _mappFile = MemoryMappedFile.CreateFromFile(filename);
                _indexTable = DBFileHelper.ReadIndexTable(_mappFile);
            }
            _fileHeaderController = new FileHeaderController(_indexTable, _mappFile);
            
        }

        #endregion

        #region Implementation of IFileController

        /// <summary>
        ///     获取一个值，该值标示了当前的文件是否为数据库主文件
        /// </summary>
        public bool IsMainFile { get; private set; }

        /// <summary>
        ///     确保当前
        /// </summary>
        /// <param name="id">类型编号</param>
        /// <param name="size">数据大小</param>
        /// <param name="remaining">出去本次需要计算的数据大小后，文件内部的剩余大小</param>
        /// <returns>如果返回true, 则证明当前文件内部可以包含本次大小的数据</returns>
        public bool EnsureSize(ulong id, uint size, out StorePosition remaining)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     存储一个对象数据
        /// </summary>
        /// <param name="tokenId">类型编号</param>
        /// <param name="position">存储的位置信息</param>
        /// <param name="data">要存储的数据</param>
        public void Store(ulong tokenId, StorePosition position, byte[] data)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}