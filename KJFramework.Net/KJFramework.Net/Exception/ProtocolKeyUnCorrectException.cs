using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     协议ID错误异常
    /// </summary>
    /// <remarks>
    ///     当协议ID小于0时触发该异常。
    /// </remarks>
    public class ProtocolKeyUnCorrectException : System.Exception
    {
        /// <summary>
        ///     协议ID错误异常
        /// </summary>
        /// <remarks>
        ///     当协议ID小于0时触发该异常。
        /// </remarks>
        public ProtocolKeyUnCorrectException() : base("协议ID错误 !")
        {
        }
    }
}
