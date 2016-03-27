using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     服务ID不正确异常
    /// </summary>
    /// <remarks>
    ///     当服务ID小于0时触发该异常。
    /// </remarks>
    public class ServiceKeyUnCorrectException : System.Exception
    {
        /// <summary>
        ///     服务ID不正确异常
        /// </summary>
        /// <remarks>
        ///     当服务ID小于0时触发该异常。
        /// </remarks>
        public ServiceKeyUnCorrectException() : base("服务ID不正确 !")
        {
        }
    }
}
