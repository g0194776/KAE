using System;
using System.Collections.Generic;
using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     消息帮助器为空异常
    /// </summary>
    public class MessageHelperHasNullException : System.Exception
    {
        /// <summary>
        ///     消息帮助器为空异常
        /// </summary>
        public MessageHelperHasNullException() : base("消息帮助器未找到 !")
        {
        }
    }
}
