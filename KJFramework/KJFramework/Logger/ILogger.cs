using System;
using KJFramework.Logger.LogObject;

namespace KJFramework.Logger
{
    /// <summary>
    ///     日志记录器元接口, 提供了相关的基本操作。
    /// </summary>
    public interface ILogger<T> where T : ILog
    {
        /// <summary>
        ///     获取或设置记录文件路径
        /// </summary>
        String LogFilePath { get; set; }
        /// <summary>
        ///     获取或设置记录器的可用状态
        /// </summary>
        bool Enable { get; set; }
        /// <summary>
        ///     使用一个日志文件来初始化日志记录器, 如果该文件不存在，则自动创建。
        /// </summary>
        /// <param name="logFilePath" type="string">
        ///     <para>
        ///         日志文件全路径
        ///     </para>
        /// </param>
        void Initialize(String logFilePath);
        /// <summary>
        ///     保存
        /// </summary>
        void Save();
        /// <summary>
        ///     将指定记录类型添加到记录集合中
        /// </summary>
        /// <param name="log" type="T">
        ///     <para>
        ///         记录
        ///     </para>
        /// </param>
        void Log(T log);
    }
}
