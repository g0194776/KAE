using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB.Controllers
{
    /// <summary>
    ///     页面控制器
    /// </summary>
    internal class PageController : IPageController
    {
        #region Constructor

        /// <summary>
        ///     页面控制器
        /// </summary>
        /// <param name="fileId">所属的文件编号</param>
        /// <param name="indexTable">索引表</param>
        /// <param name="mappFile">内存映射文件句柄</param>
        public PageController(ushort fileId, IIndexTable indexTable, MemoryMappedFile mappFile)
        {
            _fileId = fileId;
            _indexTable = indexTable;
            _mappFile = mappFile;
            Initialize();
        }

        #endregion

        #region Members

        private readonly object _lockObj = new object();
        private readonly ushort _fileId;
        private readonly IIndexTable _indexTable;
        private readonly MemoryMappedFile _mappFile;
        private readonly IDictionary<uint, IPage> _pages = new Dictionary<uint, IPage>();
        
        #endregion

        #region Implementation of IPageController

        /// <summary>
        ///     创建一个新的页面
        /// </summary>
        /// <returns>如果创建成功，则返回一个新的页面实例</returns>
        public IPage CreatePage()
        {
            lock (_lockObj)
            {
                IPage page = new Page(_fileId, ++_indexTable.UsedPageCounts, _mappFile, true);
                _pages.Add(page.Id, page);
                return page;
            }
        }

        /// <summary>
        ///     获取一个具有指定编号的页面
        /// </summary>
        /// <param name="pageId">页面编号</param>
        /// <returns>如果存在指定条件的页面，则返回，反之返回null.</returns>
        public IPage GetPageById(uint pageId)
        {
            lock (_lockObj)
            {
                IPage page;
                return _pages.TryGetValue(pageId, out page) ? page : null;
            }
        }

        /// <summary>
        ///     存储二进制数据到指定位置
        /// </summary>
        /// <param name="tokenId">令牌编号</param>
        /// <param name="position">位置信息</param>
        /// <param name="data">要保存的数据</param>
        public bool Store(ulong tokenId, StorePosition position, byte[] data)
        {
            IPage page = GetPageById(position.PageId);
            return page.Store(data, position);
        }

        #endregion

        #region Members

        /// <summary>
        ///     初始化当前文件中的所有页面
        /// </summary>
        private void Initialize()
        {
            if (_indexTable.UsedPageCounts == 0U) return;
        }

        #endregion
    }
}