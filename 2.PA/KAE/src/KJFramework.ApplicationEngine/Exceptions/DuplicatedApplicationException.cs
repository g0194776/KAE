namespace KJFramework.ApplicationEngine.Exceptions
{
    /// <summary>
    ///    重复KAE应用异常
    /// </summary>
    public class DuplicatedApplicationException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///    重复KAE应用异常
        /// </summary>
        public DuplicatedApplicationException(string message) 
            : base(message)
        {

        }

        #endregion
    }
}