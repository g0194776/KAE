using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     服务器ID不正确异常
    /// </summary>
    /// <remarks>
    ///     当服务器（FS）ID小于0时，触发该异常。
    /// </remarks>
    public class ServerIdUnCorrectException : System.Exception
    {
        /// <summary>
        ///     服务器ID不正确异常
        /// </summary>
        /// <remarks>
        ///     当服务器（FS）ID小于0时，触发该异常。
        /// </remarks>
        public ServerIdUnCorrectException() : base("服务器ID不正确 !")
        {
        }
    }
}
