using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     IP地址错误异常
    /// </summary>
    public class IPAddressUnCorrectException : System.Exception
    {
        /// <summary>
        ///     IP地址错误异常
        /// </summary>
        public IPAddressUnCorrectException() : base("IP地址错误 !")
        {
        }
    }
}
