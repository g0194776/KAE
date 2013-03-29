using System;
using KJFramework.Data.ObjectDB.Controllers;
using KJFramework.Data.ObjectDB.Helpers;
using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB
{
    /// <summary>
    ///     ���ض������ݿ�
    /// </summary>
    internal class LocalObjectDatabase : IObjectDatabase
    {
        #region Members

        private readonly IFileController _fileController;

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
            _fileController = new FileController(filename);
        }

        #endregion

        #region Implementation of IObjectDatabase

        /// <summary>
        ///     �洢һ������
        /// </summary>
        /// <param name="obj">��Ҫ���洢�Ķ���</param>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
        public void Store(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            ulong tokenId = UtilityHelper.CalcTokenId(obj.GetType().FullName);
            StorePosition position;
            if (!_fileController.EnsureSize(tokenId, 1024U, out position))
                throw new System.Exception("��������");
            _fileController.Store(tokenId, position, new byte[] { });
        }

        #endregion
    }
}