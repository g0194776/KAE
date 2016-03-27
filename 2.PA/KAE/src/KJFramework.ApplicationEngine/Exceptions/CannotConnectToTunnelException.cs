namespace KJFramework.ApplicationEngine.Exceptions
{
    /// <summary>
    ///    与APP内部建立通信隧道异常
    /// </summary>
    public class CannotConnectToTunnelException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///    与APP内部建立通信隧道异常
        /// </summary>
        public CannotConnectToTunnelException(string message) 
            : base(message)
        {

        }

        #endregion
    }
}