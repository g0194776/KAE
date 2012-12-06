using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     UDP监听器内置对象未被初始化异常
    /// </summary>
    /// <remarks>
    ///     当使用UDP监听器中的UdpClient时，如果该对象未被初始化( == null), 则会触发该异常
    /// </remarks>
    public class UdpListenerNotFoundException : System.Exception
    {
        /// <summary>
        ///     UDP监听器内置对象未被初始化异常
        /// </summary>
        /// <remarks>
        ///     当使用UDP监听器中的UdpClient时，如果该对象未被初始化( == null), 则会触发该异常
        /// </remarks>
        public UdpListenerNotFoundException() : base("UdpClient 对象未被初始化 !")
        {
        }
    }
}
