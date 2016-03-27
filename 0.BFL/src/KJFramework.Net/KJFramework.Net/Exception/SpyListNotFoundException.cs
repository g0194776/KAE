using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     拦截器列表未找到异常
    /// </summary>
    public class SpyListNotFoundException : System.Exception
    {
        /// <summary>
        ///     拦截器列表未找到异常
        /// </summary>
        public SpyListNotFoundException() : base("拦截器列表未找到 !")
        {
        }
    }
}
