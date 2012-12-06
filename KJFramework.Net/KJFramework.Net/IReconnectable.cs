namespace KJFramework.Net
{
    /// <summary>
    ///     拥有重新连接功能的元接口，提供了相关的基本操作。
    /// </summary>
    public interface IReconnectable
    {
        /// <summary>
        ///     重新连接
        /// </summary>
        bool Retry();
        /// <summary>
        ///     获取重试连接的总共次数
        /// </summary>
        int RetryCount { get; }
        /// <summary>
        ///     获取或设置已重试次数
        /// </summary>
        int RetryIndex { get; set; }
    }
}