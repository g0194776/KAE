using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
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
        /// <param name="indexTable">索引表</param>
        /// <param name="mappedFile">内存映射文件句柄</param>
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
        ///     添加一个类型令牌
        /// </summary>
        /// <param name="fullname">要添加的类型全名称</param>
        /// <returns>返回类型令牌</returns>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
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
        ///     添加一个类型令牌
        /// </summary>
        /// <param name="id">要添加的类型编号</param>
        /// <returns>返回类型令牌</returns>
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
        ///     获取具有指定编号的类型令牌信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回类型令牌</returns>
        public TypeToken? GetToken(ulong id)
        {
            return _indexTable.GetToken(id);
        }

        /// <summary>
        ///     获取或者添加一个类型令牌
        ///     <para>* 如果指定的类型全名未被加入到当前索引表中，则此方法会自动添加这个类型令牌</para>
        /// </summary>
        /// <param name="fullname">要添加的类型全名称</param>
        /// <returns>返回类型令牌</returns>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
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