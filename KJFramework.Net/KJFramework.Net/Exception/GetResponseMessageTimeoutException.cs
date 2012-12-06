using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     获取指定的响应消息超异常
    /// </summary>
    /// <remarks>
    ///     当获取的消息在指定时间内仍然不存在，则触发该异常
    /// </remarks>
    public class GetResponseMessageTimeoutException : System.Exception
    {
        /// <summary>
        ///     获取指定的响应消息超异常
        /// </summary>
        /// <remarks>
        ///     当获取的消息在指定时间内仍然不存在，则触发该异常
        /// </remarks>
        public GetResponseMessageTimeoutException() : base("获取指定的响应消息超时 !")
        {
        }
    }
}
