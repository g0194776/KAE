namespace KJFramework.ServiceModel.Exceptions
{
    /// <summary>
    ///     未响应异常
    ///     <para>* 该错误一般指在指定时间内未收到服务器端的应答</para>
    /// </summary>
    public class NonresponseException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///     未响应异常
        ///     <para>* 该错误一般指在指定时间内未收到服务器端的应答</para>
        /// </summary>
        public NonresponseException()
        { 
        }

        /// <summary>
        ///     未响应异常
        ///     <para>* 该错误一般指在指定时间内未收到服务器端的应答</para>
        /// </summary>
        public NonresponseException(string message)
            : base(message)
        { 
        }

        #endregion
    }
}