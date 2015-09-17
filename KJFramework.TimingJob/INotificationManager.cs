using System;

namespace KJFramework.TimingJob
{
    /// <summary>
    ///     信息通知管理器
    /// </summary>
    public interface INotificationManager
    {
        #region Methods.

        /// <summary>
        ///     记录一段Info级别的信息
        /// </summary>
        /// <param name="format">信息格式化字符串</param>
        /// <param name="args">参数信息</param>
        void Info(string format, params object[] args);
        /// <summary>
        ///     记录一段Warn级别的信息
        /// </summary>
        /// <param name="format">信息格式化字符串</param>
        /// <param name="args">参数信息</param>
        void Warn(string format, params object[] args);
        /// <summary>
        ///     记录一段Warn级别的信息
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="format">信息格式化字符串</param>
        /// <param name="args">参数信息</param>
        void Warn(Exception ex, string format, params object[] args);
        /// <summary>
        ///     记录一段Error级别的信息
        /// </summary>
        /// <param name="ex">异常信息</param>
        void Error(Exception ex);
        /// <summary>
        ///     记录一段Warn级别的信息
        /// </summary>
        /// <param name="format">信息格式化字符串</param>
        /// <param name="args">参数信息</param>
        void Error(string format, params object[] args);
        /// <summary>
        ///     记录一段Error级别的信息
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="format">信息格式化字符串</param>
        /// <param name="args">参数信息</param>
        void Error(Exception ex, string format, params object[] args);
        /// <summary>
        ///     记录一段Critical级别的信息
        /// </summary>
        /// <param name="format">信息格式化字符串</param>
        /// <param name="args">参数信息</param>
        void Critical(string format, params object[] args);

        #endregion
    }
}