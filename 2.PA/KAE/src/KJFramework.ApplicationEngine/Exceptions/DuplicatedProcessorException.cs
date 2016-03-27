namespace KJFramework.ApplicationEngine.Exceptions
{
    /// <summary>
    ///    重复KAE处理器异常
    /// </summary>
    public class DuplicatedProcessorException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///    重复KAE处理器异常
        /// </summary>
        public DuplicatedProcessorException(string message) 
            : base(message)
        {

        }

        #endregion
    }
}