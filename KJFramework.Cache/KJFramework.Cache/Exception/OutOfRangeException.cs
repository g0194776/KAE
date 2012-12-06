namespace KJFramework.Cache.Exception
{
    /// <summary>
    ///     超出范围异常
    /// </summary>
    public class OutOfRangeException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///     超出范围异常
        /// </summary>
        /// <param name="message">异常信息</param>
        public OutOfRangeException(string message)
            : base(message)
        {

        }

        #endregion
    }
}