namespace KJFramework.Data.ObjectDB.Exceptions
{
    /// <summary>
    ///     钩子处理异常
    /// </summary>
    public class HookProcessException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///     钩子处理异常
        /// </summary>
        /// <param name="message">异常信息</param>
        public HookProcessException(string message)
            : base(message)
        {

        }

        #endregion
    }
}