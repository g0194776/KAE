using System;
using System.Collections.Generic;
using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     消息创建器为空异常
    /// </summary>
    public class MessageBuilderHasNullException : System.Exception
    {
        /// <summary>
        ///     消息创建器为空异常
        /// </summary>
        public MessageBuilderHasNullException() : base("消息创建器未找到 !")
        {
        }   
    }
}
