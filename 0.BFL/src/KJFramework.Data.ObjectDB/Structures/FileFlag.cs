using System;
using KJFramework.Data.ObjectDB.Enums;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     文件头标示
    /// </summary>
    internal class FileFlag : IFileFlag
    {
        #region Members

        private readonly ushort _flag;

        #endregion

        #region Constructor

        /// <summary>
        ///     文件头标示
        /// </summary>
        public FileFlag()
        {
            _flag = 0;
            _flag |= (Environment.Is64BitOperatingSystem ? (ushort)(1 << 15) : (ushort)0);
            _flag |= (ushort)(StoreModes.Full) << 14;
        }

        /// <summary>
        ///     文件头标示
        /// </summary>
        /// <param name="flag">初始化标示</param>
        public FileFlag(ushort flag)
        {
            _flag = flag;
        }

        #endregion

        #region Implementation of IFileFlag

        public ushort GetData()
        {
            return _flag;
        }

        #endregion
    }
}