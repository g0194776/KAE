using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     结束时间设置错误异常
    /// </summary>
    /// <remarks>
    ///     当结束时间小于当前时间则会触发该异常。
    /// </remarks>
    public class EndDateTimeUnCorrectException : System.Exception
    {
        /// <summary>
        ///     结束时间设置错误异常
        /// </summary>
        /// <remarks>
        ///     当结束时间小于当前时间则会触发该异常。
        /// </remarks>
        public EndDateTimeUnCorrectException()
            : base("结束时间设置错误 !")
        {
        }   
    }
}
