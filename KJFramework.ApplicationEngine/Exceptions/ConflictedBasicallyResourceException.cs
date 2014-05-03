namespace KJFramework.ApplicationEngine.Exceptions
{
    /// <summary>
    ///    基础KAE应用资源冲突异常
    /// </summary>
    public class ConflictedBasicallyResourceException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///    基础KAE应用资源冲突异常
        /// </summary>
        public ConflictedBasicallyResourceException(string message) 
            : base(message)
        {

        }

        #endregion
    }
}