using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     关键字为空异常
    /// </summary>
    public class KeyHasNullException : System.Exception
    {
        /// <summary>
        ///     关键字为空异常
        /// </summary>
        public KeyHasNullException()
            : base("关键字为null !")
        {
        }
    }
}
