using System;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_RETRYTIMEOUT<I>(Object sender, ServerRetryTimeoutEventArgs<I> e);
    /// <summary>
    ///     服务器重连超时事件
    /// </summary>
    /// <remarks>
    ///     当重连次数到达上限后，触发该事件。
    /// </remarks>
    public class ServerRetryTimeoutEventArgs<I> : System.EventArgs
    {
        private I _serverconnectobject;
        /// <summary>
        ///     重连超时的服务器连接对象
        /// </summary>
        public I Serverconnectobject
        {
            get { return _serverconnectobject; }
        }

        /// <summary>
        ///     服务器重连超时事件
        /// </summary>
        /// <param name="serverConnectObject" type="T">
        ///     <para>
        ///         重连超时的服务器连接对象
        ///     </para>
        /// </param>
        /// <remarks>
        ///     当重连次数到达上限后，触发该事件。
        /// </remarks>
        public ServerRetryTimeoutEventArgs(I serverConnectObject)
        {
            _serverconnectobject = serverConnectObject;
        }
    }
}
