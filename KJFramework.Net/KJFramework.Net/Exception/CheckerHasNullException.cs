using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     检查器为空异常
    /// </summary>
    public class CheckerHasNullException : System.Exception
    {
        /// <summary>
        ///     检查器为空异常
        /// </summary>
        public CheckerHasNullException() : base("检查器为空 !")
        {
        }   
    }
}
