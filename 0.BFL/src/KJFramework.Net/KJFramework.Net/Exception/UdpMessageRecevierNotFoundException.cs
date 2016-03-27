using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     UDP消息接收器没有被找到异常
    /// </summary>
    /// <remarks>
    ///     当UDP接收器 == null时，触发该异常
    /// </remarks>
    public class UdpMessageRecevierNotFoundException : System.Exception
    {
        /// <summary>
        ///     UDP消息接收器没有被找到异常
        /// </summary>
        /// <remarks>
        ///     当UDP接收器 == null时，触发该异常
        /// </remarks>
        public UdpMessageRecevierNotFoundException() : base("UDP消息接收器没有被找到 !")
        { }
    }
}
