namespace KJFramework.Exceptions
{
    /// <summary>
    ///     保存日志文件失败异常
    /// </summary>
    public class SaveLogFileException : System.Exception
    {
        /// <summary>
        ///     保存日志文件失败异常
        /// </summary>
        public SaveLogFileException() : base("保存日志文件失败 !")
        {
        }
    }
}
