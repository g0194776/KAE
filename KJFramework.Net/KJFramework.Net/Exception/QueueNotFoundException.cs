using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     队列未找到
    /// </summary>
    public class QueueNotFoundException : System.Exception
    {
        /// <summary>
        ///     队列未找到
        /// </summary>
        public QueueNotFoundException() : base("队列未找到 !")
        {

        }
    }
}
