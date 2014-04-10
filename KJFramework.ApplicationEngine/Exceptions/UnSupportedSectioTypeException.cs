namespace KJFramework.ApplicationEngine.Exceptions
{
    /// <summary>
    ///    不支持的数据节异常
    /// </summary>
    public class UnSupportedSectioTypeException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///    不支持的数据节异常
        /// </summary>
        public UnSupportedSectioTypeException(string message) 
            : base(message)
        {

        }

        #endregion
    }
}