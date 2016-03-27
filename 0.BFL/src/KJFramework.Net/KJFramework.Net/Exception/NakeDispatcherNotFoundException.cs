using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     核心消息分配器未找到异常
    /// </summary>
    /// <remarks>
    ///     核心消息分配器 == null时，触发该异常
    /// </remarks>
    public class NakeDispatcherNotFoundException : System.Exception
    {
        /// <summary>
        ///     核心消息分配器未找到异常
        /// </summary>
        /// <remarks>
        ///     核心消息分配器 == null时，触发该异常
        /// </remarks>
        public NakeDispatcherNotFoundException() : base("核心消息分配器未找到 !")
        {
        }
    }
}
