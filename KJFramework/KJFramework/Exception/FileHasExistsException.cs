namespace KJFramework.Exception
{
    /// <summary>
    ///     文件已经存在异常
    /// </summary>
    /// <remarks>
    ///     当要创建文件时，设置为不允许覆盖，而要创建的文件存在，则触发该异常。
    /// </remarks>
    public class FileHasExistsException : System.Exception
    {
        /// <summary>
        ///     文件已经存在异常
        /// </summary>
        /// <remarks>
        ///     当要创建文件时，设置为不允许覆盖，而要创建的文件存在，则触发该异常。
        /// </remarks>
        public FileHasExistsException() : base("文件已经存在 !")
        {
        }
    }
}