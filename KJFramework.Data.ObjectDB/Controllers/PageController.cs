using System;
using System.Collections.Generic;
using KJFramework.Data.ObjectDB.Structures;
using KJFramework.Tracing;

namespace KJFramework.Data.ObjectDB.Controllers
{
    /// <summary>
    ///     ҳ�������
    /// </summary>
    internal class PageController : IPageController
    {
        #region Constructor

        /// <summary>
        ///     ҳ�������
        /// </summary>
        /// <param name="fileId">�������ļ����</param>
        /// <param name="fileMemoryAllocator">�ļ��ڴ�������</param>
        /// <param name="indexTable">������</param>
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
        ///     ����һ���µ�ҳ��
        /// </summary>
        /// <returns>��������ɹ����򷵻�һ���µ�ҳ��ʵ��</returns>
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
        ///     ��ȡһ������ָ����ŵ�ҳ��
        /// </summary>
        /// <param name="pageId">ҳ����</param>
        /// <returns>�������ָ��������ҳ�棬�򷵻أ���֮����null.</returns>
        /// <exception cref="InvalidOperationException">�Ƿ���ҳ����</exception>
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
        ///     �洢���������ݵ�ָ��λ��
        /// </summary>
        /// <param name="tokenId">���Ʊ��</param>
        /// <param name="position">λ����Ϣ</param>
        /// <param name="data">Ҫ���������</param>
        public bool Store(ulong tokenId, StorePosition position, byte[] data)
        {
            IPage page = GetPageById(position.PageId);
            return page.Store(data, position);
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ʼ����ǰ�ļ��е�����ҳ��
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