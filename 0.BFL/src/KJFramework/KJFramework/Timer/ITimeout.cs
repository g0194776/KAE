namespace KJFramework.Timer
{
    /// <summary>
    ///     超时计算元接口, 提供了相应的基本操作。
    /// </summary>
    public interface ITimeout
    {
        /// <summary>
        ///     开始
        /// </summary>
        void Start();
        /// <summary>
        ///     停止
        /// </summary>
        void Stop();
        /// <summary>
        ///    获取当前超时器的可用状态。
        /// </summary>
        bool Enable { get; set; }
    }
}
