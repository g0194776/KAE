using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     会话构造器为空异常
    /// </summary>
    public class SessionBuilderHasNullException : System.Exception
    {
        /// <summary>
        ///     会话构造器为空异常
        /// </summary>
        public SessionBuilderHasNullException() : base("会话构造器不能为空 !")
        {
        }
    }
}
