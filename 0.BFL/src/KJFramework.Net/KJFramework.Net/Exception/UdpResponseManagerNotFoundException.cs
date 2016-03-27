using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     UDP消息响应器未找到异常
    /// </summary>
    public class UdpResponseManagerNotFoundException : System.Exception
    {
        /// <summary>
        ///     UDP消息响应器未找到异常
        /// </summary>
        public UdpResponseManagerNotFoundException() : base("UDP消息响应器未找到 !")
        {
        }
    }
}
