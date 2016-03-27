namespace KJFramework.ApplicationEngine.Exceptions
{
    /// <summary>
    ///    申请资源失败异常
    /// </summary>
    public class AllocResourceFailedException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///    申请资源失败异常
        /// </summary>
        public AllocResourceFailedException(string message) 
            : base(message)
        {

        }

        #endregion
    }
}