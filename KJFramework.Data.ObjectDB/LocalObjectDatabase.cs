using System;
using KJFramework.Data.ObjectDB.Controllers;
using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB
{
    /// <summary>
    ///     ���ض������ݿ�
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
        ///     ���ض������ݿ�
        /// </summary>
        /// <param name="filename">�������ݿ��ļ�ȫ·��</param>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
        public LocalObjectDatabase(string filename)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException("filename");
            _filename = filename;
            _fileHeaderController = new FileHeaderController(filename);
        }

        #endregion
    }
}