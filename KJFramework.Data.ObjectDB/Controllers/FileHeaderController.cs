using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
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
        /// <param name="indexTable">������</param>
        /// <param name="mappedFile">�ڴ�ӳ���ļ����</param>
        public FileHeaderController(IIndexTable indexTable, MemoryMappedFile mappedFile)
        {
            _indexTable = indexTable;
            _mappedFile = mappedFile;
        }

        #endregion

        #region Members

        private readonly IIndexTable _indexTable;
        private readonly MemoryMappedFile _mappedFile;
        private readonly MemoryMappedFile _mappFile;

        #endregion

        #region Implementation of IPageController

        /// <summary>
        ///     ���һ����������
        /// </summary>
        /// <param name="fullname">Ҫ��ӵ�����ȫ����</param>
        /// <returns>������������</returns>
        /// <exception cref="System.ArgumentNullException">��������Ϊ��</exception>
        public TypeToken AddToken(string fullname)
        {
            if (string.IsNullOrEmpty(fullname)) throw new ArgumentNullException("fullname");
            TypeToken? token;
            ulong tokenId = UtilityHelper.CalcTokenId(fullname);
            if ((token = _indexTable.GetToken(tokenId)) != null) return (TypeToken)token;
            TypeToken newToken = new TypeToken();
            newToken.Id = tokenId;
            newToken.IsTemporaryStore = false;
            _indexTable.AddToken(newToken);
            return newToken;
        }

        /// <summary>
        ///     ���һ����������
        /// </summary>
        /// <param name="id">Ҫ��ӵ����ͱ��</param>
        /// <returns>������������</returns>
        public TypeToken AddToken(ulong id)
        {
            TypeToken? token;
            if ((token = _indexTable.GetToken(id)) != null) return (TypeToken)token;
            TypeToken newToken = new TypeToken();
            newToken.Id = id;
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
        ///     ��ȡ�������һ����������
        ///     <para>* ���ָ��������ȫ��δ�����뵽��ǰ�������У���˷������Զ���������������</para>
        /// </summary>
        /// <param name="fullname">Ҫ��ӵ�����ȫ����</param>
        /// <returns>������������</returns>
        /// <exception cref="System.ArgumentNullException">��������Ϊ��</exception>
        public TypeToken GetOrAddToken(string fullname)
        {
            if (string.IsNullOrEmpty(fullname)) throw new ArgumentNullException("fullname");
            ulong tokenId = UtilityHelper.CalcTokenId(fullname);
            TypeToken? token;
            TypeToken actualToken;
            if ((token = GetToken(tokenId)) == null)
                actualToken = AddToken(tokenId);
            else actualToken = (TypeToken) token;
            return actualToken;
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