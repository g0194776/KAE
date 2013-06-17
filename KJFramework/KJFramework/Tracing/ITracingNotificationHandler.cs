namespace KJFramework.Tracing
{
    /// <summary>
    ///     日志通知器接口
    /// </summary>
    public interface ITracingNotificationHandler
    {
        /// <summary>
        ///     处理当前日志的通知
        ///     <para>* 只有在满足当前日志等级后，才会调用此方法</para>
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="ex">异常对象</param>
        /// <param name="message">错误消息</param>
        /// <param name="loggerType">记录器类型名称</param>
        void Handle(TracingLevel level, System.Exception ex, string message, string loggerType);
    }
}