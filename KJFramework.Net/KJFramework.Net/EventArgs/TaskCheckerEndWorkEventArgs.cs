using System;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_TASKCHECKER_ENDWORK(Object sender, TaskCheckerEndWorkEventArgs e);
    /// <summary>
    ///     任务检查器停止工作事件。
    /// </summary>
    /// <remarks>
    ///     当一个任务管理器停止工作时，触发该事件。停止工作的原因一般都是当属性IsActive = false。
    /// </remarks>
    public class TaskCheckerEndWorkEventArgs : System.EventArgs
    {
        /// <summary>
        ///     任务检查器停止工作事件。
        /// </summary>
        /// <remarks>
        ///     当一个任务管理器停止工作时，触发该事件。停止工作的原因一般都是当属性IsActive = false。
        /// </remarks>
        public TaskCheckerEndWorkEventArgs()
        {
        }
    }
}
