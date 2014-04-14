namespace KJFramework.ApplicationEngine.Exceptions
{
    /// <summary>
    ///    不支持的数据节异常
    /// </summary>
    public class UnSupportedSectionTypeException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///    不支持的数据节异常
        /// </summary>
        public UnSupportedSectionTypeException(string message) 
            : base(message)
        {

        }

        #endregion
    }
}