using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     消息缓冲池未找到异常
    /// </summary>
    /// <remarks>
    ///     当消息缓冲池 == null 时，触发该异常
    /// </remarks>
    public class BufferPoolNotFoundException : System.Exception
    {
        /// <summary>
        ///     消息缓冲池未找到异常
        /// </summary>
        /// <remarks>
        ///     当消息缓冲池 == null 时，触发该异常
        /// </remarks>
        public BufferPoolNotFoundException() : base("消息缓冲池未找到异常 !")
        {
        }
    }
}
