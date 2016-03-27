using System;

namespace KJFramework.EventArgs
{
    public delegate void DELEGATE_TIMEOUT(Object sender, TimeoutEventArgs e);
    /// <summary>
    ///      超时器超时事件
    /// </summary>
    /// <remarks>
    ///     当已经到达所指定的尝试次数，仍然未满足指定条件，则会触发该事件。
    /// </remarks>
    public class TimeoutEventArgs : System.EventArgs
    {
    }
}
