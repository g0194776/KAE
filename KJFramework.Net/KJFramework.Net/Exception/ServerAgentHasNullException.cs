using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     服务器代理器为空异常
    /// </summary>
    public class ServerAgentHasNullException : System.Exception
    {
        /// <summary>
        ///     服务器代理器为空异常
        /// </summary>
        public ServerAgentHasNullException() : base("服务器代理器为空 ！")
        {
        }
    }
}
