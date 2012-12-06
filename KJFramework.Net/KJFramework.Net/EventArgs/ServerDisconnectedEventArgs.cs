using System;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_SERVER_DISCONNECTED(Object sender, ServerDisconnectedEventArgs e);
    /// <summary>
    ///     服务器断开事件
    /// </summary>
    /// <remarks>
    ///     当与服务器的TCP连接断开后, 触发该事件
    /// </remarks>
    public class ServerDisconnectedEventArgs : System.EventArgs
    {
        /// <summary>
        ///     服务器断开事件
        /// </summary>
        /// <remarks>
        ///     当与服务器的TCP连接断开后, 触发该事件
        /// </remarks>
        public ServerDisconnectedEventArgs()
        {
        }
    }
}
