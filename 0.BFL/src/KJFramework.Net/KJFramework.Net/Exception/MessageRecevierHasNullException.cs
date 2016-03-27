using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     消息接收器为空异常
    /// </summary>
    public class MessageRecevierHasNullException : System.Exception
    {
        /// <summary>
        ///     消息接收器为空异常
        /// </summary>
        public MessageRecevierHasNullException() : base("消息接收器为空 !")
        {
        }
    }
}
