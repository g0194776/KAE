namespace KJFramework.Enums
{
    /// <summary>
    ///     超时器开始运行的计算方式
    /// </summary>
    public enum TimerStartType
    {
        /// <summary>
        ///     立即运行超时器。
        /// </summary>
        RunTimerNow,
        /// <summary>
        ///     等待事件间隔的第二次触发时才开始计算
        /// </summary>
        WaitOnce
    }
}
