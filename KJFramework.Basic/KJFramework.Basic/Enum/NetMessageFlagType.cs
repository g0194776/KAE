namespace KJFramework.Basic.Enum
{
    /// <summary>
    ///     网络消息标示类型
    /// </summary>
    /// <remarks>
    ///     该表示表示了指定网络消息是接收到的还是要发送的。
    /// </remarks>
    public enum NetMessageFlagType
    {
        /// <summary>
        ///     接收到的消息
        /// </summary>
        /// <remarks>
        ///     如果设置为此项，将不会初始化消息头内容字节数组，节省内存空间
        /// </remarks>
        Arrive,
        /// <summary>
        ///     要发送的消息
        /// </summary>
        /// <remarks>
        ///     如果设置为此项，将会初始化消息头内容字节数组，为了发送消息时消息头部内容的填充做准备。
        /// </remarks>
        Send
    }   
}
