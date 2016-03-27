using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     消息分配器为空异常
    /// </summary>
    public class MessageDispatcherHasNullException : System.Exception
    {
        /// <summary>
        ///     消息分配器为空异常
        /// </summary>
        public MessageDispatcherHasNullException() : base("消息分配器为空 !")
        {
        }
    }
}
