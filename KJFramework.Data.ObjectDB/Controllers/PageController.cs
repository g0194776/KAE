using System;
using System.Collections.Generic;
using KJFramework.Data.ObjectDB.Structures;
using KJFramework.Tracing;

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
        /// <param name="fileMemoryAllocator">文件内存申请器</param>
        /// <param name="indexTable">索引表</param>
        public PageController(ushort fileId, IFileMemoryAllocator fileMemoryAllocator, IIndexTable indexTable)
        {
            _fileId = fileId;
            _indexTable = indexTable;
            _allocator = fileMemoryAllocator;
            Initialize();
        }

        #endregion

        #region Members

        private readonly object _lockObj = new object();
        private readonly ushort _fileId;
        private readonly IIndexTable _indexTable;
        private readonly IDictionary<uint, IPage> _pages = new Dictionary<uint, IPage>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (PageController));
        private IFileMemoryAllocator _allocator;
        
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
                _allocator.Alloc(Global.ServerPageSize);
                IPage page = new Page(_allocator, _fileId, ++_indexTable.UsedPageCounts, true);
                _pages.Add(page.Id, page);
                return page;
            }
        }

        /// <summary>
        ///     获取一个具有指定编号的页面
        /// </summary>
        /// <param name="pageId">页面编号</param>
        /// <returns>如果存在指定条件的页面，则返回，反之返回null.</returns>
        /// <exception cref="InvalidOperationException">非法的页面编号</exception>
        public IPage GetPageById(uint pageId)
        {
            IPage page;
            System.Exception exception = null;
            lock (_lockObj)
            {
                if (_pages.TryGetValue(pageId, out page)) return page;
                try
                {
                    page = new Page(_allocator, _fileId, pageId, false);
                    _pages.Add(pageId, page);
                }
                catch (System.Exception ex)
                {
                    _tracing.Error(ex, null);
                    exception = ex;
                }
            }
            if (exception != null) throw exception;
            return page;
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

        #region Methods


        #endregion
    }
}