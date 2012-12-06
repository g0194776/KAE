using System;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_CONNECTIONCOUNT_FULL(Object sender, ConnectionCountFullEventArgs e);
    /// <summary>
    ///     连接数到达上限事件
    /// </summary>
    public class ConnectionCountFullEventArgs : System.EventArgs
    {
    }
}
