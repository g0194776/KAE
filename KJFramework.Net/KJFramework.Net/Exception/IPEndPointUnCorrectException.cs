using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     终结点错误异常
    /// </summary>
    /// <remarks>
    ///     当尝试使用此终结点进行数据发送时，如果出现错误则触发该异常
    /// </remarks>
    public class IPEndPointUnCorrectException : System.Exception
    {
        /// <summary>
        ///     终结点错误异常
        /// </summary>
        /// <remarks>
        ///     当尝试使用此终结点进行数据发送时，如果出现错误则触发该异常
        /// </remarks>
        public IPEndPointUnCorrectException() : base("终结点错误 !")
        {
        }
    }
}
