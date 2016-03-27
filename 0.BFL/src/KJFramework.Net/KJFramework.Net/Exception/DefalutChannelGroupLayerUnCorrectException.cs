using System;
using System.Collections.Generic;
using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     默认的通道群组所支持子层数目不正确异常
    /// </summary>
    public class DefalutChannelGroupLayerUnCorrectException : System.Exception
    {
        /// <summary>
        ///     默认的通道群组所支持子层数目不正确异常
        /// </summary>
        public DefalutChannelGroupLayerUnCorrectException() : base("默认的通道群组所支持子层数目不正确 !")
        {
            
        }
    }
}
