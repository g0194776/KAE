using KJFramework.EventArgs;

namespace KJFramework.Timer
{
    /// <summary>
    ///     超时器元接口, 提供了相关的基本操作以及相关属性结构。
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        ///     尝试总数
        /// </summary>
        int TryCount { get; set; }
        /// <summary>
        ///     当前尝试次数
        /// </summary>
        int TryIndex { get; set; }
        /// <summary>
        ///      超时器超时事件
        /// </summary>
        /// <remarks>
        ///     当已经到达所指定的尝试次数，仍然未满足指定条件，则会触发该事件。
        /// </remarks>
        event DELEGATE_TIMEOUT Timeout;
    }
}
