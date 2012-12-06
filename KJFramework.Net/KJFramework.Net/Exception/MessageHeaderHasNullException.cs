using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     消息头为空异常。
    /// </summary>
    /// <remarks>
    ///     当消息头为null时，则触发该异常。
    /// </remarks>
    public class MessageHeaderHasNullException : System.Exception
    {
        /// <summary>
        ///     消息头为空异常。
        /// </summary>
        /// <remarks>
        ///     当消息头为null时，则触发该异常。
        /// </remarks>
        public MessageHeaderHasNullException() : base("消息头为空 !")
        {
        }
    }
}
