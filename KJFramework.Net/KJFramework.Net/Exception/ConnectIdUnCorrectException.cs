using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     连接ID错误异常
    /// </summary>
    /// <remarks>
    ///     当连接ID小于0的时候触发该异常。
    /// </remarks>
    public class ConnectIdUnCorrectException : System.Exception
    {
        /// <summary>
        ///     连接ID错误异常
        /// </summary>
        /// <remarks>
        ///     当连接ID小于0的时候触发该异常。
        /// </remarks>
        public ConnectIdUnCorrectException() : base("连接ID错误 !")
        {
        }
    }
}
