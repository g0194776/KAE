namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     连接远程网络终结点失败异常
    /// </summary>
    public class ConnectFailException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///     连接远程网络终结点失败异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public ConnectFailException(string message)
            : base(message)
        {

        }

        #endregion
    }
}