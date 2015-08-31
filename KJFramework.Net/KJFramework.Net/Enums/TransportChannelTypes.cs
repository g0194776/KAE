namespace KJFramework.Net.Enums
{
    /// <summary>
    ///   传输信道类型
    /// </summary>
    public enum TransportChannelTypes
    {
        /// <summary>
        ///   TCP通信信道
        /// </summary>
        TCP,
        /// <summary>
        ///   UDP通信信道
        /// </summary>
        UDP,
        /// <summary>
        ///   HTTP通信信道
        /// </summary>
        HTTP,
        /// <summary>
        ///   命名管道通信信道
        /// </summary>
        NamedPipe,
        /// <summary>
        ///   消息信道
        /// </summary>
        Message,
        /// <summary>
        ///   傀儡通信信道
        /// </summary>
        Puppet
    }
}