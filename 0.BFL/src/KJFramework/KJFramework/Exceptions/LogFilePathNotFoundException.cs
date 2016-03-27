namespace KJFramework.Exceptions
{
    /// <summary>
    ///     日志文件未找到异常
    /// </summary>
    public class LogFilePathNotFoundException : System.Exception
    {
        /// <summary>
        ///     日志文件未找到异常
        /// </summary>
        public LogFilePathNotFoundException() : base("日志文件未找到 !")
        {
        }
    }
}
