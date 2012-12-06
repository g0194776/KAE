using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     消息体长度不相等异常
    /// </summary>
    /// <remarks>
    ///     当消息体长度与消息头部中的消息体长度不相等时，触发该异常。
    /// </remarks>
    public class LengthUnCorrectException : System.Exception
    {
        /// <summary>
        ///     消息体长度不相等异常
        /// </summary>
        /// <remarks>
        ///     当消息体长度与消息头部中的消息体长度不相等时，触发该异常。
        /// </remarks>
        public LengthUnCorrectException() : base("消息体长度与消息头部中的消息体长度不相等 !")
        {
        }
    }
}
