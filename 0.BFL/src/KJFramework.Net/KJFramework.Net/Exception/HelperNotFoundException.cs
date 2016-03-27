using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     没有找到帮助器异常
    /// </summary>
    /// <remarks>
    ///     当解析接收数据时, 如果当前任务方法找不到帮助器, 则触发该异常
    /// </remarks>
    public class HelperNotFoundException : System.Exception
    {
        /// <summary>
        ///     没有找到帮助器异常
        /// </summary>
        /// <remarks>
        ///     当解析接收数据时, 如果当前任务方法找不到帮助器, 则触发该异常
        /// </remarks>
        public HelperNotFoundException() : base("没有找到消息接收器帮助器！")
        {
        }
    }
}
