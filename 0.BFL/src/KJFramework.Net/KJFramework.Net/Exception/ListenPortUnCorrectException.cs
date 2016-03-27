using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     监听端口非法异常
    /// </summary>
    /// <remarks>
    ///     当监听的端口小于等于0 时，触发该异常
    /// </remarks>
    public class ListenPortUnCorrectException : System.Exception
    {
        /// <summary>
        ///     监听端口非法异常
        /// </summary>
        /// <remarks>
        ///     当监听的端口小于等于0 时，触发该异常
        /// </remarks>
        public ListenPortUnCorrectException() : base("监听端口非法 !")
        {
        }
    }
}
