using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     TCP监听器内置对象未被初始化异常
    /// </summary>
    /// <remarks>
    ///     当使用TCP监听器中的TcpListener时，如果该对象未被初始化( == null), 则会触发该异常
    /// </remarks>
    public class TcpListenerNotFoundException : System.Exception
    {
        /// <summary>
        ///     TCP监听器内置对象未被初始化异常
        /// </summary>
        /// <remarks>
        ///     当使用TCP监听器中的TcpListener时，如果该对象未被初始化( == null), 则会触发该异常
        /// </remarks>
        public TcpListenerNotFoundException() : base("TcpListener 对象未被初始化异常")
        {
        }
    }
}
