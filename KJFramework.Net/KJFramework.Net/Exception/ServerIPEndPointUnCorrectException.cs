using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     服务器远程终端错误异常
    /// </summary>
    public class ServerIPEndPointUnCorrectException : System.Exception
    {
        /// <summary>
        ///     服务器远程终端错误异常
        /// </summary>
        public ServerIPEndPointUnCorrectException() : base("服务器远程终端错误 !")
        {
        }
    }
}
