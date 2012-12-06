namespace KJFramework.Exception
{
    /// <summary>
    ///     日志文件路径格式不正确异常
    /// </summary>
    public class LogFilePathUnCorrectException : System.Exception
    {
        /// <summary>
        ///     日志文件未找到异常
        /// </summary>
        public LogFilePathUnCorrectException()
            : base("日志文件路径格式不正确 !")
        {
        }
    }
}
