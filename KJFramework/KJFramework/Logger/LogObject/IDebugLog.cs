using System;
using KJFramework.Basic.Enum;

namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     错误记录对象元接口, 提供了基本的属性结构
    /// </summary>
    public interface IDebugLog : ILog
    {
        /// <summary>
        ///     堆栈消息
        /// </summary>
        String StrackMessage { get; set; }
        /// <summary>
        ///     调试消息
        /// </summary>
        String DebugMessage { get; set; }
        /// <summary>
        ///     异常位置
        /// </summary>
        String Location { get; set; }
        /// <summary>
        ///     异常等级
        /// </summary>
        DebugGrade Grade { get; set; }
    }
}
