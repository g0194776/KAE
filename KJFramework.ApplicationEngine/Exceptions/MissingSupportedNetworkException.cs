namespace KJFramework.ApplicationEngine.Exceptions
{
    /// <summary>
    ///    缺少支持的网络类型异常
    ///    <para>* 当一个KAE的应用内部支持了相应处理协议的Processor但是没有任何建议的网络类型时，会抛出此类异常</para>
    /// </summary>
    public class MissingSupportedNetworkException : System.Exception
    {
        #region Constructor

        /// <summary>
        ///    缺少支持的网络类型异常
        ///    <para>* 当一个KAE的应用内部支持了相应处理协议的Processor但是没有任何建议的网络类型时，会抛出此类异常</para>
        /// </summary>
        public MissingSupportedNetworkException(string message) 
            : base(message)
        {

        }

        #endregion
    }
}