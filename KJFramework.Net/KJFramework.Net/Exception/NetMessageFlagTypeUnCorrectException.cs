using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     消息标示类型不合法异常
    /// </summary>
    /// <remarks>
    ///     当一个消息的标示类型被设置成Arrive(接收)时，却又要附加消息头标示在消息内容字节数组中，则会触发该异常， 还有如下情况将会触发该异常：
    ///        * 发送一个消息请求时，如果要发送的消息为接收标示类型，而不是发送标示类型，则触发异常。
    /// </remarks>
    public class NetMessageFlagTypeUnCorrectException : System.Exception
    {
        /// <summary>
        ///     消息标示类型不合法异常
        /// </summary>
        /// <remarks>
        ///     当一个消息的标示类型被设置成Arrive(接收)时，却又要附加消息头标示在消息内容字节数组中，则会触发该异常
        /// </remarks>
        public NetMessageFlagTypeUnCorrectException() : base("该消息设置的标示类型不合法 !")
        {
        }
    }
}
