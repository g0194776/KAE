using System;
using System.Collections.Generic;
using System.IO;
using KJFramework.Data.ObjectDB.Exceptions;
using KJFramework.Data.ObjectDB.Helpers;
using KJFramework.Data.ObjectDB.Hooks;
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
        private readonly FileStream _fileStream;
        private readonly IFileHeaderController _fileHeaderController;
        private readonly IPageController _pageController;
        private readonly IFileMemoryAllocator _allocator;
        private readonly byte _fileId;

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
            _fileStream = new FileStream(filename, FileMode.OpenOrCreate);
            _allocator = new FileMemoryAllocator(_fileStream);
            _fileHeaderController = new FileHeaderController(_allocator, _indexTable);
            _indexTable = !File.Exists(filename) ? DBFileHelper.CreateNew(_fileStream) : DBFileHelper.ReadIndexTable(_allocator);
            _pageController = new PageController(_fileId, _allocator, _indexTable);
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
            TypeToken token = _fileHeaderController.GetOrAddToken(id);
            IPage page;
            //never store current object type before.
            if (token.Positions.Count == 0)
            {
                page = _pageController.CreatePage();
                remaining = new StorePosition {PageId = page.Id, FileId = _fileId};
                return true;
            }
            PositionData position = token.Positions.GetLastPosition();
            page = _pageController.GetPageById((uint)(position.StartPageId + position.PageCount) - 1U);
            if (page.EnsureSize(size, out remaining)) return true;
            page = _pageController.CreatePage();
            remaining = new StorePosition {PageId = page.Id, FileId = _fileId};
            return true;
        }

        /// <summary>
        ///     存储一个对象数据
        /// </summary>
        /// <param name="obj">要保存的对象</param>
        /// <param name="tokenId">类型编号</param>
        /// <param name="position">存储的位置信息</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="HookProcessException">数据钩子处理失败</exception>
        public void Store(object obj, ulong tokenId, StorePosition position)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            //fake data.
            byte[] data = new byte[669];
            DataProcessHookManager.Process(obj.GetType(), data);
            _pageController.Store(tokenId, position, data);
        }

        /// <summary>
        ///     获取保存在数据库中所有指定类型的对象集合
        /// </summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="tokenId">类型令牌编号</param>
        /// <returns>返回相关对象集合, 如果无法查找到有效对象则返回null.</returns>
        public IList<T> Get<T>(ulong tokenId) where T : new()
        {
            TypeToken? nullableToken = _indexTable.GetToken(tokenId);
            if (nullableToken == null) return null;
            TypeToken token = (TypeToken)nullableToken;
            IList<PositionData> positions = token.Positions.GetPositionDataByFileId(_fileId);
            if (positions == null) return null;
            IList<T> objects = new List<T>();
            foreach (PositionData position in positions)
            {
                for (ushort i = position.StartPageId; i < position.PageCount; i++)
                {
                    IPage page = _pageController.GetPageById(i);
                    if (page == null) throw new InvalidOperationException(string.Format("#Illegal page id #{0}, beacause it havn't exist yet!", i));
                    IList<byte[]> avaData = page.GetAllData();
                    if (avaData == null) continue;
                    //fake implementation.
                    foreach (byte[] data in avaData) objects.Add(new T());
                }
            }
            return objects;
        }

        #endregion
    }
}