namespace KJFramework.ServiceModel.Exceptions
{
    /// <summary>
    ///     不能到达异常
    ///     <para>* 该错误一般指请求错误或者无法达到远程终结点的异常</para>
    /// </summary>
    public class CannotReachableException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///     不能到达异常
        ///     <para>* 该错误一般指请求错误或者无法达到远程终结点的异常</para>
        /// </summary>
        public CannotReachableException()
        { 
        }

        /// <summary>
        ///     不能到达异常
        ///     <para>* 该错误一般指请求错误或者无法达到远程终结点的异常</para>
        /// </summary>
        public CannotReachableException(string message)
            : base(message)
        { 
        }

        #endregion
    }
}