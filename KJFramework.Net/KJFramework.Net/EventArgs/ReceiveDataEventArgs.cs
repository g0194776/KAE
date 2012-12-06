namespace KJFramework.Net.EventArgs
{
    /// <summary>
    ///     接收新数据事件
    /// </summary>
    public class ReceiveDataEventArgs : System.EventArgs
    {
        #region 成员

        private byte[] _data;
        /// <summary>
        ///     获取接收的数据
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     接收新数据事件
        /// </summary>
        /// <param name="data">新的数据</param>
        public ReceiveDataEventArgs(byte[] data)
        {
            _data = data;
        }

        #endregion
    }
}