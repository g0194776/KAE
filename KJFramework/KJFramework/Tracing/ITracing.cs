namespace KJFramework.Tracing
{
    /// <summary>
    ///     日志器接口
    /// </summary>
    public interface ITracing
    {
        void Info(string format, params object[] args);
        void Info(System.Exception ex, string format, params object[] args);
        void Warn(string format, params object[] args);
        void Warn(System.Exception ex, string format, params object[] args);
        void Error(string format, params object[] args);
        void Error(System.Exception ex, string format, params object[] args);
        void Critical(string format, params object[] args);
        void Critical(System.Exception ex, string format, params object[] args);
        void LogFileOnly(TracingLevel level, string format, params object[] args);
        void LogFileOnly(TracingLevel level, System.Exception error, string format, params object[] args);
    }
}
