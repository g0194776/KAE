namespace KJFramework.Basic.Enum
{
    /// <summary>
    ///     绑定方式枚举
    /// </summary>
    public enum BindingTypes
    {
        /// <summary>
        ///     TCP绑定
        /// </summary>
        Tcp,
        /// <summary>
        ///     UDP绑定
        /// </summary>
        Udp,
        /// <summary>
        ///     HTTP绑定
        /// </summary>
        Http,
        /// <summary>
        ///     HTTPS绑定
        /// </summary>
        Https,
        /// <summary>
        ///     消息队列绑定
        /// </summary>
        MessageQueue,
        /// <summary>
        ///     消息通道绑定
        /// </summary>
        Ipc
    }
}