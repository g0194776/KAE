namespace KJFramework.Net.Channels.Enums
{
    /// <summary>
    ///     通讯状态枚举
    /// </summary>
    public enum CommunicationStates
    {
        /// <summary>
        ///    未知
        /// </summary>
        Unknown,
        /// <summary>
        ///     已经被创建
        /// </summary>
        Created,
        /// <summary>
        ///     打开中
        /// </summary>
        Opening,
        /// <summary>
        ///     已经打开
        /// </summary>
        Opened,
        /// <summary>
        ///     关闭中
        /// </summary>
        Closing,
        /// <summary>
        ///     已经关闭
        /// </summary>
        Closed,
        /// <summary>
        ///     出现错误
        /// </summary>
        Faulte
    }
}