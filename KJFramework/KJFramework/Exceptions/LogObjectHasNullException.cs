namespace KJFramework.Exceptions
{
    /// <summary>
    ///     给定的记录对象为空异常
    /// </summary>
    public class LogObjectHasNullException : System.Exception
    {
        /// <summary>
        ///     给定的记录对象为空异常
        /// </summary>
        public LogObjectHasNullException() : base("给定的记录对象不能为空 !")
        {
        }
    }
}
