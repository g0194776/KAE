using System;
namespace KJFramework.Net.EventArgs
{
    /// <summary>
    ///     接收器断开连接事件
    /// </summary>
    public class RecevierDisconnectedEventArgs : System.EventArgs
    {
        #region 成员

        private String _key;
        /// <summary>
        ///     获取或设置唯一标识
        /// </summary>
        public String Key
        {
            get { return _key; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     接收器断开连接事件
        /// </summary>
        /// <param name="key">唯一标识</param>
        public RecevierDisconnectedEventArgs(String key)
        {
            _key = key;
        }

        #endregion
    }
}