using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     消息体数据为null异常
    /// </summary>
    /// <remarks>
    ///     当要使用消息体数据的时候，却发现消息体数据不存在，则触发该异常。
    /// </remarks>
    public class MessageBodyHasNullException : System.Exception
    {
        /// <summary>
        ///     消息体数据为null异常
        /// </summary>
        /// <remarks>
        ///     当要使用消息体数据的时候，却发现消息体数据不存在，则触发该异常。
        /// </remarks>
        public MessageBodyHasNullException() : base("消息体数据为 null !")
        {
        }    
    }
}
