using System;

namespace KJFramework.EventArgs
{
    public delegate void DELEGATE_TICK(Object sender, TickEventArgs e);
    /// <summary>
    ///     超时器触发间隔事件
    /// </summary>
    public class TickEventArgs : System.EventArgs
    {
    }
}
