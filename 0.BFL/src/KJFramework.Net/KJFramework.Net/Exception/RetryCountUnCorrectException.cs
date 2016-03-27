using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     超时次数不正确异常
    /// </summary>
    /// <remarks>
    ///     当超时次数小于0时，触发该异常。
    /// </remarks>
    public class RetryCountUnCorrectException : System.Exception
    {
        /// <summary>
        ///     超时次数不正确异常
        /// </summary>
        /// <remarks>
        ///     当超时次数小于0时，触发该异常。
        /// </remarks>
        public RetryCountUnCorrectException() : base("超时次数不正确 !")
        {
        }
    }
}
