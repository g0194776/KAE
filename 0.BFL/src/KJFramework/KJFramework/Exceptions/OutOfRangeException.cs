namespace KJFramework.Exceptions
{
    /// <summary>
    ///     ������Χ�쳣
    /// </summary>
    public class OutOfRangeException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///     ������Χ�쳣
        /// </summary>
        /// <param name="message">�쳣��Ϣ</param>
        public OutOfRangeException(string message)
            : base(message)
        {

        }

        #endregion
    }
}