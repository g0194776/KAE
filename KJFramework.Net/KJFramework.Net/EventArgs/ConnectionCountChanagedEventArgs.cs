using System;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_CONNECTIONCOUNT_CHANAGE(Object sender, ConnectionCountChanagedEventArgs e);
    /// <summary>
    ///     连接数更改事件
    /// </summary>
    /// <remarks>
    ///     当连接池中的连接数有更改时，触发该事件。
    /// </remarks>
    public class ConnectionCountChanagedEventArgs : System.EventArgs
    {
        private int _connectioncount;
        /// <summary>
        ///     当前的连接数
        /// </summary>
        public int ConnectCount
        {
            get
            {
                return _connectioncount;
            }
        }
        /// <summary>
        ///      连接数更改事件
        /// </summary>
        /// <param name="ConnectCount" type="int">
        ///     <para>
        ///         当前的连接数
        ///     </para>
        /// </param>
        /// <remarks>
        ///     当连接池中的连接数有更改时，触发该事件。
        /// </remarks>
        public ConnectionCountChanagedEventArgs(int ConnectCount)
        {
            _connectioncount = ConnectCount;
        }
    }
}
