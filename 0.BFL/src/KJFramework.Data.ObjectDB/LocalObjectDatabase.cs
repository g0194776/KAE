using System;
using System.Collections.Generic;
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
            _fileController.Store(obj, tokenId, position);
        }

        /// <summary>
        ///     获取保存在数据库中所有指定类型的对象集合
        /// </summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <returns>返回相关对象集合, 如果无法查找到有效对象则返回null.</returns>
        public IList<T> Get<T>() where T : new()
        {
            ulong tokenId = UtilityHelper.CalcTokenId(typeof(T).FullName);
            return _fileController.Get<T>(tokenId);
        }

        #endregion
    }
}