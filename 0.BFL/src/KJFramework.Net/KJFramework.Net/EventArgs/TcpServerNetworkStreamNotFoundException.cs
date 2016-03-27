using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.EventArgs
{
    /// <summary>
    ///     TCP服务器网络流没有被找到异常
    /// </summary>
    /// <remarks>
    ///     当使用TCP协议向服务器发送请求时，如果服务器的NetworkStream不存在或者为null时，触发该异常
    /// </remarks>
    public class TcpServerNetworkStreamNotFoundException : System.Exception
    {
        /// <summary>
        ///     TCP服务器网络流没有被找到异常
        /// </summary>
        /// <remarks>
        ///     当使用TCP协议向服务器发送请求时，如果服务器的NetworkStream不存在或者为null时，触发该异常
        /// </remarks>
        public TcpServerNetworkStreamNotFoundException() : base("TCP协议的服务器网络流没有被找到 !")
        {
        }
    }
}
