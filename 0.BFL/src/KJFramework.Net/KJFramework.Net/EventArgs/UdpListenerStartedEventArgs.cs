using System;
using System.Net.Sockets;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_UDPLISTENER_STARTED(Object sender, UdpListenerStartedEventArgs e);
    /// <summary>
    ///     UDP端口监听器开始监听事件
    /// </summary>
    public class UdpListenerStartedEventArgs : System.EventArgs
    {
        private UdpClient _client;
        /// <summary>
        ///     连接上的UDP客户端
        /// </summary>
        public UdpClient Client
        {
            get { return _client; }
        }

        /// <summary>
        ///     UDP端口监听器开始监听事件
        /// </summary>
        /// <param name="Client">监听中的UDP客户端</param>
        public UdpListenerStartedEventArgs(UdpClient Client)
        {
            _client = Client;
        }

    }
}
