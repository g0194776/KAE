using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     比较器为空异常
    /// </summary>
    public class ComparerHasNullException : System.Exception
    {
        /// <summary>
        ///     比较器为空异常
        /// </summary>
        public ComparerHasNullException() : base("比较器为空 !")
        {
        }
    }
}
