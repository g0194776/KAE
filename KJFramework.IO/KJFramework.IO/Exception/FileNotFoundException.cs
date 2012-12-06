namespace KJFramework.IO.Exception
{
    /// <summary>
    ///     文件未找到异常
    /// </summary>
    public class FileNotFoundException : System.Exception
    {
        /// <summary>
        ///     文件未找到异常
        /// </summary>
        public FileNotFoundException() : base("文件未找到 !")
        {
        }
    }
}