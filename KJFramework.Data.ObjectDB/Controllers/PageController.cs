using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using KJFramework.Data.ObjectDB.Structures;

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
        /// <param name="indexTable">������</param>
        /// <param name="mappFile">�ڴ�ӳ���ļ����</param>
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
        ///     ����һ���µ�ҳ��
        /// </summary>
        /// <returns>��������ɹ����򷵻�һ���µ�ҳ��ʵ��</returns>
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
        ///     ��ȡһ������ָ����ŵ�ҳ��
        /// </summary>
        /// <param name="pageId">ҳ����</param>
        /// <returns>�������ָ��������ҳ�棬�򷵻أ���֮����null.</returns>
        public IPage GetPageById(uint pageId)
        {
            lock (_lockObj)
            {
                IPage page;
                return _pages.TryGetValue(pageId, out page) ? page : null;
            }
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
    }
}