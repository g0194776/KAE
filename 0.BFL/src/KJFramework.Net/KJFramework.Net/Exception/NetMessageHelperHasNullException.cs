using System;
using System.Collections.Generic;
using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///    网络消息帮助器未找到异常
    /// </summary>
    public class NetMessageHelperHasNullException : System.Exception
    {
        /// <summary>
        ///    网络消息帮助器未找到异常
        /// </summary>
        public NetMessageHelperHasNullException() :base("网络消息帮助器未找到")
        { 
        }   
    }
}
