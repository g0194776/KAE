namespace KJFramework.ApplicationEngine.Exceptions
{
    /// <summary>
    ///    无法找到本机有效的IPv4通信地址
    /// </summary>
    public class IPv4LocalAddressNotFoundException : System.Exception
    {
        #region Constructors.

        /// <summary>
        ///    无法找到本机有效的IPv4通信地址
        /// </summary>
        public IPv4LocalAddressNotFoundException(string message)
            : base(message)
        { }

        #endregion
    }
}