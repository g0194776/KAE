using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     服务器连接对象为空异常
    /// </summary>
    public class ServerConnectObjectHasNullException : System.Exception
    {
        /// <summary>
        ///     服务器连接对象为空异常
        /// </summary>
        public ServerConnectObjectHasNullException() : base("服务器连接对象不能为空 !")
        {
        }
    }
}
