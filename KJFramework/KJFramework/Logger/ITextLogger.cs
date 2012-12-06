using System;
using KJFramework.Basic.Enum;
using KJFramework.Logger.LogObject;

namespace KJFramework.Logger
{
    /// <summary>
    ///     文字记录器元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITextLogger<T> : ILogger<T>, IDisposable
        where T : ITextLog
    {
        /// <summary>
        ///     将指定记录类型添加到记录集合中
        ///           *  记录器会自动判断是否为头部标示。
        ///           *  使用此方法，记录的异常优先等级默认为：普通
        /// </summary>
        /// <param name="exception">异常对象</param>
        void Log(System.Exception exception);
        /// <summary>
        ///     将指定记录类型添加到记录集合中
        ///           *  记录器会自动判断是否为头部标示。
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="grade">异常等级</param>
        void Log(System.Exception exception, DebugGrade grade);
        /// <summary>
        ///     将指定记录类型添加到记录集合中
        ///           *  记录器会自动判断是否为头部标示。
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="grade">异常等级</param>
        /// <param name="name">
        ///     记录人
        ///         * [注] ： 如果不当作头来用，可以设置为null
        /// </param>
        void Log(System.Exception exception, DebugGrade grade, String name);
        /// <summary>
        ///     将指定记录类型添加到记录集合中
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="grade">异常等级</param>
        /// <param name="isHead">头标示</param>
        /// <param name="name">
        ///     记录人
        ///         * [注] ： 如果不当作头来用，可以设置为null
        /// </param>
        void Log(System.Exception exception, DebugGrade grade, bool isHead, String name);
        /// <summary>
        ///     将指定文字内容记录到记录集合中
        /// </summary>
        /// <param name="text">文字内容</param>
        void Log(String text);
    }
}