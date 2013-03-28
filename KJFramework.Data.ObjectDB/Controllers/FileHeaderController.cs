using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Security.Cryptography;
using System.Text;
using KJFramework.Data.ObjectDB.Helpers;
using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB.Controllers
{
    /// <summary>
    ///     �ļ�ͷ������
    /// </summary>
    internal class FileHeaderController : IFileHeaderController
    {
        #region Constructor

        /// <summary>
        ///     �ļ�ͷ������
        /// </summary>
        /// <param name="filename">�������ݿ��ļ�ȫ·��</param>
        public FileHeaderController(string filename)
        {
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
        }

        #endregion

        #region Members

        private readonly IIndexTable _indexTable;
        private readonly MemoryMappedFile _mappFile;
        private static readonly MD5 _md5Provider = new MD5CryptoServiceProvider();

        #endregion

        #region Implementation of IPageController

        /// <summary>
        ///     ���һ����������
        /// </summary>
        /// <param name="fullname">Ҫ��ӵ�����ȫ����</param>
        /// <returns>������������</returns>
        /// <exception cref="System.ArgumentNullException">��������Ϊ��</exception>
        public unsafe TypeToken AddToken(string fullname)
        {
            if (string.IsNullOrEmpty(fullname)) throw new ArgumentNullException("fullname");
            byte[] hashData = _md5Provider.ComputeHash(Encoding.UTF8.GetBytes(fullname));
            ulong tokenId;
            TypeToken? token;
            fixed (byte* pByte = hashData) tokenId = *(ulong*)(pByte + 8);
            if ((token = _indexTable.GetToken(tokenId)) != null) return (TypeToken)token;
            TypeToken newToken = new TypeToken();
            newToken.Id = tokenId;
            newToken.IsTemporaryStore = false;
            _indexTable.AddToken(newToken);
            return newToken;
        }

        /// <summary>
        ///     ��ȡ����ָ����ŵ�����������Ϣ
        /// </summary>
        /// <param name="id">���</param>
        /// <returns>������������</returns>
        public TypeToken? GetToken(ulong id)
        {
            return _indexTable.GetToken(id);
        }

        /// <summary>
        ///     ��ȡ����������Ϣ����
        /// </summary>
        public IEnumerable<TypeToken> GetTokens()
        {
            return _indexTable.GetTokens();
        }

        /// <summary>
        ///     �ύ�������
        /// </summary>
        public void Commit()
        {
            DBFileHelper.StoreIndexTable(_mappFile, _indexTable);
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (_mappFile != null) _mappFile.Dispose();
        }

        #endregion
    }
}