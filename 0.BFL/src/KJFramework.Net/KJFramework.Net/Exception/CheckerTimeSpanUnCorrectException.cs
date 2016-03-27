using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     检查器时间间隔设置不正确异常
    /// </summary>
    public class CheckerTimeSpanUnCorrectException : System.Exception
    {
        /// <summary>
        ///     检查器时间间隔设置不正确异常
        /// </summary>
        public CheckerTimeSpanUnCorrectException() : base("检查器时间间隔设置不正确 !")
        {
        }
    }
}
