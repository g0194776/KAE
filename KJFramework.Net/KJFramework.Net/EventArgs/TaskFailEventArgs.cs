using System;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_TASKFAIL(Object sender, TaskFailEventArgs e);
    /// <summary>
    ///     任务检查失败事件
    /// </summary>
    /// <remarks>
    ///     当任务检查线程退出，或者异常的时候触发此事件
    /// </remarks>
    public class TaskFailEventArgs : System.EventArgs
    {
        /// <summary>
        ///     任务检查失败事件
        /// </summary>
        /// <remarks>
        ///     当任务检查线程退出，或者异常的时候触发此事件
        /// </remarks>
        public TaskFailEventArgs()
        {
        }
    }
}
