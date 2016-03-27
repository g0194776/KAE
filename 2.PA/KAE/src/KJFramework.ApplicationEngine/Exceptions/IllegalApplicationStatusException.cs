namespace KJFramework.ApplicationEngine.Exceptions
{
    /// <summary>
    ///    错误的KAE应用状态异常
    /// </summary>
    public class IllegalApplicationStatusException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///    错误的KAE应用状态异常
        /// </summary>
        public IllegalApplicationStatusException(string message) 
            : base(message)
        {

        }

        #endregion
    }
}