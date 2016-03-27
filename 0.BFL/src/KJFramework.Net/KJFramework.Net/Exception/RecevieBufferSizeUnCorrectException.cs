using System;
using System.Collections.Generic;
using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     接收缓冲大小不正确异常
    /// </summary>
    public class RecevieBufferSizeUnCorrectException : System.Exception
    {
        /// <summary>
        ///     接收缓冲大小不正确异常
        /// </summary>
        public RecevieBufferSizeUnCorrectException() : base("接收缓冲大小不正确 !")
        {
        }
    }
}
