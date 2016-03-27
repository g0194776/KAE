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
    ///     �ļ�������
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
        ///     �ļ�������
        /// </summary>
        /// <param name="filename">�ļ���ַȫ·��</param>
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
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ���ļ��Ƿ�Ϊ���ݿ����ļ�
        /// </summary>
        public bool IsMainFile { get; private set; }

        /// <summary>
        ///     ȷ����ǰ
        /// </summary>
        /// <param name="id">���ͱ��</param>
        /// <param name="size">���ݴ�С</param>
        /// <param name="remaining">��ȥ������Ҫ��������ݴ�С���ļ��ڲ���ʣ���С</param>
        /// <returns>�������true, ��֤����ǰ�ļ��ڲ����԰������δ�С������</returns>
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
        ///     �洢һ����������
        /// </summary>
        /// <param name="obj">Ҫ����Ķ���</param>
        /// <param name="tokenId">���ͱ��</param>
        /// <param name="position">�洢��λ����Ϣ</param>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
        /// <exception cref="HookProcessException">���ݹ��Ӵ���ʧ��</exception>
        public void Store(object obj, ulong tokenId, StorePosition position)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            //fake data.
            byte[] data = new byte[669];
            DataProcessHookManager.Process(obj.GetType(), data);
            _pageController.Store(tokenId, position, data);
        }

        /// <summary>
        ///     ��ȡ���������ݿ�������ָ�����͵Ķ��󼯺�
        /// </summary>
        /// <typeparam name="T">ָ����������</typeparam>
        /// <param name="tokenId">�������Ʊ��</param>
        /// <returns>������ض��󼯺�, ����޷����ҵ���Ч�����򷵻�null.</returns>
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