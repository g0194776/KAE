using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     构造网络包异常
    /// </summary>
    /// <remarks>
    ///     当构造网络包数据不充足时, 触发该异常
    /// </remarks>
    public class CreateNetMessageException : System.Exception
    {
        /// <summary>
        ///     构造网络包异常
        /// </summary>
        /// <remarks>
        ///     当构造网络包数据不充足时, 触发该异常
        /// </remarks>
        public CreateNetMessageException() : base("构造网络包时, 出现了异常!")
        {
        }
    }
}
