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
    ///     文件头管理器
    /// </summary>
    internal class FileHeaderController : IFileHeaderController
    {
        #region Constructor

        /// <summary>
        ///     文件头管理器
        /// </summary>
        /// <param name="filename">本地数据库文件全路径</param>
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
        ///     添加一个类型令牌
        /// </summary>
        /// <param name="fullname">要添加的类型全名称</param>
        /// <returns>返回类型令牌</returns>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
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
        ///     获取具有指定编号的类型令牌信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回类型令牌</returns>
        public TypeToken? GetToken(ulong id)
        {
            return _indexTable.GetToken(id);
        }

        /// <summary>
        ///     获取类型令牌信息集合
        /// </summary>
        public IEnumerable<TypeToken> GetTokens()
        {
            return _indexTable.GetTokens();
        }

        /// <summary>
        ///     提交变更请求
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