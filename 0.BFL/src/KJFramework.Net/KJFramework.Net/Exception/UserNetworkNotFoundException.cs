using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     用户网络流未找到异常
    /// </summary>
    /// <remarks>
    ///     当程序使用自身模块存在的网络流，并且该流为null,时触发此异常
    /// </remarks>
    public class UserNetworkNotFoundException : System.Exception
    {
        /// <summary>
        ///     用户网络流未找到异常
        /// </summary>
        /// <remarks>
        ///     当程序使用自身模块存在的网络流，并且该流为null,时触发此异常
        /// </remarks>
        public UserNetworkNotFoundException() : base("用户网络流(NetworkStream) - 未找到!")
        {
        }
    }
}
