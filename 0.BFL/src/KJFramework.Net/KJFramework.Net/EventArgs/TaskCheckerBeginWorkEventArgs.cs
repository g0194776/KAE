using System;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_TASKCHECKER_BEGINWORK(Object sender, TaskCheckerBeginWorkEventArgs e);
    /// <summary>
    ///     任务检查器开始工作事件
    /// </summary>
    /// <remarks>
    ///     当一个任务检查器开始工作时，触发该事件。
    /// </remarks>
    public class TaskCheckerBeginWorkEventArgs : System.EventArgs
    {
        /// <summary>
        ///     任务检查器开始工作事件
        /// </summary>
        /// <remarks>
        ///     当一个任务检查器开始工作时，触发该事件。
        /// </remarks>
        public TaskCheckerBeginWorkEventArgs()
        {
        }
    }
}
