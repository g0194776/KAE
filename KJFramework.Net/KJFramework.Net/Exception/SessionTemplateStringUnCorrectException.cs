using System;
using System.Collections.Generic;

using System.Text;

namespace KJFramework.Net.Exception
{
    /// <summary>
    ///     会话基础模板字符串错误异常
    /// </summary>
    /// <remarks>
    ///     当会话基础模板字符串不支持固有的可填充模式时，触发该异常
    ///        * 会话基础模板字符串必须仅包含2个可填充点 : 如 : {0}, {1}
    /// </remarks>
    public class SessionTemplateStringUnCorrectException : System.Exception
    {
        /// <summary>
        ///     会话基础模板字符串错误异常
        /// </summary>
        /// <remarks>
        ///     当会话基础模板字符串不支持固有的可填充模式时，触发该异常
        ///        * 会话基础模板字符串必须仅包含2个可填充点 : 如 : {0}, {1}
        /// </remarks>
        public SessionTemplateStringUnCorrectException() : base("会话基础模板字符串错误 !")
        {
        }
    }
}
