using System;
using KJFramework.Data.ObjectDB.Controllers;
using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB
{
    /// <summary>
    ///     本地对象数据库
    /// </summary>
    internal class LocalObjectDatabase : IObjectDatabase
    {
        #region Members

        private readonly string _filename;
        private readonly IIndexTable _indexTable;
        private readonly IFileHeaderController _fileHeaderController;

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
            _filename = filename;
            _fileHeaderController = new FileHeaderController(filename);
        }

        #endregion
    }
}