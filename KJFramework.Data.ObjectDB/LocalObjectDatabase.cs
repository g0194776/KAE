using System;
using KJFramework.Data.ObjectDB.Controllers;
using KJFramework.Data.ObjectDB.Helpers;
using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB
{
    /// <summary>
    ///     本地对象数据库
    /// </summary>
    internal class LocalObjectDatabase : IObjectDatabase
    {
        #region Members

        private readonly IFileController _fileController;

        #endregion

        #region Constructor

        /// <summary>
        ///     本地对象数据库
        /// </summary>
        /// <param name="filename">本地数据库文件全路径</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public LocalObjectDatabase(string filename)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException("filename");
            _fileController = new FileController(filename);
        }

        #endregion

        #region Implementation of IObjectDatabase

        /// <summary>
        ///     存储一个对象
        /// </summary>
        /// <param name="obj">需要被存储的对象</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public void Store(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            ulong tokenId = UtilityHelper.CalcTokenId(obj.GetType().FullName);
            StorePosition position;
            if (!_fileController.EnsureSize(tokenId, 1024U, out position))
                throw new System.Exception("容量不足");
            _fileController.Store(tokenId, position, new byte[] { });
        }

        #endregion
    }
}