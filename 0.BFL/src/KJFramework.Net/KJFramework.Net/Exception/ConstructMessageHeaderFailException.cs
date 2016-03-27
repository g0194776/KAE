using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     构造消息头失败异常
    /// </summary>
    /// <remarks>
    ///     当根据读取的数据来构造消息头, 并且构造失败的时候, 触发该异常
    /// </remarks>
    public class ConstructMessageHeaderFailException : System.Exception
    {
        /// <summary>
        ///     构造消息头失败异常
        /// </summary>
        /// <remarks>
        ///     当根据读取的数据来构造消息头, 并且构造失败的时候, 触发该异常
        /// </remarks>
        public ConstructMessageHeaderFailException() : base("构造消息头失败!")
        {
        }
    }
}
