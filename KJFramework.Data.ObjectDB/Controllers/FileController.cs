using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using KJFramework.Data.ObjectDB.Helpers;
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
        private readonly MemoryMappedFile _mappFile;
        private readonly IFileHeaderController _fileHeaderController;
        private readonly IPageController _pageController;

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
            throw new NotImplementedException();
        }

        /// <summary>
        ///     �洢һ����������
        /// </summary>
        /// <param name="tokenId">���ͱ��</param>
        /// <param name="position">�洢��λ����Ϣ</param>
        /// <param name="data">Ҫ�洢������</param>
        public void Store(ulong tokenId, StorePosition position, byte[] data)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}