using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     点对点通道管理器未找到异常
    /// </summary>
    public class P2PChannelManagerNotFoundException : System.Exception
    {
        /// <summary>
        ///     点对点通道管理器未找到异常
        /// </summary>
        public P2PChannelManagerNotFoundException() : base("点对点通道管理器未找到 !")
        {
        }
    }
}
