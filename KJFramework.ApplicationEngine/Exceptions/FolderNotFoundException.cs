namespace KJFramework.ApplicationEngine.Exceptions
{
    /// <summary>
    ///    目标文件夹不存在异常
    /// </summary>
    public class FolderNotFoundException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///    目标文件夹不存在异常
        /// </summary>
        public FolderNotFoundException(string message) 
            : base(message)
        {

        }

        #endregion
    }
}