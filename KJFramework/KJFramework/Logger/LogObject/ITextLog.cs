using System;

namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     日志文本记录项源接口，提供了相关的属性结构。
    /// </summary>
    public interface ITextLog : IDebugLog
    {
        /// <summary>
        ///     获取当前要写入日志的内容。
        ///         * [注] 这里可以直接返回带有格式的日志内容。
        /// </summary>
        /// <returns>返回日志的内容</returns>
        String GetLogContent();
        /// <summary>
        ///     获取日志头部信息
        /// </summary>
        /// <returns>返回头部信息</returns>
        String GetHead();
        /// <summary>
        ///     获取或设置当前记录项是否当作头部记录项。
        /// </summary>
        bool IsHead { get; set; }
        /// <summary>
        ///     获取或设置日志记录项格式
        /// </summary>
        ITextLogFormatter Formatter { get; set; }
    }
}