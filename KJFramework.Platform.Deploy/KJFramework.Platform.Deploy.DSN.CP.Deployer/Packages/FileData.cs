namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages
{
    /// <summary>
    ///     文件数据元接口，提供了相关的基本属性结构。
    /// </summary>
    internal class FileData : IFileData
    {
        #region Constructor

        /// <summary>
        ///     文件数据元接口，提供了相关的基本属性结构。
        /// </summary>
        /// <param name="data">封包片数据</param>
        public FileData(byte[] data)
        {
            _data = data;
        }

        /// <summary>
        ///     文件数据元接口，提供了相关的基本属性结构。
        /// </summary>
        /// <param name="data">封包片数据</param>
        /// <param name="currentId">当前封包片编号</param>
        public FileData(byte[] data, int currentId)
        {
            _data = data;
            _currentId = currentId;
        }

        #endregion

        #region Implementation of IFileData

        private int _currentId;
        private byte[] _data;

        /// <summary>
        ///     获取或设置当前文件数据包的编号
        /// </summary>
        public int CurrentId
        {
            get { return _currentId; }
            set { _currentId = value; }
        }

        /// <summary>
        ///     获取当前包的二进制数据
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
        }

        #endregion
    }
}