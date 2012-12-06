namespace KJFramework.Basic.Enum
{
    /// <summary>
    ///     消息传输类型
    /// </summary>
    /// <remarks>
    ///     此类型表示了该消息是直接点对点传递，还是经由服务器转发。
    /// </remarks>
    public enum MessageTransferType
    {
        /// <summary>
        ///     点对点传递
        /// </summary>
        P2P,
        /// <summary>
        ///     服务器转发
        /// </summary>
        Transmit
    }
}
